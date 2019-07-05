using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.UserData;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.AddressTranslationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        private IWorkflowService workflowService {get; set; }
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public WorkflowController(IWorkflowService workflowService)
        {
            this.workflowService = workflowService;
        }
        [Route("CreateWorkflow")]
        [HttpPost]
        public ActionResult<WorkflowDataResponse> CreateWorkflow(string fileName, int Emp_Id)
        {
            WorkflowDataResponse workflowDataResponse = workflowService.InsertWorkflow( fileName,Emp_Id);
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
            Workflow workflow = 
                context.Workflows.Where(
                    w => w.CRD_BY_NR == user.ID 
                         && w.WFL_STA_TE != (int)Enums.ATStatus.Inactive).FirstOrDefault();
            return Ok(workflow);
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("UpdateWorkflowStatusById")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateWorkflowStatusById([FromBody] Workflow workflowDataRequest)
        {
            WorkflowDataResponse workflowDataResponse = workflowService.UpdateWorkflowStatusById(workflowDataRequest);
            return Ok(workflowDataResponse);
        }

        /// <summary>
        /// This Api is for getting all workflow records 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAllWorkFlow()
        {
            return Ok(await workflowService.GetAllWorkFlow());
        }
    }
}