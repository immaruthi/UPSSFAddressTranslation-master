using Microsoft.AspNetCore.Mvc;
using UPS.ServicesDataRepository;
using UPS.DataObjects.Shipment;
using Microsoft.EntityFrameworkCore;
using UPS.ServicesDataRepository.DataContext;
using UPS.Quincus.APP;
using UPS.Quincus.APP.Response;
using Microsoft.Extensions.Configuration;
using ExcelFileRead;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using UPS.AddressTranslationService.Controllers;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using UPS.DataObjects.WR_FLW;
using System.Data.SqlClient;
using AtService.Models;
using System;
using System.Net.Http.Headers;
using System.Collections.Generic;
using UPS.ServicesDataRepository.OverrideDbContext;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using UPS.ServicesDataRepository.Common;
using System.Security.Claims;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    [Authorize]
    public class ExcelWorkflowController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public ExcelWorkflowController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        public List<WorkflowDataRequest> UploadExcel(string Emp_Id)
        {
            List<WorkflowDataRequest> excelwkflow = new List<WorkflowDataRequest>();

            var file = Request.Form.Files[0];

            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            FileInfo file1 = null; ;
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            if (file.Length > 0)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string fullPath = Path.Combine(newPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                file1 = new FileInfo(Path.Combine(newPath, fileName));

                string result = SaveExcelFileInformation1(fileName,Emp_Id);
            }

            excelwkflow = ExcelData();
            //ImportDataFromExcel(file1);
            return excelwkflow;
        }

        public string SaveExcelFileInformation1(string excelFile,string EmpName)
        {
            int EMP_ID = GetEmpID(EmpName);
            SqlConnection con = new SqlConnection(DBConnectionContext.connectionString);
            string query = "INSERT INTO dbo.[WR-FLW]([FLE-NA], [WFL-STA-TE],[CRD-DT],[UDT-DT],[CRD-BY-NR]) VALUES('" + excelFile + "', 1 ,'" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "', "+ EMP_ID+")";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return "Success";
            }
            catch (SqlException e)
            {
                return "Failed";
            }

        }

        public int GetEmpID(string Empname)
        {
            SqlConnection conn = new SqlConnection(DBConnectionContext.connectionString);
            conn.Open();
            //string query = "select * from [WR-FLW] inner join [USR] on USR.ID = [WR-FLW].[CRD-BY-NR] ";
            string query = "select ID from [USR] where [USR].[USR-EML-TE] ='"+Empname+"'";
            SqlCommand command = new SqlCommand(query, conn);

            SqlDataReader reader = command.ExecuteReader();
            int EmpID = 0;
            while (reader.Read())
            {
                EmpID = Convert.ToInt32(reader["ID"]);
            }
            conn.Close();
            return EmpID;
        }

        //public List<WorkflowDataRequest> ExcelData()
        //{
        //    List<WorkflowDataRequest> excelWorkflowsLst = new List<WorkflowDataRequest>();


        //    SqlConnection conn = new SqlConnection(DBConnectionContext.connectionString);
        //    conn.Open();
        //    //string query = "select * from [WR-FLW] inner join [USR] on USR.ID = [WR-FLW].[CRD-BY-NR] ";
        //    string query = "select * from [WR-FLW] " +
        //        "inner join [USR] on USR.ID = [WR-FLW].[CRD-BY-NR] order by [WR-FLW].ID desc"; // USR.ID = (select [ID] from [USR] where [USR].[USR-EML-TE]= '"+ EmpID+"')";
        //    SqlCommand command = new SqlCommand(query, conn);

        //    SqlDataReader reader = command.ExecuteReader();
        //    //DataTable dataTable = new DataTable();

        //    //dataTable.Load(reader);
        //    //while (reader.HasRows)
        //    //{
        //    while (reader.Read())
        //    {
        //        WorkflowDataRequest exflow = new WorkflowDataRequest();
        //        exflow.ID = Convert.ToInt32(reader[0]);
        //        exflow.USR_FST_NA = reader["USR-FST-NA"].ToString();
        //        exflow.FLE_NA = reader["FLE-NA"].ToString();
        //        exflow.WFL_STA_TE = reader["WFL-STA-TE"] != null ? Convert.ToInt32(reader["WFL-STA-TE"].ToString()) : 0;
        //        exflow.CRD_DT = reader["CRD-DT"] != null && reader["CRD-DT"].ToString() != string.Empty ? Convert.ToDateTime(reader["CRD-DT"].ToString()): DateTime.Now;
        //        exflow.UDT_DT = reader["UDT-DT"] != null && reader["UDT-DT"].ToString() != string.Empty ? Convert.ToDateTime(reader["UDT-DT"].ToString()): DateTime.Now;

        //        if (exflow.WFL_STA_TE == 0)
        //        {
        //            exflow.WFL_STA_TE_TEXT = "Created"; //Uploaded
        //        }
        //        else if (exflow.WFL_STA_TE == 1 || exflow.WFL_STA_TE == 2)
        //        {
        //            exflow.WFL_STA_TE_TEXT = "InProgress"; //Curated || Translated
        //        }
        //        else if(exflow.WFL_STA_TE == 3)
        //        {
        //            exflow.WFL_STA_TE_TEXT = "Completed"; //Done
        //        }

        //        excelWorkflowsLst.Add(exflow);
        //    }


        //    // }
        //    conn.Close();


        //    return excelWorkflowsLst;

        //}

        public List<WorkflowDataRequest> ExcelData()
        {
            List<WorkflowDataRequest> excelWorkflowsLst = new List<WorkflowDataRequest>();


            SqlConnection conn = new SqlConnection(DBConnectionContext.connectionString);
            conn.Open();
            string query = "select * from [WR-FLW] inner join [USR] on USR.ID = [WR-FLW].[CRD-BY-NR] ";
            //string query = "select * from [WR-FLW] inner join [USR] on USR.ID = [WR-FLW].[CRD-BY-NR] where USR.ID = (select [ID] from [USR] where [USR].[USR-EML-TE]= '" + EmpID + "')";
            SqlCommand command = new SqlCommand(query, conn);

            SqlDataReader reader = command.ExecuteReader();
            //DataTable dataTable = new DataTable();

            //dataTable.Load(reader);
            //while (reader.HasRows)
            //{
            while (reader.Read())
            {
                WorkflowDataRequest exflow = new WorkflowDataRequest();
                exflow.ID = Convert.ToInt32(reader[0]);
                //exflow.USR_FST_NA = reader["USR-FST-NA"].ToString();
                exflow.FLE_NA = reader["FLE-NA"].ToString();
                exflow.WFL_STA_TE = Convert.ToInt32(reader["WFL-STA-TE"]);
                exflow.CRD_DT = reader["CRD-DT"] != null && reader["CRD-DT"].ToString() != string.Empty ? Convert.ToDateTime(reader["CRD-DT"].ToString()) : DateTime.Now;
                exflow.UDT_DT = reader["UDT-DT"] != null && reader["UDT-DT"].ToString() != string.Empty ? Convert.ToDateTime(reader["UDT-DT"].ToString()) : DateTime.Now;

                excelWorkflowsLst.Add(exflow);
            }


            // }
            conn.Close();


            return excelWorkflowsLst;

        }






        [HttpGet("[action]")]
        public List<WorkflowDataRequest> getExcelData()
        {
            List<WorkflowDataRequest> lst = new List<WorkflowDataRequest>();
            lst = ExcelData();
            return lst;
        }


        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
