using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;

namespace UPS.ServicesAsyncActions
{
    public interface IUserServicesAsync
    {
        int CreateUser(USR userData);

        int DeleteUser(USR userData);

        UserDataResponse GetUserData();

        UserDataResponse UpdateUser(USR userData);
        UserDataResponse ValidateUser(String USR_ID_TE, String USR_PWD_TE);
        Task<UserDataResponse> ValidateUserAsync(String USR_ID_TE, String USR_PWD_TE);
    }
}
