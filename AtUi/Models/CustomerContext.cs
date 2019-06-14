using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace RMG.Models
{
    public class CustomerContext
    {
            public string ConnectionString { get; set; }

            public CustomerContext(string connectionString)
            {
                this.ConnectionString = connectionString;
            }

            private MySqlConnection GetConnection()
            {
                return new MySqlConnection(ConnectionString);
            }

            public List<Customer> GetAllCustomer()
            {
                List<Customer> list = new List<Customer>();

                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_AllCustomers", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Customer()
                            {
                                cust_code = reader["cust_code"].ToString(),
                                cust_name = reader["cust_name"].ToString(),
                                location_id = reader["location_id"].ToString(),
                                Project_Id = reader["Project_Id"].ToString(),
                                status = reader["status"].ToString(),
                            });
                        }
                    }
                }
                return list;
            }

            public void AddCustomer(Customer customer)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_AddCustomer", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("cust_code", customer.cust_code);
                    cmd.Parameters.AddWithValue("cust_name", customer.cust_name);
                    cmd.Parameters.AddWithValue("location_id", customer.location_id);
                    cmd.Parameters.AddWithValue("Project_Id", customer.Project_Id);
                    cmd.Parameters.AddWithValue("status", customer.status);


                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }

            public void DeleteCustomer(string cust_id)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_DeleteCustomer", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("pcust_id", cust_id);


                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
            public void UpdateCustomer(Customer customer)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_UpdateCustomer", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pcust_code", customer.cust_code);
                    cmd.Parameters.AddWithValue("cust_name", customer.cust_name);
                    cmd.Parameters.AddWithValue("location_id", customer.location_id);
                    cmd.Parameters.AddWithValue("Project_Id", customer.Project_Id);
                    cmd.Parameters.AddWithValue("status", customer.status);


                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
    }
}
