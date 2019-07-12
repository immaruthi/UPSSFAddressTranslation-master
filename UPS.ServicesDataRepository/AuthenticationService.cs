using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class UPSAuthenticationService : IUPSAuthenticationService
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        public UPSAuthenticationService(ApplicationDbContext applicationDbContext,IConfiguration _configuration)
        {
            context = applicationDbContext;
            this.configuration = _configuration;
        }
        public async Task<UserDataResponse> AuthenticateUserAsync(string USR_ID_TE, string USR_PWD_TE)
        {
            UserDataResponse userDataResponse = new UserDataResponse();
            try
            {
                USR user =
                    await context.UserData
                    .FirstOrDefaultAsync(
                        (USR usr) =>
                            usr.USR_ID_TE.ToLower() == USR_ID_TE.ToLower() 
                            && usr.USR_PWD_TE == USR_PWD_TE);
                   
                userDataResponse.Success = false;
                if (user != null)
                {
                    userDataResponse.Success = true;
                    userDataResponse.Message = "Valid User";
                }
                else
                {
                   userDataResponse.Message = "User not found, Please check your credentials";
                }
                userDataResponse.User = user;
                
            }
            catch (Exception ex)
            {
                userDataResponse.Success = false;
                userDataResponse.OperationException = ex;
                userDataResponse.Message = "Error while getting user";
            }

            return userDataResponse;
        }

        public string GenerateValidationToken(string userIdText, int? Id)
        {
            var signingKey = Convert.FromBase64String(configuration[JwtConstant.SigningKey]);
            var expiryDuration = int.Parse(configuration[JwtConstant.ExpiryInMinutes]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration[JwtConstant.Site],
                Audience = configuration[JwtConstant.Site],
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject =
                    new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(JwtConstant.UserIdText, userIdText),
                        new Claim(JwtConstant.UserId,Convert.ToString(Id))
                    }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            string token = jwtTokenHandler.WriteToken(jwtToken);

            return token;
        }
    }
}
