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

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
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
    }
}
