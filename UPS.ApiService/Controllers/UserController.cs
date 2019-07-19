using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            int loggedUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId).Value);
            string userResponse = this.userService.CreateUser(user, loggedUserId);
            
            JsonResult response = new JsonResult(userResponse);
            response.StatusCode =
                userResponse.Equals(ResponseConstant.Validation_Error) || userResponse.Equals(ResponseConstant.Create_Error)
                ?(int)HttpStatusCode.InternalServerError: (int)HttpStatusCode.OK;
            return response;
        }

        /// <summary>
        /// This Service is for getting All user with their roles and mapped cities
        /// If User has no roles in db then adding OperationUser as a default Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await this.userService.GetAllUser());
        }

        /// <summary>
        /// This Service is for updating User Info With their roles and city Mapping
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [Authorize(Roles = Constants.Role.Admin)]
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (user == null || user.ID == 0)
            {
                return BadRequest();
            }
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId).Value);
            string responseMessage =await this.userService.UpdateUser(user,userId);

            JsonResult response = new JsonResult(responseMessage);
            response.StatusCode =
               responseMessage.Equals(ResponseConstant.Update_Error) || responseMessage.Equals(ResponseConstant.Something_Went_Wrong)
               ? (int)HttpStatusCode.InternalServerError : (int)HttpStatusCode.OK;

            return response;
        }
    }
}