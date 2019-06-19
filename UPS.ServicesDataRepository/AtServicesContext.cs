using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class AtServicesContext: ApplicationDbContext
    {
        private static DbContextOptions<ApplicationDbContext> applicationDbContext = null;

        public AtServicesContext():base(applicationDbContext)
        {

        }

        public static string ConnectionString { get; set; }

        public void AddContextConfiguration(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

    }
}
