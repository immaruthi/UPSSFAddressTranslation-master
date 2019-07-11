using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPS.DataObjects.SPC_LST;
using UPS.ServicesDataRepository;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperListController : ControllerBase
    {
        private ShipperCompnayService shipperCompnayService { get; set; }

        [Route("GetShipmentList")]
        [HttpGet]
        public ShipperCompanyResponse GetShipmentData()
        {
            shipperCompnayService = new ShipperCompnayService();
            ShipperCompanyResponse shipperCompanyResponse = shipperCompnayService.GetShipperList();
            return shipperCompanyResponse;
        }
    }
}