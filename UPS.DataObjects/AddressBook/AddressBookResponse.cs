using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.AddressBook
{
    public class AddressBookResponse
    {
        public IEnumerable<AddressBook> AddressBookList { get; set; }
        public AddressBook AddressBookData { get; set; }
        public string OperatonExceptionMessage { get; set; }
        public bool Success { get; set; }
        public string BeforeAddress { get; set; }
    }
}
