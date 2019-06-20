﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.AddressTranslationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {


            new GetConnectionString().getconnection(Configuration);

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            //services.AddDbContext<UPSDataContext>(
            //    option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //new AtServicesContext().AddContextConfiguration(Configuration);

            services.AddDbContext<ApplicationDbContext>(
                option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



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

            //services.AddIdentityServerAuthentication();


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }

    }
}
