using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class RoleService : IRoleService
    {
        private ApplicationDbContext context;

        public RoleService(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }
        public string AddRole(UpsRoles upsRole)
        {
            string response = string.Empty;
            try
            {
                List<UpsRoles> upsRoles = 
                    new List<UpsRoles>()
                    {
                        upsRole
                    };

                this.context.BulkInsert(upsRoles);

                response= string.Format(ResponseConstant.Create_Success, "Role");
            }
            catch (Exception exception)
            {
                response= string.Format(ResponseConstant.Create_Error,"Role",exception?.Message);
            }

            response = string.Format(ResponseConstant.Something_Went_Wrong);

            return response;
        }

        public JsonResult DeleteRole(int id)
        {
            JsonResult response = new JsonResult(string.Empty);
            UpsRoles upsRoles =
                 this.context.UpsRoles
                 .FirstOrDefault(
                     (UpsRoles roles) => roles.Id == id);
            if (upsRoles == null)
            {
                response.Value = string.Format(ResponseConstant.Not_Exists, "Role");
                response.StatusCode =(int) HttpStatusCode.NotFound;
               
            }
            else
            {
                try
                {
                    this.context.Remove(upsRoles);
                    this.context.SaveChanges();

                    response.Value = ResponseConstant.Delete_Success;
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                catch (Exception exception)
                {
                    response.Value = string.Format(ResponseConstant.Delete_Error,exception.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }

            return response;
        }

        public async Task<List<UpsRoles>> GetAllRoles()
        {
            List<UpsRoles> upsRoles = 
                await this.context.UpsRoles.ToListAsync();

            return upsRoles;
        }

        public string UpdateRole(UpsRoles upsRoles)
        {
            UpsRoles upsRole =
                this.context.UpsRoles
                .FirstOrDefault(
                    (UpsRoles role) => role.Id == upsRoles.Id);
            upsRole.UpdatedDate = DateTime.Now;
            upsRole.Name = upsRoles.Name;
            try
            {
                this.context.UpsRoles.Update(upsRole);
                this.context.SaveChanges();
                return string.Format(ResponseConstant.Update_Success, "Role");
            }
            catch (Exception exception)
            {
                return string.Format(ResponseConstant.Update_Error, exception.Message); 
            }
        }
    }
}
