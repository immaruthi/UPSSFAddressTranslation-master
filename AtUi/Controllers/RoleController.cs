
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMG.Models;

namespace RMG.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {

        [HttpGet("[action]")]
        public IEnumerable<RoleAttribute> GetAllRoles()
        {
            RoleContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.RoleContext)) as RoleContext;
            return context.GetAllRoles();
        }



        [HttpPost("[action]")]
        //  [Route("api/RoleAttribute/Create")]
        public int Create([FromBody] List<RoleAttribute> Role)
        {
            RoleContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.RoleContext)) as RoleContext;

            foreach (RMG.Models.RoleAttribute e in Role)
            {
                context.AddRole(e);
            }
            return 1;


        }

        //  [HttpPut("[action]")]
        //  public int Edit([FromBody] List<RoleAttributes> Role)
        // {
        //   RoleContext context = HttpContext.RequestServices.GetService(typeof(Roles.Models.RoleContext)) as RoleContext;

        // foreach (Roles.Models.RoleAttributes e in Role)
        //{
        //  context.UpdateRole(e);
        // }
        // return 1;
        //}


        [HttpPut("[action]")]
        // [Route("api/FetchEmployeeData/UpdateEmployees")]
        public int UpdateRole([FromBody]RoleAttribute role)
        {
            RoleContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.RoleContext)) as RoleContext;
            context.UpdateRole(role);
            return 1;
        }

        [HttpGet("[action]")]
        public void DisableRole(string Employee_Id)
        {
            RoleContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.RoleContext)) as RoleContext;
            context.DisableRole(Employee_Id);
        }

        [HttpGet("[action]")]
        public IEnumerable<EmpDropAttribute> GetEmployeeDropdown()
        {
            RoleContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.RoleContext)) as RoleContext;
            return context.GetEmployeeDropdown();
        }
    }
}




