using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UPS.Quincus.APP.Common;
using UPS.Quincus.APP.Request;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.DataContext;
using UPS.ServicesDataRepository.OverrideDbContext;

namespace UPS.AddressTranslationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // ********************
            // Setup CORS
            // ********************
            services.AddCors(c =>
            {
                c.AddPolicy("AllowAtUIOrigin", options => options.WithOrigins(Configuration["CorsEnableDomain:Domain"]));
            });

            DBConnectionContext.getconnection(Configuration);
            MapProxy.webProxyURI = Configuration["webProxy:URL"];
            MapProxy.webProxyUsername = Configuration["webProxy:Username"];
            MapProxy.webProxyPassword = Configuration["webProxy:Password"];
            MapProxy.WebProxyEnable = Configuration["webProxy:Enable"];

            /******* Dependancy Injection section start************/
            services.AddSingleton<IQuincusAddressTranslationRequest>(new QuincusAddressTranslationRequest() { endpoint = Configuration["Quincus:GeoCodeEndPoint"] });
            services.AddTransient<IUserServicesAsync, UserServices>();
            services.AddTransient<IUPSAuthenticationService, UPSAuthenticationService>();
            services.AddTransient<IWorkflowService, WorkflowService>();

            /*************** Dependancy Injection section End*************/
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<ApplicationDbContext>(
                option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var signingKey = Convert.FromBase64String(Configuration["Jwt:SigningKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Site"],
                        ValidIssuer = Configuration["Jwt:Site"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                    };
                });

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
            app.UseCors(
                options => options.WithOrigins(Configuration["CorsEnableDomain:Domain"]).AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

        }

    }
}
