using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesDataRepository.DataContext;
using UPS.ServicesDataRepository.OverrideDbContext;
using System.Linq;
using UPS.ServicesDataRepository;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class ExcelWorkflowController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public ExcelWorkflowController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("[action]")]
        public List<WorkflowDataRequest> getExcelData()
        {
            WorkflowService workflowService = new WorkflowService();
            List<WorkflowDataRequest> workflowDataRequests = new List<WorkflowDataRequest>();
            workflowDataRequests = workflowService.getExcelData();
            return workflowDataRequests;
        }
    }
}
