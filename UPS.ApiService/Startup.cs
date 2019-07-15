using AtService.Extensions;
using ElmahCore.Mvc;
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
using System;
using System.Text;
using UPS.Quincus.APP.Request;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

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

            services.ContextSetup(Configuration);

            services.AddSingleton<IQuincusAddressTranslationRequest>(new QuincusAddressTranslationRequest() { endpoint = Configuration["Quincus:GeoCodeEndPoint"] });
            /* Dependancy Injection */

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUPSAuthenticationService, UPSAuthenticationService>();


            services.AddTransient<IAddressAuditLogAsync, AddressAuditLogService>();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddTransient<IShipmentAsync, ShipmentService>();
            services.AddTransient<IAddressBookService, AddressBookService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEntityValidationService, EntityValidationServic>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IShipperCompanyAsync, ShipperCompanyService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Core API", Description = "Swagger Core API" });
                c.OperationFilter<FormFileSwaggerFilter>();
            }

             );

            services.AddElmah();
            
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

            services.IocSetup();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(
             options => options.WithOrigins(Configuration["CorsEnableDomain:Domain"]).AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseElmah();
         
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");

            }
            );

            app.AddLogFile(env, Configuration);
        }
    }
}
