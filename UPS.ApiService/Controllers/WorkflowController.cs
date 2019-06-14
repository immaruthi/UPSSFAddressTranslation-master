using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AtService.Models;
using Microsoft.AspNetCore.Mvc;

namespace UPS.AddressTranslationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        // GET: api/<controller>
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
        // GET: api/<controller>
        [HttpPost]
        public ActionResult Post()
        {

            SqlConnection connection = new SqlConnection(GetConnectionString.connectionString);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

            DataSet ds = new DataSet();
            string sql = null;
            sql = "insert into [WR-FLW] ([FLE-NA]) values('test1')";
            try
            {
                connection.Open();
                sqlDataAdapter.InsertCommand = new SqlCommand(sql, connection);
                sqlDataAdapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            sqlDataAdapter.Fill(ds);
            return this.Get();
        }
    }
}