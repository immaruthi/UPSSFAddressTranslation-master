using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
namespace RMG.Models
{
    public class RoleContext
    {
        public string ConnectionString { get; set; }

        public RoleContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public List<RoleAttribute> GetAllRoles()
        {
            List<RoleAttribute> list = new List<RoleAttribute>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_GetAllRoles", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;



                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new RoleAttribute()
                        {
                            Employee_Id = reader["Employee_Id"].ToString(),
                            Employee_Name = reader["Emp_Name"].ToString(),
                            Role_Designation = reader["Role_Designation"].ToString(),
                            Role_Description = reader["Role_Description"].ToString(),
                            Role_Status = reader["Role_Status"].ToString(),
                            Role_StartDate = reader["Role_StartDate"].ToString(),
                            Role_EndDate = reader["Role_EndDate"].ToString(),
                        });
                    }
                }

            }
            return list;



        }
        public void AddRole(RoleAttribute Role)
        {

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_AddRole", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //  RoleAttributes rol = new RoleAttributes();

                cmd.Parameters.AddWithValue("Employee_Id", Role.Employee_Id);
                cmd.Parameters.AddWithValue("Employee_Name", Role.Employee_Name);
                cmd.Parameters.AddWithValue("Role_Designation", Role.Role_Designation);
                cmd.Parameters.AddWithValue("Role_Description", Role.Role_Description);
                cmd.Parameters.AddWithValue("Role_Status", Role.Role_Status);
                cmd.Parameters.AddWithValue("Role_StartDate", Role.Role_StartDate);
                cmd.Parameters.AddWithValue("Role_EndDate", Role.Role_EndDate);


                cmd.ExecuteNonQuery();
                conn.Close();

            }


        }


        public int UpdateRole(RoleAttribute role)
        {

            using (MySqlConnection conn = GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand("sp_UpdateRole", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("emp_Id", role.Employee_Id);
                cmd.Parameters.AddWithValue("Employee_Name", role.Employee_Name);
                cmd.Parameters.AddWithValue("Role_Designation", role.Role_Designation);
                cmd.Parameters.AddWithValue("Role_Description", role.Role_Description);
                cmd.Parameters.AddWithValue("Role_Status", role.Role_Status);
                cmd.Parameters.AddWithValue("Role_StartDate", role.Role_StartDate);
                cmd.Parameters.AddWithValue("Role_EndDate", role.Role_EndDate);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return 1;
        }

        public int DisableRole(string Employee_Id)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_DeleteRole", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("eid", Employee_Id);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return 1;
        }
        public List<EmpDropAttribute> GetEmployeeDropdown()
        {
            List<EmpDropAttribute> elist = new List<EmpDropAttribute>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EmployeeDropdown", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        elist.Add(new EmpDropAttribute()
                        {
                            Emp_Id = reader["Emp_Id"].ToString(),
                            Emp_Name = reader["Emp_Name"].ToString(),
                        });
                    }
                }

            }
            return elist;
        }

    }

}











