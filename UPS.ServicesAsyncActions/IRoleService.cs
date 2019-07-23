using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;

namespace UPS.ServicesAsyncActions
{
    public interface IRoleService
    {
        Task<List<UpsRoles>> GetAllRoles();
        JsonResult DeleteRole(int id);
        string AddRole(UpsRoles upsRoles);
        string UpdateRole(UpsRoles upsRoles);



    }
}
