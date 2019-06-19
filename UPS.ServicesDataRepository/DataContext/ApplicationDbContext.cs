using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;
using UPS.DataObjects.WR_FLW;

namespace UPS.ServicesDataRepository.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        const string connectionString = "Server=tcp:upssf.database.windows.net,1433;Initial Catalog=AT;Persist Security Info=False;User ID=suresh;Password=123456Aa#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = "2", Name = "Customer", NormalizedName = "Customer" }
                );
            builder.Entity<ShipmentDataRequest>().ToTable("SMT-DTL-FRM-XL");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //        var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}

        public DbSet<USR> UserData
        {
            get;
            set;
        }
        public DbSet<ShipmentDataRequest> shipmentDataRequests
        {
            get; set;
        }
        public DbSet<WorkflowDataRequest> workflowDataRequests
        {
            get; set;
        }
    }
}
