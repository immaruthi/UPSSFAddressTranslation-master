using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPS.ServicesDataRepository;
using UPS.DataObjects.Shipment;
using Microsoft.EntityFrameworkCore;
using UPS.ServicesDataRepository.DataContext;
using UPS.Quincus.APP;
using UPS.Quincus.APP.Response;
using Microsoft.Extensions.Configuration;

namespace AtService.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ShipmentController : ControllerBase
    {

        private readonly IConfiguration configuration;

        public ShipmentController(IConfiguration Configuration)
        {
            this.configuration = Configuration;
        }

        private ShipmentService shipmentService { get; set; }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult Post()
        {
            shipmentService = new ShipmentService();
            shipmentService.CreateShipment(new ShipmentDataRequest()
            {
                SHP_ADR_TE = "test1",
                WFL_ID = 1,
                SF_TRA_LG_ID = null,
                QQS_TRA_LG_ID = null
            });

            return Ok();
        }

        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult GetShipmentData(int wid)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var context = new ApplicationDbContext(optionsBuilder.Options);
            var shipments = context.shipmentDataRequests.Where(w => w.WFL_ID == wid).OrderBy(s => s.SHP_ADR_TE);
            return Ok(shipments);
        }


        [Route("UpdateShipmentCode")]
        [HttpPost]
        public async Task<ActionResult> UpdateShipmentCode([FromBody] ShipmentGeoCodes shipmentGeoCodes)
        {
            QuincusResponse quincusResponse = null;

            QuincusTokenDataResponse quincusTokenDataResponse = QuincusService.GetToken(new UPS.Quincus.APP.Configuration.QuincusParams()
            {
                endpoint = configuration["Quincus:TokenEndPoint"],
                password= configuration["Quincus:Password"],
                username= configuration["Quincus:UserName"],

            });

            if (quincusTokenDataResponse.ResponseStatus)
            {

                quincusResponse = QuincusService.GetGeoCodeReponseFromQuincus(new UPS.Quincus.APP.Request.QuincusGeoCodeDataRequest()
                {
                    endpoint = configuration["Quincus:GeoCodeEndPoint"],
                    id = shipmentGeoCodes.geoCode,
                    quincusTokenData = quincusTokenDataResponse.quincusTokenData
                });

                if(quincusTokenDataResponse.ResponseStatus)
                {

                    IList<Geocode> geocodes = quincusResponse.QuincusReponseData.geocode;

                    string TranslatedCode = geocodes[0].translated_adddress;

                    return Ok(quincusResponse.QuincusReponseData);
                }
            }
            else
            {
                return Ok(quincusTokenDataResponse.exception);
            }

            return Ok("Error");
        }
    }
}
