using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AtService.Controllers;

namespace AtService.Models
{

    public class LoginContext
    {
        public string ConnectionString { get; set; }

        public LoginContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public bool ValidateUser(String Emp_Id, String pwd)
        {
            bool isUserExists = false;
            using (SqlConnection connection = new SqlConnection(GetConnectionString.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("select * from [USR] where [USR-EML-TE]='" + Emp_Id + "' and [USR-PWD-TE]='" + pwd + "'", connection);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return isUserExists;
        }

        public bool ValidateUserId(String Emp_Id)
        {
            bool isUserExists = false;
            using (SqlConnection connection = new SqlConnection(GetConnectionString.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("select * from [USR] where [USR-EML-TE]='" + Emp_Id + "'", connection);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return isUserExists;
        }
        public LoginData getLoginData(string Emp_Id)
        {
            //LoginData ld = new LoginData();
            //using (MySqlConnection conn = GetConnection())
            //{
            //    conn.Open();
            //    string query = "select Emp_Id,Last_Login_Date from pact_rmg_login_info  where Emp_Id like '%" + Emp_Id + "%'";
            //    MySqlCommand cmd = new MySqlCommand(query, conn);
            //    using (var reader = cmd.ExecuteReader())
            //    {
            //        reader.Read();

            //        ld.Emp_Id = reader["Emp_Id"].ToString();
            //        ld.Last_Login_Date = reader["Last_Login_Date"].ToString();



            //    }
            //}

            LoginData ld = new LoginData();

            using (SqlConnection connection = new SqlConnection(GetConnectionString.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("select * from [USR] where [USR-EML-TE]='" + Emp_Id + "'", connection);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ld.Emp_Id = ds.Tables[0].Rows[0]["USR-EML-TE"].ToString();
                    ld.Last_Login_Date = System.DateTime.Now.ToShortDateString();
                }
            }

            return ld;
        }


    }
}
