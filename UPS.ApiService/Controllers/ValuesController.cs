using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using UPS.ServicesDataRepository.Common;

namespace UPS.AddressTranslationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    [Authorize]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string userIdText = HttpContext.User.Claims.FirstOrDefault(x => x.Type ==JwtConstant.UserIdText).Value;
            string Id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId).Value;

            return new string[] { "value1", "value2", "UserId:"+ Id, "UserIdText:"+ userIdText };
        }

    }
}
