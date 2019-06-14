using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class AssignProject
    {
        public string Project_Assign_ID { get; set; }
        public string Emp_Id { get; set; }
        public string Project_ID { get; set; }
        public string Assign_Project_StartDate { get; set; }
        public string Assign_Project_EndDate { get; set; }
        public string Billable { get; set; }
        public string Billing_Percentage { get; set; }
        public string Location { get; set; }
        public string Onsite { get; set; }

        public string Emp_Name { get; set; }
        public string Project_Name { get; set; }



    }
}
