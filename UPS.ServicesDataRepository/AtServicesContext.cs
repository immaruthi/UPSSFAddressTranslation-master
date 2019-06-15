using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.ServicesDataRepository
{
    public class AtServicesContext
    {
        public static string ConnectionString { get; set; }

        public void AddContextConfiguration(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

    }
}
