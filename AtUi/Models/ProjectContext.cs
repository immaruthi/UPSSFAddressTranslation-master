using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class ProjectContext
    {
        public string ConnectionStrings { get; set; }

        public ProjectContext(string connectionString)
        {
            this.ConnectionStrings = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionStrings);
        }
        public List<ProjectAttribute> GetAllProjects()
        {
            List<ProjectAttribute> list = new List<ProjectAttribute>();

            using (MySqlConnection conn = GetConnection())
            {

                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sp_getallprojects ", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ProjectAttribute()
                        {
                            Project_ID = reader["Project_ID"].ToString(),
                            Project_Name = reader["Project_Name"].ToString(),
                            Project_Description = reader["Project_Description"].ToString(),
                            Project_StartDate = reader["Project_StartDate"].ToString(),
                            Project_EndDate = reader["Project_EndDate"].ToString(),
                            Project_Status= reader["Project_Status"].ToString(),

                        });

                    }
                }
            }
            return list;


        }

     
        //to add projects
        public int AddProjects(ProjectAttribute project)
        {

            using (MySqlConnection con = new MySqlConnection(ConnectionStrings))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_AddProjects", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Project_ID", project.Project_ID);               //procedure parameters
                cmd.Parameters.AddWithValue("Project_Name", project.Project_Name);               //procedure parameters
                cmd.Parameters.AddWithValue("Project_Description", project.Project_Description);
                cmd.Parameters.AddWithValue("Project_StartDate", project.Project_StartDate);
                cmd.Parameters.AddWithValue("Project_EndDate", project.Project_EndDate);
                cmd.Parameters.AddWithValue("Project_Status", project.Project_Status);

                cmd.ExecuteNonQuery();
                // con.Close();
            }
            return 1;
        }
        public int UpdateProject(ProjectAttribute project)
        {

            using (MySqlConnection con = new MySqlConnection(ConnectionStrings))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_UpdateProject", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pProject_ID", project.Project_ID);               //procedure parameters
                cmd.Parameters.AddWithValue("Project_Name", project.Project_Name);               //procedure parameters
                cmd.Parameters.AddWithValue("Project_Description", project.Project_Description);
                cmd.Parameters.AddWithValue("Project_StartDate", project.Project_StartDate);
                cmd.Parameters.AddWithValue("Project_EndDate", project.Project_EndDate);
                cmd.Parameters.AddWithValue("Project_Status", project.Project_Status);

                cmd.ExecuteNonQuery();
                con.Close();
            }
            return 1;
        }
        public int DisableProject(string Project_ID)
        {

            using (MySqlConnection con = new MySqlConnection(ConnectionStrings))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("sp_DeleteProject", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("pid", Project_ID);

                cmd.ExecuteNonQuery();
                con.Close();
            }
            return 1;
        }

    }
}
    
    

