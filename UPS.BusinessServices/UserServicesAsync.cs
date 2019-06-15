using System;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;
using UPS.ServicesDataRepository;

namespace UPS.BusinessServices
{
    public class UserServicesAsync
    {
        private static UserServices userServices { get; set; }
        public static UserServicesAsync userServicesAsync { get; set; }

        static UserServicesAsync()
        {
            userServices = new UserServices();
            userServicesAsync = new UserServicesAsync();
        }

        public async Task<UserDataResponse> GetUserDataResponse(int? productID = null)
        {
            return await Task<UserDataResponse>.Run(() => userServices.GetUserData());
        }



    }
}
