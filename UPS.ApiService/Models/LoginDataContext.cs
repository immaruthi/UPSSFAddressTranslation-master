using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPS.AddressTranslationService.Models
{
    public class LoginDataContext
    {
        public string ConnectionString { get; set; }

        public LoginDataContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        //private MySqlConnection GetConnection()
        //{
        //    return new MySqlConnection(ConnectionString);
        //}
        //public bool ValidateUser(String Emp_Id, String pwd)
        //{
        //    bool isUserExists = false;
        //    using (MySqlConnection connection = GetConnection())
        //    {
        //        connection.Open();
        //        MySqlCommand cmd = new MySqlCommand("sp_ValidateUser", connection);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("p_user_id", Emp_Id);
        //        cmd.Parameters.AddWithValue("p_pwd", pwd);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                int i = Convert.ToInt32(reader["record_count"]);
        //                isUserExists = i == 1 ? true : false;

        //            }
        //        }
        //    }
        //    return isUserExists;
        //}

        //public bool ValidateUserId(String Emp_Id)
        //{
        //    bool isUserExists = false;
        //    using (MySqlConnection connection = GetConnection())
        //    {
        //        connection.Open();
        //        MySqlCommand cmd = new MySqlCommand("sp_ValidateUserId", connection);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("p_id", Emp_Id);


        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                int i = Convert.ToInt32(reader["user_record_count"]);
        //                isUserExists = i == 1 ? true : false;

        //            }
        //        }
        //    }
        //    return isUserExists;
        //}
    }
}
