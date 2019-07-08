using System;
using System.Collections.Generic;
using System.Text;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class AddressBookService : IAddressBookService
    {
        private ApplicationDbContext context;
        public AddressBookService(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }
        public void InsertAddress()
        {
            throw new NotImplementedException();
        }
    }
}
