using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPS.DataObjects.AddressBook;
using UPS.ServicesDataRepository;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private AddressBookService addressBookService { get; set; }

        [Route("GetAddressBookData")]
        [HttpGet]
        public AddressBookResponse GetAddressBookData()
        {
            addressBookService = new AddressBookService();
            AddressBookResponse addressBookResponse = addressBookService.GetAddressBookData();
            return addressBookResponse;
        }
    }
}