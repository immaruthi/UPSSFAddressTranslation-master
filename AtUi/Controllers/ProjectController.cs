using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RMG.Models;

namespace Projects.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        //ProjectContext project_obj = new ProjectContext();                       //class name of context file


        [HttpGet("[action]")]
        public IEnumerable<ProjectAttribute> GetAllProjects()
        {
            ProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.ProjectContext)) as ProjectContext;
            return context.GetAllProjects();
        }

        [HttpPost("[action]")]
        [Route("api/Projects/AddProjects")]
        public int AddProjects([FromBody] List<ProjectAttribute> project)
        {
            ProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.ProjectContext)) as ProjectContext;
            foreach (RMG.Models.ProjectAttribute p in project)
            {
                context.AddProjects(p);
            }
            return 1;
        }

        [HttpPut("[action]")]
        // [Route("api/customer/Edit")]
        public int UpdateProject([FromBody] ProjectAttribute project)
        {
            ProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.ProjectContext)) as ProjectContext;
            context.UpdateProject(project);
            return 1;
        }

        [HttpGet("[action]")]
        public void DisableProject(string Project_ID)
        {
            ProjectContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.ProjectContext)) as ProjectContext;
            context.DisableProject(Project_ID);
        }
    }
}