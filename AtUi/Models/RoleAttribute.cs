
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class RoleAttribute
    {
        private RoleContext context;

        public string Employee_Id { get; set; }

        public string Employee_Name { get; set; }

        public string Role_Designation { get; set; }

        public string Role_Description { get; set; }

        public string Role_Status { get; set; }

        public string Role_StartDate { get; set; }

        public string Role_EndDate { get; set; }

        public int flag { get; set; }

        //   public string Role_CreatedBy { get; set; }

        //  public string Role_CreatedDate { get; set; }

        //   public string Role_LastUpdatedBy { get; set; }

        //  public string Role_LastUpdatedDate { get; set; }

    }
}
