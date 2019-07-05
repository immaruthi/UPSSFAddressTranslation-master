using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using AtService.Models;
using UPS.ServicesDataRepository;
using UPS.Application.CustomLogs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UPS.ServicesDataRepository.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UPS.AddressTranslationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    [Authorize]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string userIdText = HttpContext.User.Claims.FirstOrDefault(x => x.Type ==JwtConstant.UserIdText).Value;
            string Id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId).Value;

            return new string[] { "value1", "value2", "UserId:"+ Id, "UserIdText:"+ userIdText };
        }

    }
}
