using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class UserService : IUserService
    {
        private ApplicationDbContext context;
        private IEntityValidationService entityValidationService;

        public UserService(ApplicationDbContext context,
            IEntityValidationService entityValidationService)
        {
            this.context = context;
            this.entityValidationService = entityValidationService;
        }
        public string CreateUser(User user)
        {
            string responseMessage = string.Empty;
            bool isUserExist = CheckExistingUser(user);
            if (!isUserExist)
            {
                if (this.entityValidationService.IsValidEntity(user))
                {
                    try
                    {
                        user.IsActive = true;
                        var userInfo = this.context.Add(user).Entity;
                        this.context.SaveChanges();

                        if (
                            userInfo != null
                            && userInfo.ID > 0)

                        {
                           responseMessage = AddRoleForUser(user, userInfo);

                            if (
                                user.Cities != null
                                && user.Cities.Any())
                            {
                                responseMessage = AddUserCities(user, userInfo);
                            }
                          
                        }
                    }
                    catch (Exception exception)
                    {
                        responseMessage = string.Format(ResponseConstant.Create_Error, "User");
                        // Need to log exception
                    }
                }
                else
                {
                    responseMessage = ResponseConstant.Validation_Error;
                }
            }
            else
            {
                responseMessage = ResponseConstant.User_Exist;
            }
            

            return responseMessage;
        }

        private string AddRoleForUser(User user, User userInfo)
        {
            try
            {
                List<UserRole> userRole =
                new List<UserRole>{
                    new UserRole()
                    {
                        IsAcive = true,
                        RoleId = user.Role,
                        UserId = userInfo.ID,
                    }
                };

                this.context.BulkInsert(userRole);

                return ResponseConstant.User_Create_Success;
            }
            catch (Exception exception)
            {
                return string.Format(ResponseConstant.Create_Error, "Roles");
            }
           
        }

        private string AddUserCities(User user, User userInfo)
        {
            try
            {
                List<UserCityMapping> userCityMappings = new List<UserCityMapping>();
                user.Cities.ForEach(city =>
                {
                    UserCityMapping userCityMapping =
                    new UserCityMapping()
                    {
                        UserId = userInfo.ID ?? -1,
                        City = city,
                        CreatedDate = DateTime.Now,
                        State = true
                    };
                    userCityMappings.Add(userCityMapping);
                });

                this.context.BulkInsert(userCityMappings);
                return ResponseConstant.User_Create_Success;
            }
            catch (Exception exception)
            {
                return string.Format(ResponseConstant.Create_Error, "City");
            }
           
        }

        private bool CheckExistingUser(User user)
        {
            return this.context.UserData.Any(
                (User u) =>
                 u.UserId.ToLower().Equals(user.UserId.ToLower())
                );
        }

        public int DeleteUser(User userData)
        {
            throw new NotImplementedException();
        }

        public UserDataResponse GetUserData()
        {
            throw new NotImplementedException();
        }

        public UserDataResponse UpdateUser(User userData)
        {
            string connectionString = AtServicesContext.ConnectionString;

            throw new NotImplementedException();
        }
    }
}
