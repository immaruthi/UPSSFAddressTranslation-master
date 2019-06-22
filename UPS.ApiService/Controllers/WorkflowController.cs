using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AtService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.AddressTranslationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("SiteCorsPolicy")]
    public class WorkflowController : ControllerBase
    {
        private WorkflowService workflowService {get; set;}
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
        public int Post(IFormFile fromFile, int Emp_Id)
        {
            int wid = 0;
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.FLE_NA = fromFile.FileName;
            workflowDataRequest.CRD_BY_NR = Emp_Id;
            workflowDataRequest.CRD_DT = DateTime.Now.ToString();
            workflowDataRequest.WFL_STA_TE = 1;
            workflowService = new WorkflowService();
            wid = workflowService.CreateWorkflow(workflowDataRequest);
            //SqlConnection connection = new SqlConnection(GetConnectionString.connectionString);

            //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

            //DataSet ds = new DataSet();
            //string sql = null;
            //sql = "insert into [WR-FLW] ([FLE-NA]) values('test1')";
            //try
            //{
            //    connection.Open();
            //    sqlDataAdapter.InsertCommand = new SqlCommand(sql, connection);
            //    sqlDataAdapter.InsertCommand.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //}
            //sqlDataAdapter.Fill(ds);
            //return this.Get();
            return wid;
        }

        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public WorkflowDataRequest GetWorkflowData(string filename)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var context = new ApplicationDbContext(optionsBuilder.Options);
            WorkflowDataRequest workflow = context.workflowDataRequests.Where(w => w.FLE_NA == filename).FirstOrDefault();
            return workflow;
        }
    }
}