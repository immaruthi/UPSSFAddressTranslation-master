using System;
using System.Data;
using System.Data.SqlClient;
using AtService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPS.DataObjects.UserData;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.OverrideDbContext;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class LoginController : ControllerBase
    {
        public LoginContext loginContext = null;
        public UserServices userServices = null;
        public LoginController()
        {
            userServices = new UserServices();
            loginContext = new LoginContext(DBConnectionContext.connectionString, userServices);
        }

        [HttpPost("[action]")]
        public ActionResult<UserDataResponse> ValidateUser([FromBody] LoginContextData loginContextData)
        {
            return loginContext.ValidateUser(loginContextData.USR_ID_TE, loginContextData.USR_PWD_TE);
        }

        //[HttpGet("[action]")]
        //public bool ValidateUserId(String userId)
        //{
        //    return loginContext.ValidateUserId(userId);
        //}
        //[HttpGet("[action]")]
        //public LoginData getLoginData(string Emp_Id)
        //{
        //    //LoginDataContext context = HttpContext.RequestServices.GetService(typeof(LoginDataContext)) as LoginDataContext;
        //    return loginContext.getLoginData(Emp_Id);
        //}


        //[HttpGet("[action]")]
        //public bool InsertUser(String firstName, String lastName, String userName, String password)
        //{

        //    SqlConnection sqlConnection = new SqlConnection(DBConnectionContext.connectionString);

        //    try
        //    {

        //        sqlConnection.Open();

        //        string strcmd = "Insert into USR([USR-FST-NA],[USR-LST-NA],[USR-EML-TE],[USR-PWD-TE],[IS-ACT-B]) values('" + firstName + "','" + lastName + "','" + userName + "','" + password + "',0)";

        //        SqlCommand sqlCommand = new SqlCommand(strcmd, sqlConnection);

        //        sqlCommand.ExecuteNonQuery();

        //        sqlConnection.Close();

        //        sqlConnection.Dispose();

        //    }
        //    catch(Exception ex)
        //    {
        //        sqlConnection.Dispose();
        //        return false;
        //    }

        //    //LoginContext context = HttpContext.RequestServices.GetService(typeof(LoginContext)) as LoginContext;
        //    return true; //context.ValidateUserId(userId);
        //}

    }
}