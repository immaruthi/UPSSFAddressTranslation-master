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
using UPS.ServicesAsyncActions;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class ExcelWorkflowController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _context;
        private IAddressBookService _addressBookService;
        private IEntityValidationService _entityValidationService;
        private DbContextOptionsBuilder<ApplicationDbContext> _optionsBuilder;

        public ExcelWorkflowController(IHostingEnvironment hostingEnvironment, ApplicationDbContext applicationDbContext,
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
            WorkflowService workflowService = new WorkflowService(_context,_addressBookService,_entityValidationService);
            List<WorkflowDataRequest> workflowDataRequests = new List<WorkflowDataRequest>();
            workflowDataRequests = workflowService.getExcelData();
            return workflowDataRequests;
        }
    }
}
