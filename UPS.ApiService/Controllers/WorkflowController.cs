using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
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
    public class WorkflowController : ControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;
        private WorkflowService _workflowService { get; set; }

        private readonly ApplicationDbContext _context;
        private IAddressBookService _addressBookService;
        private IEntityValidationService _entityValidationService;
        private DbContextOptionsBuilder<ApplicationDbContext> _optionsBuilder;

        public WorkflowController(IHostingEnvironment hostingEnvironment, ApplicationDbContext applicationDbContext,
            IAddressBookService addressBookService,
            IEntityValidationService entityValidationService)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = applicationDbContext;
            _addressBookService = addressBookService;
            _entityValidationService = entityValidationService;
            _optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        }
        [HttpGet("[action]")]
        public List<WorkflowDataRequest> getExcelData()
        {
            WorkflowService workflowService = new WorkflowService(this._context,this._addressBookService,this._entityValidationService);
            List<WorkflowDataRequest> workflowDataRequests = new List<WorkflowDataRequest>();
            workflowDataRequests = workflowService.getExcelData();
            return workflowDataRequests;
        }


        [Route("CreateWorkflow")]
        [HttpPost]
        public ActionResult<WorkflowDataResponse> CreateWorkflow(IFormFile fromFile, int Emp_Id)
        {
            WorkflowDataResponse workflowDataResponse = new WorkflowDataResponse();
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.FLE_NA = fromFile.FileName;
            workflowDataRequest.CRD_BY_NR = Emp_Id;
            workflowDataRequest.CRD_DT = DateTime.Parse(DateTime.Now.ToString()).ToLocalTime();
            workflowDataRequest.WFL_STA_TE = 0;
            _workflowService = new WorkflowService(this._context,this._addressBookService,this._entityValidationService);
            workflowDataResponse = _workflowService.InsertWorkflow(workflowDataRequest);
            return Ok(workflowDataResponse);
        }

        [Route("GetWorkflowData")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetWorkflowData([FromBody] USR user)
        {
            _optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var context = new ApplicationDbContext(_optionsBuilder.Options);
            WorkflowDataRequest workflow = 
                context.workflowDataRequests.Where(
                    w => w.CRD_BY_NR == user.ID 
                         && w.WFL_STA_TE != (int)Enums.ATStatus.Inactive).FirstOrDefault();
            return Ok(workflow);
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("UpdateWorkflowStatusById")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateWorkflowStatusById([FromBody] WorkflowDataRequest workflowDataRequest)
        {
            _workflowService = new WorkflowService(this._context,this._addressBookService,this._entityValidationService);
            WorkflowDataResponse workflowDataResponse = _workflowService.UpdateWorkflowStatusById(workflowDataRequest);
            return Ok(workflowDataResponse);
        }
    }
}