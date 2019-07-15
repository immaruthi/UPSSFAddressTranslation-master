using Microsoft.AspNetCore.Mvc;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;

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
        [Route("create")]
        public IActionResult CreatedUser(User user)
        {
            string userResponse = this.userService.CreateUser(user);
            return Ok(userResponse);
        }
    }
}