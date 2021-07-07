using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPS.DataObjects.AddressBook;
using UPS.DataObjects.ADR_ADT_LG;
using UPS.DataObjects.CST_DTL;
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
            builder.Entity<User>().ToTable("USR");
            builder.Entity<WorkflowDataRequest>().ToTable("WR-FLW");
            builder.Entity<ShipperCompanyList>().ToTable("SPC-LST");
            builder.Entity<AddressBook>().ToTable("ADR-BK");
            builder.Entity<AddressAuditLogRequest>().ToTable("ADR-ADT-LG");
            builder.Entity<CST_DTL>().ToTable("CST-DTL");
            builder.Entity<UserCityMapping>().ToTable("USR-CTY-MPG");
            builder.Entity<UserRole>().ToTable("USR-IN-RLE");
            builder.Entity<UpsRoles>().ToTable("RLE");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DBConnectionContext.connectionString);
        }
        public DbSet<User> UserData
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

        public DbSet<ShipperCompanyList> shipperCompanyRequests
        {
            get; set;
        }

        public DbSet<AddressBook> AddressBooks
        {
            get; set;
        }

        public DbSet<AddressAuditLogRequest> AddressAuditLogRequests
        {
            get; set;
        }
        public DbSet<CST_DTL> CST_DTL
        {
            get; set;
        }

        public DbSet<UserCityMapping> UserCityMapping
        {
            get; set;
        }

        public new DbSet<UserRole> UserRoles
        {
            get; set;
        }
        public  DbSet<UpsRoles> UpsRoles
        {
            get; set;
        }
    }
}
