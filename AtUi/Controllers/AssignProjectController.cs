using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RMG.Models;


namespace RMG.Controllers
{
    [Route("api/[controller]")]
    public class AssignProjectController : Controller
    { 
    

        [HttpGet("[action]")]
        public IEnumerable<RMG.Models.AssignProject> GetAllAssignProject()
        {
            AssignProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.AssignProjectContext)) as AssignProjectContext;

            return context.GetAllAssignProject();
        }


        [HttpGet]
        [Route("api/AssignProject/Details/{id}")]
        public AssignProject Details(string project_Assign_ID)


        {

            AssignProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.AssignProjectContext)) as AssignProjectContext;

            return context.GetAssignProjectData(project_Assign_ID);

        }

        [HttpPost("[action]")]
       
        public int AddAssignProject([FromBody]List<RMG.Models.AssignProject> assignProject)
        {
            AssignProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.AssignProjectContext)) as AssignProjectContext;
            foreach (RMG.Models.AssignProject ap in assignProject)
            {
                context.AddAssignProject(ap);
            }
            return 1;

        }

        [HttpPut("[action]")]
        // [Route("api/assignProject/Edit")]
        public int UpdateAssignProject([FromBody]AssignProject assignProject)
        {
            AssignProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.AssignProjectContext)) as AssignProjectContext;
            context.UpdateAssignProject(assignProject);
            return 1;
        }


        [HttpGet("[action]")]
        // [Route("api/assignProject/Delete")]
        public int DeleteAssignProject(string project_Assign_ID)
        {
            AssignProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.AssignProjectContext)) as AssignProjectContext;
            context.DeleteAssignProject(project_Assign_ID);
            return 1;
        }
    }



}

