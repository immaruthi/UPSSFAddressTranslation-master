using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.AddressBook;
using UPS.DataObjects.Common;

namespace UPS.ServicesAsyncActions
{
   public interface IAddressBookService
    {
        void InsertAddress(List<QuincusReponseData> quincusReponseData);
        List<AddressBook> GetAddressBooks();
        AddressBookResponse UpdateAddressBookById(AddressBook addressBookData);
    }
}
