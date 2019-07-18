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
                                responseMessage = AddUserCities(user, userInfo.ID);
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

        private string AddUserCities(User user, int userId)
        {
            try
            {
                List<UserCityMapping> userCityMappings = new List<UserCityMapping>();
                user.Cities.ForEach(city =>
                {
                    UserCityMapping userCityMapping =
                    new UserCityMapping()
                    {
                        UserId = userId,
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

        public async Task<List<User>> GetAllUser()
        {
            List<UserCityMapping> userCityMappings = this.context.UserCityMapping.ToList();

            List<User> users =
                await
                    (from user in this.context.UserData
                     from roles in this.context.UserRoles
                         .Where(role => role.UserId == user.ID).DefaultIfEmpty()
                     select new User()
                     {
                         ID = user.ID,
                         FirstName = user.FirstName,
                         LastName = user.LastName,
                         Email = user.Email,
                         UserId = user.UserId,
                         CreatedBy = user.CreatedBy,
                         CreatedDate = user.CreatedDate,
                         UpdatedBy = user.UpdatedBy,
                         UpdatedDate = user.UpdatedDate,
                         IsActive = user.IsActive,
                         Role = roles == null ? 0 : roles.RoleId,
                         Cities = userCityMappings
                                  .Where(_ => _.UserId == user.ID)
                                  .Select(_ => _.City).ToList()

                     }).ToListAsync();

            return users;
        }
        private void UpdateUserCities(User user)
        {
            try
            {
                List<UserCityMapping> existingCityMapping =
                    this.context.UserCityMapping
                    .Where(_ => _.UserId == user.ID) 
                    .ToList();

                this.context.BulkDelete(existingCityMapping);

                this.AddUserCities(user, user.ID);

            }
            catch (Exception exception)
            {
              
            }

        }
        public async Task<int> UpdateUser(User user, int loggedUserId)
        {
            User existingUser =
              await this.context.UserData
                        .FirstOrDefaultAsync(
                            (User usr) =>
                            usr.ID == user.ID);

            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.UpdatedBy = loggedUserId;
                existingUser.UpdatedDate = DateTime.Now;

                this.context.BulkUpdateAsync(new List<User>() { existingUser });

                UserRole userRole =
                    this.context.UserRoles
                     .FirstOrDefault(
                        (UserRole role) => role.UserId == user.ID);

                if (!(userRole != null && userRole.RoleId == user.Role))
                {
                    userRole.RoleId = user.Role;
                    this.context.BulkUpdateAsync(new List<UserRole>() { userRole });

                }

                this.UpdateUserCities(user);
            }

            return existingUser.ID;
        }

        public UserDataResponse GetUserData()
        {
            throw new NotImplementedException();
        }
    }
}
