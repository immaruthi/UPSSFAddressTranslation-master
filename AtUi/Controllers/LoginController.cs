using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMG.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AtUi.Models;

namespace RMG.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        


        [HttpGet("[action]")]
        public bool ValidateUser(String userId, String password)
        {
            LoginContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.LoginContext)) as LoginContext;
            return context.ValidateUser(userId, password);
        }



        [HttpGet("[action]")]
        public bool ValidateUserId(String userId)
        {
            LoginContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.LoginContext)) as LoginContext;
            return context.ValidateUserId(userId);
        }
        [HttpGet("[action]")]
        public LoginData getLoginData(string Emp_Id)
        {
            LoginDataContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.LoginDataContext)) as LoginDataContext;
            return context.getLoginData(Emp_Id);
        }


        [HttpGet("[action]")]
        public bool InsertUser(String firstName, String lastName, String userName, String password)
        {

            SqlConnection sqlConnection = new SqlConnection(GetConnectionString.connectionString);

            try
            {

                sqlConnection.Open();

                string strcmd = "Insert into USR([USR-FST-NA],[USR-LST-NA],[USR-EML-TE],[USR-PWD-TE],[IS-ACT-B]) values('" + firstName + "','" + lastName + "','" + userName + "','" + password + "',0)";

                SqlCommand sqlCommand = new SqlCommand(strcmd, sqlConnection);

                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();

                sqlConnection.Dispose();

            }
            catch(Exception ex)
            {
                sqlConnection.Dispose();
                return false;
            }

            //LoginContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.LoginContext)) as LoginContext;
            return true; //context.ValidateUserId(userId);
        }

    }
}