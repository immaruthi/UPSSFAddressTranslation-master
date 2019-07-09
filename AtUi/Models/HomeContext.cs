using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtUi.Models
{
    //public class HomeContext
    //{
    //    public string ConnectionString { get; set; }

    //    public HomeContext(string connectionString)
    //    {
    //        this.ConnectionString = connectionString;
    //    }
    //    private MySqlConnection GetConnection()
    //    {
    //        return new MySqlConnection(ConnectionString);
    //    }
    //    public int GetallEmployeeCount()
    //    {
    //        int empCount;
    //        using (MySqlConnection conn = GetConnection())
    //        {
    //            conn.Open();
    //            string query = "select count(*) as emp_count from pact_rmg.pact_rmg_employee_mst where flag=1;";
    //            MySqlCommand cmd = new MySqlCommand(query, conn);
    //            using (var reader = cmd.ExecuteReader())
    //            {
    //                reader.Read();
    //                empCount = Convert.ToInt32(reader["emp_count"]);      
    //            }
    //        }
    //        return empCount;
    //    }
    //    public int GetallEmpProjCount()
    //    {
    //        int empProj;
    //        using (MySqlConnection conn = GetConnection())
    //        {
    //            conn.Open();
    //            string query = "select count(*) as empProj_count from pact_rmg.pact_rmg_emp_proj where flag=1;";
    //            MySqlCommand cmd = new MySqlCommand(query, conn);
    //            using (var reader = cmd.ExecuteReader())
    //            {
    //                reader.Read();
    //                empProj = Convert.ToInt32(reader["empProj_count"]);
    //            }
    //        }
    //        return empProj;
    //    }
    //    public int GetallEmpBenchCount()
    //    {
    //        int empBenchcount;
    //        using (MySqlConnection conn = GetConnection())
    //        {
    //            conn.Open();
    //            string query = "select count(*) as empbench_count from pact_rmg.pact_rmg_emp_proj where flag=1 && billable=\"yes\"";
    //            MySqlCommand cmd = new MySqlCommand(query, conn);
    //            using (var reader = cmd.ExecuteReader())
    //            {
    //                reader.Read();
    //                empBenchcount = Convert.ToInt32(reader["empbench_count"]);
    //            }
    //        }
    //        return empBenchcount;
    //    }
    //    public int GetallProjCount()
    //    {
    //        int Projcount;
    //        using (MySqlConnection conn = GetConnection())
    //        {
    //            conn.Open();
    //            string query = "select count(*) as proj_count from pact_rmg.pact_rmg_projects_mst where flag=1;";
    //            MySqlCommand cmd = new MySqlCommand(query, conn);
    //            using (var reader = cmd.ExecuteReader())
    //            {
    //                reader.Read();
    //                Projcount = Convert.ToInt32(reader["proj_count"]);
    //            }
    //        }
    //        return Projcount;
    //    }
    //    public int GetallCustomerCount()
    //    {
    //        int custcount;
    //        using (MySqlConnection conn = GetConnection())
    //        {
    //            conn.Open();
    //            string query = "select count(*) as cust_count from pact_rmg.pact_rmg_customer_mst where flag=1;";
    //            MySqlCommand cmd = new MySqlCommand(query, conn);
    //            using (var reader = cmd.ExecuteReader())
    //            {
    //                reader.Read();
    //                custcount = Convert.ToInt32(reader["cust_count"]);
    //            }
    //        }
    //        return custcount;
    //    }
    //}
}
