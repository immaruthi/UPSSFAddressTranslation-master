using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult Get()
        {

            SqlConnection connection = new SqlConnection(GetConnectionString.connectionString);

            connection.Open();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select * from [WR-FLW]", connection);

            DataSet ds = new DataSet();

            sqlDataAdapter.Fill(ds);

            return Ok(ds);
        }
    }
}