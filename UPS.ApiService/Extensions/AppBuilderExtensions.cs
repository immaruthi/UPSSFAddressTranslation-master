using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.Application.CustomLogs;
using UPS.Quincus.APP.Common;
using UPS.ServicesDataRepository.DataContext;
using UPS.ServicesDataRepository.OverrideDbContext;

namespace AtService.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void AddLogFile(this IApplicationBuilder app, IHostingEnvironment env, IConfiguration Configuration)
        {
            AuditEventEntry.Configuration = Configuration;
            AuditEventEntry.HostingEnvironment = env;
        }

        public static void ConfigureJWTAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(
                option =>
                {
                    option.Password.RequireDigit = false;
                    option.Password.RequiredLength = 6;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.Password.RequireLowercase = false;
                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            services.AddAuthentication(new AuthenticationOptions().DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme);
            services.AddAuthentication(new AuthenticationOptions().DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme);
            services.AddAuthentication(new AuthenticationOptions().DefaultScheme = JwtBearerDefaults.AuthenticationScheme);

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Site"],
                    ValidIssuer = Configuration["Jwt:Site"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Pactera IDC JWT Integration"))
                };
            });
        }

        public static void ContextSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            DBConnectionContext.getconnection(Configuration);
            MapProxy.webProxyURI = Configuration["webProxy:URL"];
            MapProxy.webProxyUsername = Configuration["webProxy:Username"];
            MapProxy.webProxyPassword = Configuration["webProxy:Password"];
            MapProxy.WebProxyEnable = Configuration["webProxy:Enable"];
        }
    }
        
}
