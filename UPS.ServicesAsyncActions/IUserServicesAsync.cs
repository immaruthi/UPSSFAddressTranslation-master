using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.UserData;

namespace UPS.ServicesAsyncActions
{
    public interface IUserServicesAsync
    {
        int CreateUser(USR userData);

        int DeleteUser(USR userData);

        UserDataResponse GetUserData();

        UserDataResponse UpdateUser(USR userData);
    }
}
