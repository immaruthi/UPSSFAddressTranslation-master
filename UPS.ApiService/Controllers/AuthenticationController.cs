using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UPS.DataObjects.Login;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;

namespace AtService.Controllers
{
    [EnableCors("AllowAtUIOrigin")]
    public class AuthenticationController : Controller
    {
        private readonly IUPSAuthenticationService authenticationService;

        public AuthenticationController(IUPSAuthenticationService uPSAuthenticationService)
        {
            this.authenticationService = uPSAuthenticationService;
        }

        /// <summary>
        /// This method is for login valid user into application
        /// </summary>
        /// <param name="loginRequest">User Id and Password</param>
        /// <returns>Returns User Id and JWT token</returns>
        [Route("api/Login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            UserDataResponse userResponse = await authenticationService.AuthenticateUserAsync(loginRequest.USR_ID_TE, loginRequest.USR_PWD_TE);
            if (userResponse.Success)
            {
                string token = authenticationService.GenerateValidationToken(userResponse.User.USR_ID_TE, userResponse.User.ID);
                LoginDataResponse loginDataResponse =
                    new LoginDataResponse()
                    {
                        Token = token,
                        UserName = string.Format("{0} {1}", userResponse.User.USR_FST_NA, userResponse.User.USR_LST_NA)
                    };

                return Ok(loginDataResponse);
            }

            return Unauthorized();
        }

    }
}
