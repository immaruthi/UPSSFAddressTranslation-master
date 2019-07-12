using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UPS.DataObjects.AddressBook;
using UPS.DataObjects.ADR_ADT_LG;
using UPS.ServicesAsyncActions;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class AddressBookController : ControllerBase
    {
        private IAddressBookService addressBookService;
        private IAddressAuditLogAsync addressAuditLogService;
        public AddressBookController(IAddressBookService addressBookService, IAddressAuditLogAsync addressAuditLogAsync)
        {
            this.addressBookService = addressBookService;
            this.addressAuditLogService = addressAuditLogAsync;
        }

        /// <summary>
        /// This service is for getting all addresses from addressBook
        /// </summary>
        /// <returns>List of addresses </returns>
        [HttpGet]
        [Route("getall")]
        public IActionResult GetAddressBooks()
        {
            List<AddressBook> addressBooks = this.addressBookService.GetAddressBooks();
            return Ok(addressBooks);
        }

        [Route("UpdateAddressBookById/{Emp_Id}")]
        [HttpPost]
        public IActionResult UpdateAddressBookById([FromBody] AddressBook addressBookData, int Emp_Id)
        {
            AddressBookResponse addressBookResponse = this.addressBookService.UpdateAddressBookById(addressBookData);
            if (addressBookResponse.Success && !string.IsNullOrEmpty(addressBookResponse.BeforeAddress))
            {
                try
                {
                    //AddressAuditLog Update
                    AddressAuditLogRequest addressAuditLogRequest = new AddressAuditLogRequest();
                    addressAuditLogRequest.SMT_ID = addressBookResponse.AddressBookData.ShipmentId;
                    addressAuditLogRequest.CSG_ADR = addressBookResponse.AddressBookData.ConsigneeAddress;
                    addressAuditLogRequest.BFR_ADR = addressBookResponse.BeforeAddress;
                    addressAuditLogRequest.AFR_ADR = addressBookData.ConsigneeTranslatedAddress;
                    addressAuditLogRequest.UPD_BY = Emp_Id;
                    addressAuditLogRequest.UPD_FRM = "AddressBook";
                    addressAuditLogRequest.UPD_DT = DateTime.Now;
                    AddressAuditLogResponse addressAuditLogResponse = addressAuditLogService.Insert(addressAuditLogRequest);
                    if (addressAuditLogResponse.Success)
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

            }
            return Ok(addressBookResponse);
        }

    }
}