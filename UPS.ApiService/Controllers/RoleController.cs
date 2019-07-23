using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        /// This sevice is for getting all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetRoles()
        {
            List<UpsRoles> upsRoles = await this.roleService.GetAllRoles();
            return Ok(upsRoles);
        }

        /// <summary>
        /// This service is for adding roles
        /// </summary>
        /// <param name="upsRoles"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public IActionResult AddRoles(UpsRoles upsRoles)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId);
            if (userId == null)
                return Unauthorized();
            upsRoles.CreatedBy = Convert.ToInt32(userId.Value);
            upsRoles.CreatedDate = DateTime.Now;
            upsRoles.IsActive = true;

            string responseMessage = this.roleService.AddRole(upsRoles);
            JsonResult response = new JsonResult(responseMessage);

            response.StatusCode =
                responseMessage.Contains(ResponseConstant.Create_Error)
                || responseMessage.Contains(ResponseConstant.Something_Went_Wrong)
                ? (int)HttpStatusCode.InternalServerError
                : (int)HttpStatusCode.OK;

            return response;
        }

        /// <summary>
        /// This service is for updating roles
        /// </summary>
        /// <param name="upsRoles"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public IActionResult UpdateRoles(UpsRoles upsRoles)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId);
            if (userId == null)
                return Unauthorized();
            upsRoles.UpdatedBy = Convert.ToInt32(userId.Value);

            string responseMessage = this.roleService.UpdateRole(upsRoles);
            JsonResult response = new JsonResult(responseMessage);

            response.StatusCode =
                responseMessage.Contains(ResponseConstant.Update_Success)
                ? (int)HttpStatusCode.OK
                : (int)HttpStatusCode.InternalServerError;

            return response;
        }

        /// <summary>
        /// This service is for deleting role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteRoles(int id)
        {
            return this.roleService.DeleteRole(id);
        }
    }
}