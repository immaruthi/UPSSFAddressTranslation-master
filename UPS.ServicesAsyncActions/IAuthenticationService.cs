using System;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;

namespace UPS.ServicesAsyncActions
{
    public interface IUPSAuthenticationService
    {
        Task<UserDataResponse> AuthenticateUserAsync(String USR_ID_TE, String USR_PWD_TE);
        string GenerateValidationToken(string userIdText, int? Id =0);
    }
}