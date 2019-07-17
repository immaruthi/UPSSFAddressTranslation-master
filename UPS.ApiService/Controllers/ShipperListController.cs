using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.SPC_LST;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.Common;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShipperListController : ControllerBase
    {
        private IShipperCompanyAsync _shipperCompnayService { get; set; }

        public ShipperListController(IShipperCompanyAsync shipperCompanyAsync)
        {
            this._shipperCompnayService = shipperCompanyAsync;
        }

        [Route("GetShipmentList")]
        [HttpGet]
        public ShipperCompanyResponse GetShipmentData()
        {
            ShipperCompanyResponse shipperCompanyResponse = this._shipperCompnayService.GetShipperList();
            return shipperCompanyResponse;
        }

        [Route("CreateShipmentList")]
        [HttpPost]
        public async Task<ActionResult> CreateShipmentList([FromBody] ShipperCompanyList shipperCompanyRequest)
        {
            ShipperCompanyResponse shipperCompanyResponse = _shipperCompnayService.InsertShipper(shipperCompanyRequest);
            try
            {
                if (shipperCompanyResponse.Success)
                {
                    // TO DO
                }
                else
                {
                    // Log the error here
                }

            }
            catch (Exception ex)
            {
            }

            return Ok(shipperCompanyResponse);
        }

        [Route("UpdateShipperListById")]
        [HttpPost]
        public async Task<ActionResult> UpdateShipmentListById([FromBody] List<ShipperCompanyList> shipperCompanyRequests)
        {
            ShipperCompanyResponse shipperCompanyResponse = _shipperCompnayService.UpdateShipper(shipperCompanyRequests);
            try
            {
                if (shipperCompanyResponse.Success)
                {
                    // TO DO
                }
                else
                {
                    // Log the error here
                }

            }
            catch (Exception ex)
            {
            }

            return Ok(shipperCompanyResponse);
        }

        [Route("DeleteShipperListById")]
        [HttpPost]
        public async Task<ActionResult> DeleteShipmentListById([FromBody] List<ShipperCompanyList> shipperCompanyRequests)
        {
            ShipperCompanyResponse shipperCompanyResponse = _shipperCompnayService.DeleteShipper(shipperCompanyRequests);
            try
            {
                if (shipperCompanyResponse.Success)
                {
                    // TO DO
                }
                else
                {
                    // Log the error here
                }

            }
            catch (Exception ex)
            {
            }

            return Ok(shipperCompanyResponse);
        }

        [HttpGet]
        [Route("cities")]
        public async Task<ActionResult> GetShipmentCompanyCities()
        {
            List<string> cities = await this._shipperCompnayService.GetShipmentCompanyCities();
            return Ok(cities);
        }
    }
}