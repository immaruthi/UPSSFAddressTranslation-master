using AtUi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http.Headers;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RMG.Controllers
{
    [Route("api/[controller]")]
    public class ExcelWorkflowController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public ExcelWorkflowController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        public List<ExcelWorkflow> UploadExcel(string Emp_Id)
        {
            List<ExcelWorkflow> excelwkflow = new List<ExcelWorkflow>();

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

            excelwkflow = ExcelData(Emp_Id);
            //ImportDataFromExcel(file1);
            return excelwkflow;
        }

        public string SaveExcelFileInformation1(string excelFile,string EmpName)
        {
            int EMP_ID = GetEmpID(EmpName);
            SqlConnection con = new SqlConnection(GetConnectionString.connectionString);
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
            SqlConnection conn = new SqlConnection(GetConnectionString.connectionString);
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

        public List<ExcelWorkflow> ExcelData(string EmpID)
        {
            List<ExcelWorkflow> excelWorkflowsLst = new List<ExcelWorkflow>();


            SqlConnection conn = new SqlConnection(GetConnectionString.connectionString);
            conn.Open();
            //string query = "select * from [WR-FLW] inner join [USR] on USR.ID = [WR-FLW].[CRD-BY-NR] ";
            string query = "select * from [WR-FLW] inner join [USR] on USR.ID = [WR-FLW].[CRD-BY-NR] where USR.ID = (select [ID] from [USR] where [USR].[USR-EML-TE]= '"+ EmpID+"')";
            SqlCommand command = new SqlCommand(query, conn);

            SqlDataReader reader = command.ExecuteReader();
            //DataTable dataTable = new DataTable();

            //dataTable.Load(reader);
            //while (reader.HasRows)
            //{
            while (reader.Read())
            {
                ExcelWorkflow exflow = new ExcelWorkflow();
                exflow.ID = Convert.ToInt32(reader[0]);
                exflow.USR_FST_NA = reader["USR-FST-NA"].ToString();
                exflow.FLE_NA = reader["FLE-NA"].ToString();
                exflow.WFL_STA_TE = Convert.ToInt32(reader["WFL-STA-TE"]);
                exflow.CRD_DT = reader["CRD-DT"].ToString();
                exflow.UDT_DT = reader["UDT-DT"].ToString();

                excelWorkflowsLst.Add(exflow);
            }


            // }
            conn.Close();


            return excelWorkflowsLst;

        }

        public List<ExcelWorkflow> ExcelData()
        {
            List<ExcelWorkflow> excelWorkflowsLst = new List<ExcelWorkflow>();


            SqlConnection conn = new SqlConnection(GetConnectionString.connectionString);
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
                ExcelWorkflow exflow = new ExcelWorkflow();
                exflow.ID = Convert.ToInt32(reader[0]);
                exflow.USR_FST_NA = reader["USR-FST-NA"].ToString();
                exflow.FLE_NA = reader["FLE-NA"].ToString();
                exflow.WFL_STA_TE = Convert.ToInt32(reader["WFL-STA-TE"]);
                exflow.CRD_DT = reader["CRD-DT"].ToString();
                exflow.UDT_DT = reader["UDT-DT"].ToString();

                excelWorkflowsLst.Add(exflow);
            }


            // }
            conn.Close();


            return excelWorkflowsLst;

        }






        [HttpGet("[action]")]
        public List<ExcelWorkflow> getExcelData(string Emp_Id)
        {
            List<ExcelWorkflow> lst = new List<ExcelWorkflow>();
            lst = ExcelData(Emp_Id);
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
