using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace RMG.Models
{
    public class EmployeeContext
    {
        public string ConnectionString { get; set; }

        public EmployeeContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public List<Employee> GetAllEmployee(string Emp_ID)
        {
            List<Employee> list = new List<Employee>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "select Emp_Id,Emp_Name,Designation_Id,Department_Id,Edge_Practice_Id,Coe_Id,Location_Code,Joining_Date,Contact_Number,Email_ID,Reporting_To,Reporting_To_Email  from Pact_RMG_Employee_MST where flag = 1 and emp_id like '%" + Emp_ID + "%' order by Emp_Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Employee()
                        {
                            Emp_Id = reader["Emp_Id"].ToString(),
                            Emp_Name = reader["Emp_Name"].ToString(),
                            Designation_Id = reader["Designation_Id"].ToString(),
                            Department_Id = reader["Department_Id"].ToString(),
                            Edge_Practice_Id = reader["Edge_Practice_Id"].ToString(),
                            Coe_Id = reader["Coe_Id"].ToString(),
                            Location_Code = reader["Location_Code"].ToString(),
                            Joining_Date = reader["Joining_Date"].ToString(),
                            Contact_Number = reader["Contact_Number"].ToString(),
                            Email_ID = reader["Email_ID"].ToString(),
                            Reporting_To = reader["Reporting_To"].ToString(),
                            Reporting_To_Email = reader["Reporting_To_Email"].ToString(),
                        });
                    }
                }
            }
            return list;
        }
        public void AddEmployee(Employee employee)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_AddEmployee", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Emp_Id", employee.Emp_Id);
                cmd.Parameters.AddWithValue("Emp_Name", employee.Emp_Name);
                cmd.Parameters.AddWithValue("Designation_Id", employee.Designation_Id);
                cmd.Parameters.AddWithValue("Edge_Practice_Id", employee.Edge_Practice_Id);
                cmd.Parameters.AddWithValue("Department_Id", employee.Department_Id);
                cmd.Parameters.AddWithValue("Coe_Id", employee.Coe_Id);
                cmd.Parameters.AddWithValue("Location_Code", employee.Location_Code);
                cmd.Parameters.AddWithValue("Joining_Date", Convert.ToDateTime(employee.Joining_Date).Date);
                cmd.Parameters.AddWithValue("Contact_Number", employee.Contact_Number);
                cmd.Parameters.AddWithValue("Email_ID", employee.Email_ID);
                cmd.Parameters.AddWithValue("Reporting_To", employee.Reporting_To);
                cmd.Parameters.AddWithValue("Reporting_To_Email", employee.Reporting_To_Email);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }
        public int UpdateEmployee(Employee employee)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_UpdateEmployee", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pEmp_Id", employee.Emp_Id);
                    cmd.Parameters.AddWithValue("Emp_Name", employee.Emp_Name);
                    cmd.Parameters.AddWithValue("Designation_Id", employee.Designation_Id);
                    cmd.Parameters.AddWithValue("Edge_Practice_Id", employee.Edge_Practice_Id);
                    cmd.Parameters.AddWithValue("Department_Id", employee.Department_Id);
                    cmd.Parameters.AddWithValue("Coe_Id", employee.Coe_Id);
                    cmd.Parameters.AddWithValue("Location_Code", employee.Location_Code);
                    cmd.Parameters.AddWithValue("Joining_Date", employee.Joining_Date);
                    cmd.Parameters.AddWithValue("Contact_Number", employee.Contact_Number);
                    cmd.Parameters.AddWithValue("Email_ID", employee.Email_ID);
                    cmd.Parameters.AddWithValue("Reporting_To", employee.Reporting_To);
                    cmd.Parameters.AddWithValue("Reporting_To_Email", employee.Reporting_To_Email);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }
        public int DeleteEmployee(string Emp_Id)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_DeleteEmployee", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("eid", Emp_Id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return 1;


        }

        public List<Designation> GetAllDesignation()
        {
            List<Designation> list = new List<Designation>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EmployeeDesignation", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Designation()
                        {
                            Designation_Description = reader["Design_Description"].ToString(),

                        });
                    }
                }
            }
            return list;
        }

        public List<Department> GetAllDepartment()
        {
            List< Department> list = new List<Department>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EmployeeDepartment", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Department()
                        {
                            Department_Description = reader["Department_Description"].ToString(),

                        });
                    }
                }
            }
            return list;
        }
        public List<EdgePractice> GetAllEdgePractice()
        {
            List<EdgePractice> list = new List<EdgePractice>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_EmployeeEdgePractice", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new EdgePractice()
                        {
                            Edge_Practice_Description= reader["Edge_Practice_Description"].ToString(),

                        });
                    }
                }
            }
            return list;
        }

        public List<CoeDescription> GetAllCoeDescription()
        {
            List<CoeDescription> list = new List<CoeDescription>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_Employee_Coe_Description", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CoeDescription()
                        {
                          Coe_Description  = reader["Coe_Description"].ToString(),

                        });
                    }
                }
            }
            return list;
        }

        public List<BaseLocation> GetAllCity()
        {
            List<BaseLocation> list = new List<BaseLocation>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_Employee_BaseLocation", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new BaseLocation()
                        {
                            city = reader["city"].ToString(),

                        });
                    }
                }
            }
            return list;
        }
    }
}




