using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class ProjectAttribute
    {
        private ProjectContext context;

        public string Project_ID { get; set; }

        public string Project_Name { get; set; }

        public string Project_Description { get; set; }

        public string Project_StartDate { get; set; }

        public string Project_EndDate { get; set; }

        public string Project_Status { get; set; }

        public string flag { get; set; }

    }
}
