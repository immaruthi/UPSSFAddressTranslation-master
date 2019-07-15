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

        UserDataResponse GetUserData();

        UserDataResponse UpdateUser(User userData);
    }
}
