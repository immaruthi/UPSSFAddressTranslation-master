using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.AddressBook
{
    public class AddressBookResponse
    {
        public IEnumerable<AddressBookRequest> AddressBookList { get; set; }
        public string OperatonExceptionMessage { get; set; }
        public bool Success { get; set; }
    }
}
