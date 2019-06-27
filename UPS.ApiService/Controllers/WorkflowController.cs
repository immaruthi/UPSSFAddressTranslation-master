using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.UserData;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.AddressTranslationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class WorkflowController : ControllerBase
    {
        private WorkflowService workflowService {get; set; }
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        [Route("CreateWorkflow")]
        [HttpPost]
        public ActionResult<WorkflowDataResponse> CreateWorkflow(IFormFile fromFile, int Emp_Id)
        {
            WorkflowDataResponse workflowDataResponse = new WorkflowDataResponse();
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.FLE_NA = fromFile.FileName;
            workflowDataRequest.CRD_BY_NR = Emp_Id;
            workflowDataRequest.CRD_DT = DateTime.Parse(DateTime.Now.ToString()).ToLocalTime();
            workflowDataRequest.WFL_STA_TE = 1;
            workflowService = new WorkflowService();
            workflowDataResponse = workflowService.InsertWorkflow(workflowDataRequest);
            return Ok(workflowDataResponse);
        }

        [Route("GetWorkflowData")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetWorkflowData([FromBody] USR user)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var context = new ApplicationDbContext(optionsBuilder.Options);
            WorkflowDataRequest workflow = context.workflowDataRequests.Where(w => w.CRD_BY_NR == user.ID).FirstOrDefault();
            return Ok(workflow);
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("UpdateWorkflowStatusById")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateWorkflowStatusById([FromBody] WorkflowDataRequest workflowDataRequest)
        {
            workflowService = new WorkflowService();
            WorkflowDataResponse workflowDataResponse = workflowService.UpdateWorkflowStatusById(workflowDataRequest);
            return Ok(workflowDataResponse);
        }
    }
}