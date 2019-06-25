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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AtService.Controllers
{
    public class EntryPointController : Controller
    {
        

            private readonly UserManager<IdentityUser> userManager;
            private readonly IConfiguration configuration;
            private readonly IHostingEnvironment hostingEnvironment;


            public EntryPointController(UserManager<IdentityUser> UserManager, IConfiguration Configuration,IHostingEnvironment HostingEnvironment)
            {
                this.configuration = Configuration;
                this.userManager = UserManager;
            this.hostingEnvironment = HostingEnvironment;
            }

            public IActionResult Index()
            {
                return View();
            }

        //[Route("ExcelFileUpload")]
        //[HttpPost]
        //public async Task<ActionResult> ExcelFile(IList<IFormFile> excelFileName)
        //{
        //    try
        //    {
        //        //string response = string.Empty;
        //        if (excelFileName != null)
        //        {

        //            //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        //            foreach (var file in excelFileName)
        //            {
        //                if (file.Length > 0)
        //                {
        //                    string paths = hostingEnvironment.WebRootPath;

        //                    var filePath = Path.Combine(hostingEnvironment.WebRootPath, file.FileName);
        //                    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                    {

        //                        //FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
        //                        //response = new ExcelExtension().Test(filePath);
        //                        await file.CopyToAsync(fileStream);
        //                    }

        //                    return Ok(new ExcelExtension().Test(filePath));
        //                }
        //            }

        //            //return Ok(excelFileName.FileName);
        //        }
        //    }
        //    catch(Exception exception)
        //    {
        //        return Ok(exception);
        //    }

        //    return Ok("Uploaded");
        //}


        [Route("Login")]
            [HttpPost]
            public async Task<ActionResult> Login([FromBody] LoginContextData loginContextData)
            {
                var user = await userManager.FindByNameAsync(loginContextData.USR_ID_TE);

                if (user != null && await userManager.CheckPasswordAsync(user, loginContextData.USR_PWD_TE))
                {
                    var claim = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserName) };

                    var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Pactera IDC JWT Integration"));

                    int expiryInMinutes = Convert.ToInt32(60);

                    var token = new JwtSecurityToken(
                        issuer: "https://localhost:44330",//configuration["Jwt:Site"],
                        audience: "https://localhost:44330",//configuration["Jwt:Site"],
                        expires: DateTime.Now.AddDays(2),
                        signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });

                }

                return Unauthorized();

            }

            [Route("Register")]
            [HttpPost]
            public async Task<ActionResult> InsertUser([FromBody] UserRegistrationData userRegistrationData)
            {
                var user = new IdentityUser
                {
                    Email = userRegistrationData.Email,
                    UserName = userRegistrationData.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, userRegistrationData.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }

                return Ok(new { Username = user.UserName });
            }
        }
    }
