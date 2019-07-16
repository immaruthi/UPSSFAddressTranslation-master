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
using AtService.CustomConatiner;
using AtService.HeadController;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UPS.ServicesDataRepository.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UPS.AddressTranslationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class ValuesController : UPSController
    {

        //public ValuesController()
        //{
        //    IoCContainer.BuildUp(this);
        //}

        public ICustomLog iCustomLog { get; set; }

        // GET: api/<controller>
        //[Authorize(AuthenticationSchemes = "JwtBearer")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //iCustomLog.AddLogEntry(new DataObjects.LogData.LogDataModel()
            //{
            //    apiTypes = DataObjects.LogData.APITypes.AddressBookSetup,
            //    dateTime = System.DateTime.Now,
            //    LogInformation = new DataObjects.LogData.LogInformation()
            //    {
            //        LogException = new Exception("Test Exception"),
            //        LogRequest = "Test Request",
            //        LogResponse = "Test Response"
            //    }
            //});

            return new string[] { "value1", "value2" };
        }

        [HttpGet("[action]")]
        public bool ValidateUser(String userId, String password)
        {
            return true; 
        }

        [HttpGet("[action]")]
        public bool ValidateUserId(String userId)
        {
            //LoginContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.LoginContext)) as LoginContext;
            return true;//context.ValidateUserId(userId);
        }
        [HttpGet("[action]")]
        public LoginData getLoginData(string Emp_Id)
        {
            //LoginDataContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.LoginDataContext)) as LoginDataContext;

            LoginData loginData = new LoginData()
            {
                Emp_Id = Emp_Id,
                Last_Login_Date = System.DateTime.Now.ToString()
            };

            return loginData;//context.getLoginData(Emp_Id);
        }
    }
}
