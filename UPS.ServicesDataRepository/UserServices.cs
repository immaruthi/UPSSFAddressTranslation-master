using System;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;

namespace UPS.ServicesDataRepository
{
    public class UserServices : IUserServicesAsync
    {
        public int CreateUser(UserData userData)
        {
            throw new NotImplementedException();
        }

        public int DeleteUser(UserData userData)
        {
            throw new NotImplementedException();
        }

        public UserDataResponse GetUserData()
        {
            throw new NotImplementedException();
        }

        public UserDataResponse UpdateUser(UserData userData)
        {
            string connectionString = AtServicesContext.ConnectionString;

            throw new NotImplementedException();
        }
    }
}
