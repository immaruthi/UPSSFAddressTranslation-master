using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UPS.DataObjects.AddressBook;
using UPS.ServicesAsyncActions;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    [Authorize]
    public class AddressBookController : ControllerBase
    {
        private IAddressBookService addressBookService;
        public AddressBookController(IAddressBookService addressBookService)
        {
            this.addressBookService = addressBookService;
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

        [Route("UpdateAddressBookById")]
        [HttpPost]
        public IActionResult UpdateAddressBookById([FromBody] AddressBook addressBookData)
        {
            AddressBookResponse addressBookResponse = this.addressBookService.UpdateAddressBookById(addressBookData);
            
            return Ok(addressBookResponse);
        }

    }
}