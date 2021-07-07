using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.SPC_LST;
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

        public string CreateUser(User user, int loggedUserId)
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
                        user.CreatedDate = DateTime.Now;
                        user.CreatedBy = loggedUserId;
                        var userInfo = this.context.Add(user).Entity;
                        this.context.SaveChanges();

                        if (
                            userInfo != null
                            && userInfo.ID > 0)

                        {
                            responseMessage = AddRoleForUser(user, userInfo.ID);

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

        private string AddRoleForUser(User user, int userId)
        {
            try
            {
                List<UserRole> userRole =
                new List<UserRole>{
                    new UserRole()
                    {
                        IsAcive = true,
                        RoleId = user.Role,
                        UserId =userId,
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
                return string.Format(ResponseConstant.Create_Error, "City", exception.Message);
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
            List<string> availableCities = await this.GetAvailableCities();
            List<User> users =
                await
                    (from user in this.context.UserData
                     from roles in this.context.UserRoles
                         .Where(role => role.UserId == user.ID).DefaultIfEmpty()
                    orderby user.ID descending
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
                                  .Where(
                                    (UserCityMapping cityMap) => 
                                        cityMap.UserId == user.ID
                                        && availableCities.Contains(cityMap.City.ToLower().Trim()))
                                  .Select(_ => _.City).ToList(),
                         Country = user.Country

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

        public async Task<string> UpdateUser(User user, int loggedUserId)
        {
            User existingUser =
              await this.context.UserData
                        .FirstOrDefaultAsync(
                            (User usr) =>
                            usr.ID == user.ID);

            if (existingUser != null)
            {
                try
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.UpdatedBy = loggedUserId;
                    existingUser.UpdatedDate = DateTime.Now;
                    existingUser.Country = user.Country;

                    await this.context.BulkUpdateAsync(new List<User>() { existingUser });

                    UserRole userRole =
                        this.context.UserRoles
                         .FirstOrDefault(
                            (UserRole role) => role.UserId == user.ID);

                    if (userRole == null)
                    {
                        this.AddRoleForUser(user, user.ID);
                    }
                    else if (userRole.RoleId != user.Role)
                    {
                        userRole.RoleId = user.Role;
                        await this.context.BulkUpdateAsync(new List<UserRole>() { userRole });

                    }

                    this.UpdateUserCities(user);

                    return string.Format(ResponseConstant.Update_Success, "User");
                }
                catch (Exception exception)
                {
                    return string.Format(ResponseConstant.Update_Error, "User", exception.Message);
                }
            }

            else
            {
                return string.Format(ResponseConstant.Not_Exists, "User");
            }

        }

        public UserDataResponse GetUserData()
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAvailableCities()
        {
            List<string> cities =
                await this.context.shipperCompanyRequests
                        .Select(
                            (ShipperCompanyList shipperList) =>
                                shipperList.SPC_CTY_TE.ToLower().Trim())
                        .Distinct()
                        .OrderBy(city => city)
                        .ToListAsync();

            return cities;
        }
    }
}
