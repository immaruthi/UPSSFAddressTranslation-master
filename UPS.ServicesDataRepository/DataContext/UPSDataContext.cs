using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;

namespace UPS.ServicesDataRepository.DataContext
{
    public class UPSDataContext : DbContext
    {
        public UPSDataContext() : base()
        {

        }

        public UPSDataContext(DbContextOptions<UPSDataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShipmentDataRequest>().HasKey(x => x.SHP_NR);
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
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

    }
}
