
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using AtUi.Models;

namespace RMG.Models
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

                SqlCommand cmd = new SqlCommand("select * from [USR] where [USR-EML-TE]='"+ Emp_Id + "'",connection);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                if(ds.Tables[0].Rows.Count>0)
                {
                    return true;
                }
            }
            return isUserExists;
        }


    }
}
