using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AtService.Models;
using ExcelFileRead;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;

namespace AtService.Controllers
{
    [EnableCors("AllowAtUIOrigin")]
    public class EntryPointController : Controller
    {


        //private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IUserServicesAsync userService;


        public EntryPointController(UserManager<IdentityUser> UserManager, IConfiguration Configuration, IHostingEnvironment HostingEnvironment, IUserServicesAsync userServicesAsync)
        {
            this.configuration = Configuration;
            //this.userManager = UserManager;
            this.hostingEnvironment = HostingEnvironment;
            this.userService = userServicesAsync;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/Login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            UserDataResponse userResponse = await userService.ValidateUserAsync(loginRequest.USR_ID_TE, loginRequest.USR_PWD_TE);

            if (userResponse.Success)
            {
                var signingKey = Convert.FromBase64String(configuration["Jwt:SigningKey"]);
                var expiryDuration = int.Parse(configuration["Jwt:ExpiryInMinutes"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = configuration["Jwt:Site"],
                    Audience = configuration["Jwt:Site"],
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                    Subject = 
                        new ClaimsIdentity(new List<Claim>
                        {
                            new Claim("userid", userResponse.User.USR_ID_TE.ToString())
                        }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                var token = jwtTokenHandler.WriteToken(jwtToken);
                return Ok(
                    new
                    {
                        token = token,
                        UserId = userResponse.User.USR_ID_TE,
                        UserName = userResponse.User.USR_FST_NA
                    });

            }
            return Unauthorized();

            //IdentityUser user = await userManager.FindByNameAsync(loginRequest.USR_ID_TE);

            //if (user != null && await userManager.CheckPasswordAsync(user, loginRequest.USR_PWD_TE))
            //{
            //        var claim = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserName) };

            //        var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Pactera IDC JWT Integration"));

            //        int expiryInMinutes = Convert.ToInt32(60);

            //        var token = new JwtSecurityToken(
            //            issuer: configuration["Jwt:Site"],
            //            audience: configuration["Jwt:Site"],
            //            expires: DateTime.Now.AddDays(2),
            //            signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            //            );

            //        return Ok(
            //            new
            //            {
            //                token = new JwtSecurityTokenHandler().WriteToken(token),
            //                expiration = token.ValidTo,
            //                UserId = user.Id,
            //                UserName = user.UserName
            //            });

            //    }
        }

        //[Route("Register")]
        //[HttpPost]
        //public async Task<ActionResult> InsertUser([FromBody] UserRegistrationData userRegistrationData)
        //{
        //    var user = new IdentityUser
        //    {
        //        Email = userRegistrationData.Email,
        //        UserName = userRegistrationData.UserName,
        //        SecurityStamp = Guid.NewGuid().ToString()
        //    };

        //    var result = await userManager.CreateAsync(user, userRegistrationData.Password);

        //    if (result.Succeeded)
        //    {
        //        await userManager.AddToRoleAsync(user, "Customer");
        //    }

        //    return Ok(new { Username = user.UserName });
        //}
    }
}
