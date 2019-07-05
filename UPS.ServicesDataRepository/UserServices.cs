using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class UserServices : IUserServicesAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        private UserDataResponse userDataResponse;
        public int CreateUser(USR userData)
        {
            throw new NotImplementedException();
        }

        public int DeleteUser(USR userData)
        {
            throw new NotImplementedException();
        }

        public UserDataResponse GetUserData()
        {
            throw new NotImplementedException();
        }

        public UserDataResponse SelectUserByUserIdAndPassword(String USR_ID_TE, String USR_PWD_TE)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            userDataResponse = new UserDataResponse();
            try
            {
                var context = new ApplicationDbContext(optionsBuilder.Options);
                USR user = context.UserData.Where(u => u.USR_ID_TE == USR_ID_TE && u.USR_PWD_TE == USR_PWD_TE).FirstOrDefault();
                userDataResponse.Success = false;
                if(user != null)
                {
                    userDataResponse.Success = true;
                }
                userDataResponse.User = user;
            }
            catch(Exception ex)
            {
                userDataResponse.Success = false;
                userDataResponse.OperationException = ex;
            }
            return userDataResponse;
        }

        public UserDataResponse UpdateUser(USR userData)
        {
            string connectionString = AtServicesContext.ConnectionString;

            throw new NotImplementedException();
        }
    }
}
