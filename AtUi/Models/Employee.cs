using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class Employee
    {
        //private EmployeeContext context;
        public string Emp_Id { get; set; }
        public string Emp_Name { get; set; }
        public string Designation_Id { get; set; }
        public string Department_Id { get; set; }
        public string Edge_Practice_Id { get; set; }
        public string Coe_Id { get; set; }
        public string Location_Code { get; set; }
        public string Joining_Date{ get; set; }
        public string Contact_Number { get; set; }
        public string Email_ID { get; set; }
        public string Reporting_To { get; set; }
        public string Reporting_To_Email { get; set; }
        public int flag { get; set; }

    }
}
