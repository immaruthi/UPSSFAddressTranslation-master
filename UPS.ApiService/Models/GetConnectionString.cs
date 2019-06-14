using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtService.Models
{
    public class GetConnectionString
    {
        public static string connectionString { get; set; }

        public string getconnection(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }


    }
}
