using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.UserData;

namespace UPS.ServicesAsyncActions
{
    public interface IUserServicesAsync
    {
        int CreateUser(UserData userData);

        int DeleteUser(UserData userData);

        UserDataResponse GetUserData();

        UserDataResponse UpdateUser(UserData userData);
    }
}
