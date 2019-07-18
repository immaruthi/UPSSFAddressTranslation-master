using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;

namespace UPS.ServicesAsyncActions
{
    public interface IUserService
    {
        string CreateUser(User userData);

        int DeleteUser(User userData);

        Task<List<User>> GetAllUser();
        Task<int> UpdateUser(User user, int loggedUserId);
        UserDataResponse GetUserData();
        
    }
}
