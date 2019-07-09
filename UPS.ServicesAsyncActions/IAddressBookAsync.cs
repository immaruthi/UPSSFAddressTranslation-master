using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.AddressBook;

namespace UPS.ServicesAsyncActions
{
   public interface IAddressBookAsync
    {
        // void InsertAddress();
        AddressBookResponse GetAddressBookData();
    }
}
