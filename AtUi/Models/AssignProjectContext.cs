using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace RMG.Models
{
    public class AssignProjectContext
    //public class CustomerContext
    {
        public string ConnectionString { get; set; }

        public AssignProjectContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<AssignProject> GetAllAssignProject()      //doubt
        {
            List<AssignProject> list = new List<AssignProject>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_fetchAssignprojects", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //  MySqlCommand cmd = new MySqlCommand("select Project_Assign_ID,Emp_Id, Project_ID,Emp_Name,Project_Name,Assign_Project_StartDate,Assign_Project_EndDate,Billable,Billing_Percentage,Location,Onsite from  pact_rmg_emp_proj where flag=1", conn);


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new AssignProject()
                        {
                            Emp_Id = reader["Emp_Id"].ToString(),
                            Emp_Name=reader["Emp_Name"].ToString(),
                            Project_Name = reader["Project_Name"].ToString(),
                            Assign_Project_StartDate = reader["Assign_Project_StartDate"].ToString(),
                            Assign_Project_EndDate = reader["Assign_Project_EndDate"].ToString(),
                            Billable = reader["Billable"].ToString(),
                            Billing_Percentage = reader["Billing_Percentage"].ToString(),
                            Location = reader["Location"].ToString(),
                            Onsite = reader["Onsite"].ToString(),
                        });
                    }
                }
            }
            return list;
        }

        public void AddAssignProject(AssignProject assignProject)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_AddAssignProject", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Project_Assign_ID", assignProject.Project_Assign_ID);
                cmd.Parameters.AddWithValue("Emp_Id", assignProject.Emp_Id);
                cmd.Parameters.AddWithValue("Project_ID", assignProject.Project_ID);
                cmd.Parameters.AddWithValue("Assign_Project_StartDate", assignProject.Assign_Project_StartDate);
                cmd.Parameters.AddWithValue("Assign_Project_EndDate", assignProject.Assign_Project_EndDate);
                cmd.Parameters.AddWithValue("Billable", assignProject.Billable);
                cmd.Parameters.AddWithValue("Billing_Percentage", assignProject.Billing_Percentage);
                cmd.Parameters.AddWithValue("Location", assignProject.Location);
                cmd.Parameters.AddWithValue("Onsite", assignProject.Onsite);



                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }

        public AssignProject GetAssignProjectData(string Project_Assign_ID)     //doubt

        {
            AssignProject assignProject = new AssignProject();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select Project_Assign_ID,Emp_Id, Project_ID,Assign_Project_StartDate,Assign_Project_EndDate,Billable,Billing_Percentage,Location,Onsite from pact_rmg_customer WHERE Project_Assign_ID=" + Project_Assign_ID, conn);



                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {

                    while (rdr.Read())

                    {
                        assignProject.Project_Assign_ID = rdr["Project_Assign_ID"].ToString();

                        assignProject.Emp_Id = rdr["Emp_Id"].ToString();

                        assignProject.Project_ID = rdr["Project_ID"].ToString();

                        assignProject.Assign_Project_StartDate = rdr["Assign_Project_StartDate"].ToString();

                        assignProject.Assign_Project_EndDate = rdr["Assign_Project_EndDate"].ToString();

                        assignProject.Billable = rdr["Billable"].ToString();

                        assignProject.Billing_Percentage = rdr["Billing_Percentage"].ToString();


                        assignProject.Location = rdr["Location"].ToString();

                        assignProject.Onsite = rdr["Onsite"].ToString();


                    }

                }

                return assignProject;
            }

        }
        public void DeleteAssignProject(string Project_Assign_ID)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_DeleteAssignProject", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pProject_Assign_ID", Project_Assign_ID);


                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }
        public void UpdateAssignProject(AssignProject assignProject)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_UpdateAssignProject", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pProject_Assign_ID", assignProject.Project_Assign_ID);
                cmd.Parameters.AddWithValue("Emp_Id", assignProject.Emp_Id);
                cmd.Parameters.AddWithValue("Project_ID", assignProject.Project_ID);
                cmd.Parameters.AddWithValue("Assign_Project_StartDate", assignProject.Assign_Project_StartDate);
                cmd.Parameters.AddWithValue("Assign_Project_EndDate", assignProject.Assign_Project_EndDate);
                cmd.Parameters.AddWithValue("Billable", assignProject.Billable);
                cmd.Parameters.AddWithValue("Billing_Percentage", assignProject.Billing_Percentage);
                cmd.Parameters.AddWithValue("Location", assignProject.Location);
                cmd.Parameters.AddWithValue("Onsite", assignProject.Onsite);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }
    }





}
