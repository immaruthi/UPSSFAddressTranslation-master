namespace AtService.Controllers
{
    using AtService.HeadController;
    using ExcelFileRead;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using UPS.AddressTranslationService.Controllers;
    using UPS.Application.CustomLogs;
    using UPS.DataObjects.ADR_ADT_LG;
    using UPS.DataObjects.Common;
    using UPS.DataObjects.Shipment;
    using UPS.DataObjects.WR_FLW;
    using UPS.Quincus.APP;
    using UPS.Quincus.APP.Request;
    using UPS.Quincus.APP.Response;
    using UPS.ServicesAsyncActions;
    using UPS.ServicesDataRepository;
    using UPS.ServicesDataRepository.Common;
    using UPS.ServicesDataRepository.DataContext;

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    [Authorize]
    public class ShipmentController : UPSController
    {
        public ICustomLog iCustomLog { get; set; }
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private IEntityValidationService _entityValidationService;
        private IAddressBookService _addressBookService;
        private ShipmentDataResponse shipmentDataResponse;

        //private ShipmentService shipmentService { get; set; }
        private WorkflowService _workflowService { get; set; }
        private IShipmentAsync _shipmentService { get; set; }
        private IShipperCompanyAsync _shipperCompanyService { get; set; }
        private IAddressAuditLogAsync addressAuditLogService { get; set; }

        private IQuincusAddressTranslationRequest _quincusAddressTranslationRequest { get; set; }

        public ShipmentController(
            IConfiguration Configuration,
            IHostingEnvironment HostingEnvironment,
            IQuincusAddressTranslationRequest QuincusAddressTranslationRequest,
            IShipmentAsync shipmentAsync,
            IAddressBookService addressBookService,
            IAddressAuditLogAsync addressAuditLogAsync,
            IEntityValidationService entityValidationService,
            ApplicationDbContext applicationDbContext,
            IShipperCompanyAsync shipperCompanyAsync
            )
        {
            this.configuration = Configuration;
            this._hostingEnvironment = HostingEnvironment;
            this._shipmentService = shipmentAsync;
            this._context = applicationDbContext;
            this._addressBookService = addressBookService;
            this._entityValidationService = entityValidationService;
            this._workflowService = new WorkflowService(_context, _addressBookService,_entityValidationService);
            this._quincusAddressTranslationRequest = QuincusAddressTranslationRequest;
            this.addressAuditLogService = addressAuditLogAsync;
            this._shipperCompanyService = shipperCompanyAsync;
        }

        private static int _workflowID = 0;
        [Route("ExcelFileUpload")]
        [HttpPost]
      
        public async Task<ActionResult> ExcelFile(IList<IFormFile> excelFileName)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId).Value);
                ShipmentDataResponse result = null;
                //string response = string.Empty;
                if (excelFileName != null)
                {

                    //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    foreach (var file in excelFileName)
                    {
                        if (file.Length > 0)
                        {
                            //string paths = hostingEnvironment.WebRootPath;

                            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, file.FileName);

                            //var filePath = Path.Combine(@"D:\UserExcels", file.FileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                //FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                                //response = new ExcelExtension().Test(filePath);
                                await file.CopyToAsync(fileStream);
                            }


                            ExcelExtensionReponse excelExtensionReponse = new ExcelExtension().Test(filePath);
                            if (excelExtensionReponse.success)
                            {
                                var excelDataObject2 = JsonConvert.DeserializeObject<List<ExcelDataObject>>(excelExtensionReponse.ExcelExtensionReponseData);
                                WorkflowController workflowController = new WorkflowController(this._hostingEnvironment, this._context, this._addressBookService, this._entityValidationService);
                                WorkflowDataResponse response = ((WorkflowDataResponse)((ObjectResult)(workflowController.CreateWorkflow(file, userId)).Result).Value);
                                _workflowID = response.Workflow.ID;
                                result = _shipmentService.CreateShipments(excelDataObject2, _workflowID, out int? workflowStatus);
                                if (result.Success)
                                {
                                    shipmentDataResponse.Success = true;
                                    shipmentDataResponse.Shipments = result.Shipments;
                                    WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                                    workflowDataRequest.ID = _workflowID;
                                    workflowDataRequest.WFL_STA_TE = workflowStatus;
                                    _workflowService.UpdateWorkflowStatusById(workflowDataRequest);
                                }
                                else
                                {
                                    shipmentDataResponse.Success = false;
                                    shipmentDataResponse.OperationExceptionMsg = result.OperationExceptionMsg;
                                    WorkflowService workflowService = new WorkflowService(_context,_addressBookService,_entityValidationService);
                                    workflowService.DeleteWorkflowById(_workflowID);
                                }
                            }
                            else
                            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                                Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                                {
                                    apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes),7),
                                    dateTime = System.DateTime.Now,
                                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                                    {
                                        LogException = excelExtensionReponse.exception.InnerException.ToString(),
                                        LogRequest = "Excel Uploaded",
                                        LogResponse = JsonConvert.SerializeObject(excelExtensionReponse)
                                    }
                                }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                                return Ok(excelExtensionReponse);
                            }
                        }
                    }
                }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                {
                    apiTypes = UPS.DataObjects.LogData.APITypes.ExcelUpload,
                    apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes), 7),
                    dateTime = System.DateTime.Now,
                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                    {
                        LogException = null,
                        LogRequest = "Excel Uploaded",
                        LogResponse = JsonConvert.SerializeObject(shipmentDataResponse)
                    }
                }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                return Ok(shipmentDataResponse);
            }
            catch (Exception ex)
            {
                // new AuditEventEntry.WriteEntry(new Exception(ex.Message));
                return Ok(shipmentDataResponse.OperationExceptionMsg = ex.Message);
            }
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("UpdateShipmentStatusById")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateShipmentStatusById([FromBody] ShipmentDataRequest shipmentDataRequest)
        {
            //shipmentService = new ShipmentService();
            ShipmentDataResponse shipmentDataResponse = _shipmentService.UpdateShipmentStatusById(shipmentDataRequest);
            if (!shipmentDataResponse.Success)
            {
                //AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            return Ok(shipmentDataResponse);
        }

        [Route("UpdateShipmentAddressById")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateShipmentAddressById([FromBody] ShipmentDataRequest shipmentDataRequest)
        {
            //shipmentService = new ShipmentService();
            string id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId)?.Value;

            int userId = !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0;
            ShipmentDataResponse shipmentDataResponse = _shipmentService.UpdateShipmentAddressById(shipmentDataRequest);
            if (shipmentDataResponse.Success && !string.IsNullOrEmpty(shipmentDataResponse.BeforeAddress))
            {
                try
                {
                    //AddressAuditLog Update
                    AddressAuditLogRequest addressAuditLogRequest = new AddressAuditLogRequest();
                    addressAuditLogRequest.SMT_ID = shipmentDataRequest.ID;
                    addressAuditLogRequest.CSG_ADR = shipmentDataRequest.RCV_ADR_TE;
                    addressAuditLogRequest.BFR_ADR = shipmentDataResponse.BeforeAddress;
                    addressAuditLogRequest.AFR_ADR = shipmentDataResponse.ShipmentDataRequest.SHP_ADR_TR_TE;
                    addressAuditLogRequest.UPD_BY = userId;
                    addressAuditLogRequest.UPD_FRM = "Shipment";
                    addressAuditLogRequest.UPD_DT = DateTime.Parse(DateTime.Now.ToString()).ToLocalTime();
                    addressAuditLogRequest.WFL_ID = shipmentDataResponse.ShipmentDataRequest.WFL_ID;
                    AddressAuditLogResponse addressAuditLogResponse = addressAuditLogService.Insert(addressAuditLogRequest);
                    if (addressAuditLogResponse.Success)
                    {
                        // TO DO
                    }
                    else
                    {
                        // Log the error here
                    }

                }
                catch (Exception ex)
                {

                }

            }

            //we need to update the workflow status
            int? workflowstatus = _shipmentService.SelectShipmentTotalStatusByWorkflowId(shipmentDataRequest.WFL_ID);
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.ID = shipmentDataRequest.WFL_ID;
            workflowDataRequest.WFL_STA_TE = workflowstatus;
            _workflowService.UpdateWorkflowStatusById(workflowDataRequest);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
            {
                apiTypes = UPS.DataObjects.LogData.APITypes.EFCoreContext,
                apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes), 6),
                dateTime = System.DateTime.Now,
                LogInformation = new UPS.DataObjects.LogData.LogInformation()
                {
                    LogException = null,
                    LogRequest = JsonConvert.SerializeObject(shipmentDataRequest),
                    LogResponse = JsonConvert.SerializeObject(shipmentDataResponse)
                }
            }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return Ok(shipmentDataResponse);
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("GetShipmentData")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public List<ShipmentDataRequest> GetShipmentData(int wid)
        {
            //shipmentService = new ShipmentService();
            List<ShipmentDataRequest> shipmentDataRequests = _shipmentService.GetShipment(wid);

            //we need to update the workflow status
            int? workflowstatus = _shipmentService.SelectShipmentTotalStatusByWorkflowId(wid);
            WorkflowService workflowService = new WorkflowService(_context, _addressBookService, _entityValidationService);
            WorkflowDataResponse workflowDataResponse = workflowService.SelectWorkflowById(wid);
            if(workflowDataResponse.Success && workflowDataResponse.Workflow != null)
            {
                if(workflowstatus != workflowDataResponse.Workflow.WFL_STA_TE)
                {
                    WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                    workflowDataRequest.ID = wid;
                    workflowDataRequest.WFL_STA_TE = workflowstatus;
                    workflowService.UpdateWorkflowStatusById(workflowDataRequest);
                }
            }

            return shipmentDataRequests;
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("GetAllShipmentData")]
        [HttpGet]
        public List<ShipmentDataRequest> GetAllShipmentData(int wid)
        {
            //shipmentService = new ShipmentService();
            List<ShipmentDataRequest> shipmentDataRequests = _shipmentService.GetAllShipment(wid);

            //we need to update the workflow status
            int? workflowstatus = _shipmentService.SelectShipmentTotalStatusByWorkflowId(wid);
            WorkflowService workflowService = new WorkflowService(_context, _addressBookService, _entityValidationService);
            WorkflowDataResponse workflowDataResponse = workflowService.SelectWorkflowById(wid);
            if (workflowDataResponse.Success && workflowDataResponse.Workflow != null)
            {
                if (workflowstatus != workflowDataResponse.Workflow.WFL_STA_TE)
                {
                    WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                    workflowDataRequest.ID = wid;
                    workflowDataRequest.WFL_STA_TE = workflowstatus;
                    workflowService.UpdateWorkflowStatusById(workflowDataRequest);
                }
            }

            return shipmentDataRequests;
        }

        [Route("CreateOrderShipment")]
        [HttpPost]
        public async Task<ActionResult> CreateOrderShipment([FromBody] List<UIOrderRequestBodyData> uIOrderRequestBodyDatas)
        {
            string customerID = uIOrderRequestBodyDatas[0].spC_CST_ID_TE;//_shipmentService.GetShipmentCustomCodesInformation().CST_ID;
            _workflowID = uIOrderRequestBodyDatas[0].wfL_ID;
            CreateOrderShipmentResponse createOrderShipmentResponse = new CreateOrderShipmentResponse();
            createOrderShipmentResponse.FailedToProcessShipments = new List<string>();
            createOrderShipmentResponse.ProcessedShipments = new List<string>();
            //ShipmentService shipmentService = new ShipmentService();

            //List<UIOrderRequestBodyData> uIOrderRequestBodyDatas = new List<UIOrderRequestBodyData>();

            foreach (var orderRequest in uIOrderRequestBodyDatas)
            {
                string XMLMessage = string.Empty;

                XMLMessage = "<Request lang=\"zh-CN\" service=\"OrderService\">";
                XMLMessage += "<Head>" + configuration["SFExpress:Access Number"] + "</Head>";
                XMLMessage += "<Body>";
                XMLMessage += "<Order orderid=\"" + orderRequest.pkG_NR_TE + "\" custid=\"" + orderRequest.spC_CST_ID_TE + "\"";
                XMLMessage += " parcel_quantity=\"" + orderRequest.pcS_QTY_NR + "\"";
                XMLMessage += " total_net_weight=\"" + orderRequest.pkG_WGT_DE + "\"";
                XMLMessage += " j_company=\"" + orderRequest.shP_CPY_NA + "\"";
                XMLMessage += " j_address=\"" + orderRequest.shP_ADR_TE + "\"";
                XMLMessage += " j_city=\"" + orderRequest.orG_CTY_TE + "\"";
                XMLMessage += " j_post_code=\"" + orderRequest.orG_PSL_CD + "\"";
                XMLMessage += " j_contact=\"" + orderRequest.shP_CTC_TE + "\"";
                XMLMessage += " j_tel=\"" + orderRequest.shP_PH_TE + "\"";
                XMLMessage += " d_company=\"" + orderRequest.rcV_CPY_TE + "\"";
                XMLMessage += " d_city=\"" + orderRequest.dsT_CTY_TE + "\"";
                XMLMessage += " d_post_code=\"" + orderRequest.dsT_PSL_TE + "\"";
                XMLMessage += " d_contact=\"" + orderRequest.csG_CTC_TE + "\"";
                XMLMessage += " d_tel=\"" + orderRequest.pH_NR + "\"";
                XMLMessage += " specifications=\"" + orderRequest.fsT_INV_LN_DES_TE + "\"";
                XMLMessage += " d_address=\"" + orderRequest.shP_ADR_TR_TE + "\" cargo_total_weight=\"" + orderRequest.pkG_WGT_DE + "\"";
                XMLMessage += " pay_method=\"1\" is_docall=\"" + 1 + "\" need_return_tracking_no=\"" + orderRequest.poD_RTN_SVC + "\" express_type=\"6\"";
                XMLMessage += " >";
                XMLMessage += " </Order><AddedService name='COD' value=\"" + orderRequest.coD_TE + "\"></AddedService></Body></Request>";


                SFCreateOrderServiceRequest sFCreateOrderServiceRequest = new SFCreateOrderServiceRequest()
                {
                    AccessNumber = configuration["SFExpress:Access Number"],
                    BaseURI = configuration["SFExpress:Base URI"],
                    Checkword = configuration["SFExpress:Checkword"],
                    RequestURI = configuration["SFExpress:Place Order URI"],
                    Checkcode = configuration["SFExpress:CheckCode"],
                    RequestOrderXMLMessage = XMLMessage,

                };

                GetSFCreateOrderServiceResponse getSFCreateOrderServiceResponse = QuincusService.SFExpressCreateOrder(sFCreateOrderServiceRequest);

                //shipmentDataResponse = shipmentService.UpdateShipmentStatusById(shipmentDataRequest);
                //if (!shipmentDataResponse.Success)
                //{
                //    AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
                //}

                if (getSFCreateOrderServiceResponse.Response)
                {
                    XmlDocument xmlDocumentShipmentResponse = new XmlDocument();
                    xmlDocumentShipmentResponse.LoadXml(getSFCreateOrderServiceResponse.OrderResponse);

                    string xmlDocumentShipmentResponseParser = xmlDocumentShipmentResponse.InnerXml;

                    if (xmlDocumentShipmentResponseParser.Contains("<ERROR"))
                    {
                        XmlDocument xmlDocument = new XmlDocument();

                        xmlDocument.LoadXml(getSFCreateOrderServiceResponse.OrderResponse);

                        //if (xmlDocumentShipmentResponseParser.Contains("8019"))
                        //{
                        //    createOrderShipmentResponse.FailedToProcessShipments.Add("Customer order number(" + orderRequest.pkG_NR_TE + ") is already confirmed");
                        //}
                        //else if (xmlDocumentShipmentResponseParser.Contains("8016"))
                        //{
                        //    createOrderShipmentResponse.FailedToProcessShipments.Add("Repeat order numbers ( " + orderRequest.pkG_NR_TE + " )");
                        //}
                        //else
                        //{
                            createOrderShipmentResponse.FailedToProcessShipments.Add(
                                string.Format("{0}:{1}:{2}",
                                orderRequest.pkG_NR_TE,
                                xmlDocument.GetElementsByTagName("ERROR")[0].Attributes[0].InnerText,
                                xmlDocument.GetElementsByTagName("ERROR")[0].InnerXml));
                        //}
                    }
                    else
                    {
                        createOrderShipmentResponse.ProcessedShipments.Add(orderRequest.pkG_NR_TE);

                        ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                        shipmentDataRequest.ID = orderRequest.id;
                        shipmentDataRequest.WFL_ID = orderRequest.wfL_ID;
                        shipmentDataRequest.SMT_STA_NR = ((int)Enums.ATStatus.Completed);
                        shipmentDataRequest.SMT_STA_TE = "Completed";
                        _workflowID = orderRequest.wfL_ID;

                        _shipmentService.UpdateShipmentStatusById(shipmentDataRequest);
                    }

                    createOrderShipmentResponse.Response = true;
                }
                else
                {
                    createOrderShipmentResponse.Response = false;
                }
            }
            //we need to update the workflow status
            int? workflowstatus = _shipmentService.SelectShipmentTotalStatusByWorkflowId(_workflowID);
            WorkflowService workflowService = new WorkflowService(_context,_addressBookService,_entityValidationService);
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.ID = _workflowID;
            workflowDataRequest.WFL_STA_TE = workflowstatus;
            workflowService.UpdateWorkflowStatusById(workflowDataRequest);



#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
            {
                apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes), 1),
                dateTime = System.DateTime.Now,
                LogInformation = new UPS.DataObjects.LogData.LogInformation()
                {
                    LogException = null,
                    LogRequest = JsonConvert.SerializeObject(uIOrderRequestBodyDatas),
                    LogResponse = JsonConvert.SerializeObject(createOrderShipmentResponse)
                }
            }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return Ok(createOrderShipmentResponse);
        }

        [Route("CancelOrderShipment")]
        [HttpPost]
        public async Task<ActionResult> CancelOrderShipment([FromBody] SFOrderXMLRequest sFOrderXMLRequest)
        {
            //sFOrderXMLRequest.XMLMessage = "<Request service=\"OrderConfirmService\" lang=\"zh-CN\"><Head>LJ_T6NVV</Head><Body><OrderConfirm orderid=\"19066630505714563\" dealtype=\"2\"></OrderConfirm></Body></Request>"; 

            SFCancelOrderServiceRequest sFCancelOrderServiceRequest = new SFCancelOrderServiceRequest()
            {
                AccessNumber = configuration["SFExpress:Access Number"],
                BaseURI = configuration["SFExpress:Base URI"],
                Checkword = configuration["SFExpress:Checkword"],
                RequestURI = configuration["SFExpress:Cancel Order URI"],
                RequestOrderXMLMessage = sFOrderXMLRequest.XMLMessage,

            };

            GetSFCancelOrderServiceResponse getSFCancelOrderServiceResponse = QuincusService.SFExpressCancelOrder(sFCancelOrderServiceRequest);

            if (getSFCancelOrderServiceResponse.Response)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                {
                    apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                    apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes), 1),
                    dateTime = System.DateTime.Now,
                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                    {
                        LogException = null,
                        LogRequest = JsonConvert.SerializeObject(sFOrderXMLRequest),
                        LogResponse = JsonConvert.SerializeObject(getSFCancelOrderServiceResponse)
                    }
                }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                return Ok(getSFCancelOrderServiceResponse.OrderResponse);
            }
            else
            {

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                {
                    apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                    apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes), 1),
                    dateTime = System.DateTime.Now,
                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                    {
                        LogException = getSFCancelOrderServiceResponse.exception.InnerException.ToString(),
                        LogRequest = JsonConvert.SerializeObject(sFOrderXMLRequest),
                        LogResponse = null
                    }
                }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                //AuditEventEntry.WriteEntry(new Exception(getSFCancelOrderServiceResponse.exception.ToString()));
                return Ok(getSFCancelOrderServiceResponse.exception);
            }

        }

        [Route("DeleteShipments")]
        [HttpPost]
        public async Task<ActionResult> DeleteShipments([FromBody] List<ShipmentDataRequest> shipmentDataRequests)
        {
            //ShipmentService shipmentService = new ShipmentService();
            ShipmentDataResponse shipmentDataResponse = _shipmentService.DeleteShipments(shipmentDataRequests);
            return Ok(shipmentDataResponse);
        }

        [Route("GetTranslationAddress")]
        [HttpPost]
        public async Task<ActionResult> GetTranslationAddress([FromBody] List<ShipmentDataRequest> _shipmentDataRequest)
        {
            int wid = 0;
            if (_shipmentDataRequest.Any())
            {
                wid = _shipmentDataRequest.FirstOrDefault().WFL_ID;
            }
            QuincusTranslatedAddressResponse quincusTranslatedAddressResponse = new QuincusTranslatedAddressResponse();

            QuincusTokenDataResponse quincusTokenDataResponse = QuincusService.GetToken(new UPS.Quincus.APP.Configuration.QuincusParams()
            {
                endpoint = configuration["Quincus:TokenEndPoint"],
                password = configuration["Quincus:Password"],
                username = configuration["Quincus:UserName"],

            });

            if (quincusTokenDataResponse.ResponseStatus)
            {
                List<ShipmentWorkFlowRequest> shipmentWorkFlowRequests =
                    _shipmentDataRequest.Select(_ =>
                    new ShipmentWorkFlowRequest()
                    {
                        id = _.ID,
                        rcV_ADR_TE = _.RCV_ADR_TE,
                        dsT_CTY_TE = _.DST_CTY_TE,
                        wfL_ID = _.WFL_ID,
                        pkG_NR_TE = _.PKG_NR_TE,
                        rcV_CPY_TE = _.RCV_CPY_TE
                    }).ToList();

                this._quincusAddressTranslationRequest.shipmentWorkFlowRequests = shipmentWorkFlowRequests;
                this._quincusAddressTranslationRequest.token = quincusTokenDataResponse.quincusTokenData.token;

                quincusTranslatedAddressResponse = QuincusService.GetTranslationAddress(this._quincusAddressTranslationRequest);

                if (quincusTranslatedAddressResponse.Response)
                {
                    var getAddressTranslation = quincusTranslatedAddressResponse.ResponseData;

                    List<string> batchIds = new List<string>();

                    Dictionary<string, string> shipmentDetailsDictionary = new Dictionary<string, string>();
                    quincusTranslatedAddressResponse.ResponseData.ForEach(batches =>
                    {
                        batchIds.Add(batches.batch_id);

                        batches.addresses.ForEach(address =>
                        {
                            shipmentDetailsDictionary.Add(address.id, address.rcV_CPY_TE);
                        });
                       
                    });

                    var QuincusResponse = QuincusService.GetGeoCodeReponseFromQuincus(new UPS.Quincus.APP.Request.QuincusGeoCodeDataRequest()
                    {
                        endpoint = configuration["Quincus:GeoCodeEndPoint"],
                        batchIDList = batchIds,
                        quincusTokenData = quincusTokenDataResponse.quincusTokenData,
                        ShipmentDetailsDictionary = shipmentDetailsDictionary
                    });

                    if (QuincusResponse.ResponseStatus)
                    {
                        // Insert Address into AddressBook
                        _addressBookService.InsertAddress(QuincusResponse.QuincusReponseDataList,shipmentDetailsDictionary);

                        try
                        {
                            var requestIds = _shipmentDataRequest.Select(_ => _.ID).ToList();
                            List<ShipmentDataRequest> existingShipmentDetails =
                                this._context.shipmentDataRequests
                                .Where(ShpDetail =>
                                    ShpDetail.WFL_ID == wid
                                    &&
                                        (ShpDetail.SMT_STA_NR == ((int)Enums.ATStatus.Uploaded)
                                        || ShpDetail.SMT_STA_NR == ((int)Enums.ATStatus.Curated))
                                     && (!requestIds.Contains(ShpDetail.ID))
                                    )
                                .ToList();


                            QuincusResponse.QuincusReponseDataList.ForEach(datalist =>
                            {
                                List<Geocode> geocodes = (List<Geocode>)((QuincusReponseData)datalist).geocode;
                                List<ShipmentDataRequest> shipmentDataRequestList = new List<ShipmentDataRequest>(geocodes.Count);

                                foreach (Geocode geocode in geocodes)
                                {
                                    ShipmentDataRequest currentShipmentDataRequest =
                                           _shipmentDataRequest.FirstOrDefault(_ => _.PKG_NR_TE == geocode.id);
                                    ShipmentDataRequest shipmentDataRequest = CreateShipmentAddressUpdateRequest(currentShipmentDataRequest, geocode);

                                    shipmentDataRequestList.Add(shipmentDataRequest);

                                    // Checking any same address are avaible, If there then updating those address also

                                    List<ShipmentDataRequest> sameAddressShpRequest =
                                        existingShipmentDetails.Where(
                                            (ShipmentDataRequest data) =>
                                                data.RCV_ADR_TE.ToLower().Replace(" ", "")
                                                .Equals(currentShipmentDataRequest.RCV_ADR_TE.ToLower().Replace(" ", ""))
                                                )
                                        .ToList();
                                    if (sameAddressShpRequest.Any())
                                    {
                                        sameAddressShpRequest.ForEach(shpDetails =>
                                        {
                                            var sameaddressRequest = CreateShipmentAddressUpdateRequest(shpDetails, geocode);
                                            shipmentDataRequestList.Add(sameaddressRequest);

                                        });
                                    }
                                }

                                shipmentDataRequestList = shipmentDataRequestList.GroupBy(x => x.ID).Select(x => x.First()).ToList();
                                _shipmentService.UpdateShipmentAddressByIds(shipmentDataRequestList);

                                //we need to update the workflow status
                                int? workflowstatus = _shipmentService.SelectShipmentTotalStatusByWorkflowId(wid);
                                WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                                workflowDataRequest.ID = wid;
                                workflowDataRequest.WFL_STA_TE = workflowstatus;
                                _workflowService.UpdateWorkflowStatusById(workflowDataRequest);
                            });
                        }
                        catch (Exception exception)
                        {

                        }

                       

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                        {
                            apiTypes = UPS.DataObjects.LogData.APITypes.QuincusAPI_Translation,
                            apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes), 4),
                            dateTime = System.DateTime.Now,
                            LogInformation = new UPS.DataObjects.LogData.LogInformation()
                            {
                                LogException = null,
                                LogRequest = quincusTranslatedAddressResponse.QuincusContentRequest,
                                LogResponse = JsonConvert.SerializeObject(QuincusResponse.QuincusReponseDataList)
                            }
                        }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                        return Ok(QuincusResponse.QuincusReponseDataList);
                    }
                    else
                    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                        {
                            apiTypes = UPS.DataObjects.LogData.APITypes.QuincusAPI_Translation,
                            apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes), 4),
                            dateTime = System.DateTime.Now,
                            LogInformation = new UPS.DataObjects.LogData.LogInformation()
                            {
                                LogException = QuincusResponse.Exception.InnerException.ToString(),
                                LogRequest = quincusTranslatedAddressResponse.QuincusContentRequest,
                                LogResponse = null
                            }
                        }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        return Ok(QuincusResponse.Exception);
                    }

                }
                else
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Run(()=>iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                    {
                        apiTypes = UPS.DataObjects.LogData.APITypes.QuincusAPI_Translation,
                        apiType = Enum.GetName(typeof(UPS.DataObjects.LogData.APITypes),4),
                        dateTime = System.DateTime.Now,
                        LogInformation = new UPS.DataObjects.LogData.LogInformation()
                        {
                            LogException = quincusTranslatedAddressResponse.exception.InnerException.ToString(),
                            LogRequest = JsonConvert.SerializeObject(_shipmentDataRequest),
                            LogResponse = null
                        }
                    }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    return Ok(quincusTranslatedAddressResponse.exception);
                }

            }
            else
            {
                return Ok(quincusTokenDataResponse.exception);
            }
        }

        private  ShipmentDataRequest CreateShipmentAddressUpdateRequest(ShipmentDataRequest shipmentDataRequest, Geocode geocode)
        {
            
            shipmentDataRequest.SHP_ADR_TR_TE = geocode.translated_adddress;
            shipmentDataRequest.ACY_TE = geocode.accuracy;
            shipmentDataRequest.CON_NR = geocode.confidence;
            shipmentDataRequest.TranslationScore = geocode.translation_score;

            if (
                        !string.IsNullOrEmpty(geocode.translated_adddress)

               )
            {
                shipmentDataRequest.SMT_STA_NR = ((int)Enums.ATStatus.Translated);
            }
            else
            {
                shipmentDataRequest.SMT_STA_NR = shipmentDataRequest.SMT_STA_NR;
            }

            return shipmentDataRequest;
        }

        [Route("UpdateShipmentCode")]
        [HttpPost]
        public async Task<ActionResult> UpdateShipmentCode([FromBody] ShipmentGeoCodes shipmentGeoCodes)
        {
            QuincusResponse quincusResponse = null;

            QuincusTokenDataResponse quincusTokenDataResponse = QuincusService.GetToken(new UPS.Quincus.APP.Configuration.QuincusParams()
            {
                endpoint = configuration["Quincus:TokenEndPoint"],
                password = configuration["Quincus:Password"],
                username = configuration["Quincus:UserName"],

            });

            if (quincusTokenDataResponse.ResponseStatus)
            {

                quincusResponse = QuincusService.GetGeoCodeReponseFromQuincus(new UPS.Quincus.APP.Request.QuincusGeoCodeDataRequest()
                {
                    endpoint = configuration["Quincus:GeoCodeEndPoint"],
                    batchIDList = shipmentGeoCodes.geoCode,
                    quincusTokenData = quincusTokenDataResponse.quincusTokenData
                });

                if (quincusResponse.ResponseStatus)
                {
                    if (quincusResponse.QuincusReponseData != null)
                    {

                        IList<Geocode> geocodes = quincusResponse.QuincusReponseData.geocode;

                        string TranslatedCode = geocodes[0].translated_adddress;

                        return Ok(quincusResponse.QuincusReponseData);
                    }
                }
            }
            else
            {
                //AuditEventEntry.WriteEntry(new Exception(quincusTokenDataResponse.exception.ToString()));
                return Ok(quincusTokenDataResponse.exception);
            }

            return Ok("Error");
        }

        [Route("GetMatchedShipmentsWithShipperCompanies")]
        [HttpGet]
        public IActionResult GetMatchedShipmentsWithShipperCompanies(int wid)
        {
            string id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserId)?.Value;
            if (string.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }

            int userId = Convert.ToInt32(id);
            shipmentDataResponse = this._shipperCompanyService.SelectMatchedShipmentsWithShipperCompanies(wid, userId);
            if (!shipmentDataResponse.Success)
            {
                //AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            //else
            //{
            //    var json = JsonConvert.SerializeObject(shipmentDataResponse.Shipments).ToString();
            //    AuditEventEntry.WriteEntry(new Exception(json));
            //}
            return Ok(shipmentDataResponse);
        }

        [Route("GetCompletedShipments")]
        [HttpGet]
        public ShipmentDataResponse GetCompletedShipments(int wid)
        {
            shipmentDataResponse = this._shipperCompanyService.SelectCompletedShipments(wid);
            if (!shipmentDataResponse.Success)
            {
                //AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            return shipmentDataResponse;
        }

        [Route("GetAddressAuditLogData")]
        [HttpGet]
        public List<AddressAuditLogRequest> GetAddressAuditLogData()
        {
            //shipmentService = new ShipmentService();
            List<AddressAuditLogRequest> addressAuditLogRequest = addressAuditLogService.GetAll();
            return addressAuditLogRequest;
        }
    }
}

