namespace UPS.ServicesDataRepository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UPS.ServicesAsyncActions;
    using UPS.ServicesDataRepository.DataContext;
    using UPS.ServicesDataRepository.Common;
    using Microsoft.EntityFrameworkCore;
    using UPS.DataObjects.AddressBook;

    public class AddressBookService : IAddressBookAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        //private ApplicationDbContext context;
        //public AddressBookService(ApplicationDbContext applicationDbContext)
        //{
        //    this.context = applicationDbContext;
        //}
        //public void InsertAddress()
        //{
        //    throw new NotImplementedException();
        //}

        public AddressBookResponse GetAddressBookData()
        {
            AddressBookResponse addressBookResponse = new AddressBookResponse();
            //List<AddressBookRequest> AddressBookRequests = new List<AddressBookRequest>();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    addressBookResponse.AddressBookList = context.AddressBookRequests;
                    addressBookResponse.Success = true;
                    return addressBookResponse;
                }
                catch (Exception ex)
                {
                    addressBookResponse.Success = false;
                    addressBookResponse.OperatonExceptionMessage = ex.Message;
                }
            }
            return addressBookResponse;
        }
    }
}
