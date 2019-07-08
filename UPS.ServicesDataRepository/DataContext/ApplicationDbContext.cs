using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPS.DataObjects.AddressBook;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.SPC_LST;
using UPS.DataObjects.UserData;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesDataRepository.OverrideDbContext;

namespace UPS.ServicesDataRepository.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

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
            builder.Entity<USR>().ToTable("USR");
            builder.Entity<WorkflowDataRequest>().ToTable("WR-FLW");
            builder.Entity<ShipperCompanyRequest>().ToTable("SPC-LST");
            builder.Entity<AddressBook>().ToTable("ADR-BK");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DBConnectionContext.connectionString);
        }
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

        public DbSet<ShipperCompanyRequest> shipperCompanyRequests
        {
            get; set;
        }

        public DbSet<AddressBook> AddressBooks
        {
            get; set;
        }
    }
}
