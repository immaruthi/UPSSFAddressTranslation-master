using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        public UserController(IUserService  userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// This service is for creating User
        /// </summary>
        /// <param name="user">User Information</param>
        /// <returns></returns>
        
        [HttpPost]
        [Authorize(Roles = Constants.Role.Admin)]
        [Route("create")]
        public IActionResult CreatedUser(User user)
        {
            string userResponse = this.userService.CreateUser(user);
            
            JsonResult response = new JsonResult(userResponse);
            response.StatusCode =
                userResponse.Equals(ResponseConstant.Validation_Error) || userResponse.Equals(ResponseConstant.Create_Error)
                ?(int)HttpStatusCode.InternalServerError: (int)HttpStatusCode.OK;
            return response;
        }
    }
}