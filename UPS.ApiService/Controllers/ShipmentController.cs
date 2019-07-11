namespace AtService.Controllers
{
    using AtService.HeadController;
    using ExcelFileRead;
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

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class ShipmentController : UPSController
    {
        public ICustomLog iCustomLog { get; set; }
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment hostingEnvironment;
        private IAddressBookService addressBookService;
        private ShipmentDataResponse shipmentDataResponse;

        //private ShipmentService shipmentService { get; set; }
        private WorkflowService workflowService { get; set; }
        private IShipmentAsync shipmentService { get; set; }
        private IAddressAuditLogAsync addressAuditLogService { get; set; }

        private IQuincusAddressTranslationRequest _quincusAddressTranslationRequest { get; set; }

        public ShipmentController(
            IConfiguration Configuration,
            IHostingEnvironment HostingEnvironment,
            IQuincusAddressTranslationRequest QuincusAddressTranslationRequest,
            IShipmentAsync shipmentAsync,
            IAddressBookService addressBookService,
            IAddressAuditLogAsync addressAuditLogAsync
            )
        {
            this.configuration = Configuration;
            this.hostingEnvironment = HostingEnvironment;
            shipmentService = shipmentAsync;
            workflowService = new WorkflowService();
            _quincusAddressTranslationRequest = QuincusAddressTranslationRequest;
            this.addressBookService = addressBookService;
            this.addressAuditLogService = addressAuditLogAsync;
        }

        private static int _workflowID = 0;
        [Route("ExcelFileUpload/{Emp_Id}")]
        [HttpPost]
        public async Task<ActionResult> ExcelFile(IList<IFormFile> excelFileName, int Emp_Id)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
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

                            var filePath = Path.Combine(hostingEnvironment.WebRootPath, file.FileName);

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
                                WorkflowController workflowController = new WorkflowController();
                                WorkflowDataResponse response = ((WorkflowDataResponse)((ObjectResult)(workflowController.CreateWorkflow(file, Emp_Id)).Result).Value);
                                _workflowID = response.Workflow.ID;
                                result = shipmentService.CreateShipments(excelDataObject2, _workflowID);
                                if (result.Success)
                                {
                                    shipmentDataResponse.Success = true;
                                    shipmentDataResponse.Shipments = result.Shipments;
                                }
                                else
                                {
                                    shipmentDataResponse.Success = false;
                                    shipmentDataResponse.OperationExceptionMsg = result.OperationExceptionMsg;
                                    WorkflowService workflowService = new WorkflowService();
                                    workflowService.DeleteWorkflowById(_workflowID);
                                }
                            }
                            else
                            {
                                iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                                {
                                    apiTypes = UPS.DataObjects.LogData.APITypes.ExcelUpload,
                                    dateTime = System.DateTime.Now,
                                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                                    {
                                        LogException = excelExtensionReponse.exception,
                                        LogRequest = "Excel Uploaded",
                                        LogResponse = JsonConvert.SerializeObject(excelExtensionReponse)
                                    }
                                });
                                return Ok(excelExtensionReponse);
                            }
                        }
                    }
                }

                iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                {
                    apiTypes = UPS.DataObjects.LogData.APITypes.ExcelUpload,
                    dateTime = System.DateTime.Now,
                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                    {
                        LogException = null,
                        LogRequest = "Excel Uploaded",
                        LogResponse = JsonConvert.SerializeObject(shipmentDataResponse)
                    }
                });

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
            ShipmentDataResponse shipmentDataResponse = shipmentService.UpdateShipmentStatusById(shipmentDataRequest);
            if (!shipmentDataResponse.Success)
            {
                //AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            return Ok(shipmentDataResponse);
        }

        [Route("UpdateShipmentAddressById/{Emp_Id}")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateShipmentAddressById([FromBody] ShipmentDataRequest shipmentDataRequest, int Emp_Id)
        {
            //shipmentService = new ShipmentService();
            ShipmentDataResponse shipmentDataResponse = shipmentService.UpdateShipmentAddressById(shipmentDataRequest);
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
                    addressAuditLogRequest.UPD_BY = Emp_Id;
                    addressAuditLogRequest.UPD_FRM = "Shipment";
                    addressAuditLogRequest.UPD_DT = DateTime.Now;
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
            int? workflowstatus = shipmentService.SelectShipmentTotalStatusByWorkflowId(shipmentDataRequest.WFL_ID);
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.ID = shipmentDataRequest.WFL_ID;
            workflowDataRequest.WFL_STA_TE = workflowstatus;
            workflowService.UpdateWorkflowStatusById(workflowDataRequest);

            iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
            {
                apiTypes = UPS.DataObjects.LogData.APITypes.EFCoreContext,
                dateTime = System.DateTime.Now,
                LogInformation = new UPS.DataObjects.LogData.LogInformation()
                {
                    LogException = null,
                    LogRequest = JsonConvert.SerializeObject(shipmentDataRequest),
                    LogResponse = JsonConvert.SerializeObject(shipmentDataResponse)
                }
            });

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
            List<ShipmentDataRequest> shipmentDataRequests = shipmentService.GetShipment(wid);
            return shipmentDataRequests;
        }

        [Route("CreateOrderShipment")]
        [HttpPost]
        public async Task<ActionResult> CreateOrderShipment([FromBody] List<UIOrderRequestBodyData> uIOrderRequestBodyDatas)
        {
            string customerID = shipmentService.GetShipmentCustomCodesInformation();
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
                XMLMessage += "<Order orderid=\"" + orderRequest.pkG_NR_TE + "\" custid=\"" + customerID + "\"";
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

                        string xmlAttributeCollectionError = xmlDocument.GetElementsByTagName("ERROR")[0].Attributes[0].InnerText;



                        if (xmlDocumentShipmentResponseParser.Contains("8019"))
                        {
                            createOrderShipmentResponse.FailedToProcessShipments.Add("Customer order number(" + orderRequest.pkG_NR_TE + ") is already confirmed");
                        }
                        else if (xmlDocumentShipmentResponseParser.Contains("8016"))
                        {
                            createOrderShipmentResponse.FailedToProcessShipments.Add("Repeat order numbers ( " + orderRequest.pkG_NR_TE + " )");
                        }
                        else
                        {
                            createOrderShipmentResponse.FailedToProcessShipments.Add("Error Code ( " + xmlAttributeCollectionError + " ) -> " + orderRequest.pkG_NR_TE);
                        }
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

                        shipmentService.UpdateShipmentStatusById(shipmentDataRequest);
                    }

                    createOrderShipmentResponse.Response = true;
                }
                else
                {
                    createOrderShipmentResponse.Response = false;
                }
            }
            //we need to update the workflow status
            int? workflowstatus = shipmentService.SelectShipmentTotalStatusByWorkflowId(_workflowID);
            WorkflowService workflowService = new WorkflowService();
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.ID = _workflowID;
            workflowDataRequest.WFL_STA_TE = workflowstatus;
            workflowService.UpdateWorkflowStatusById(workflowDataRequest);

            iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
            {
                apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                dateTime = System.DateTime.Now,
                LogInformation = new UPS.DataObjects.LogData.LogInformation()
                {
                    LogException = null,
                    LogRequest = JsonConvert.SerializeObject(uIOrderRequestBodyDatas),
                    LogResponse = JsonConvert.SerializeObject(createOrderShipmentResponse)
                }
            });

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
                iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                {
                    apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                    dateTime = System.DateTime.Now,
                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                    {
                        LogException = null,
                        LogRequest = JsonConvert.SerializeObject(sFOrderXMLRequest),
                        LogResponse = JsonConvert.SerializeObject(getSFCancelOrderServiceResponse)
                    }
                });

                return Ok(getSFCancelOrderServiceResponse.OrderResponse);
            }
            else
            {

                iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                {
                    apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                    dateTime = System.DateTime.Now,
                    LogInformation = new UPS.DataObjects.LogData.LogInformation()
                    {
                        LogException = getSFCancelOrderServiceResponse.exception,
                        LogRequest = JsonConvert.SerializeObject(sFOrderXMLRequest),
                        LogResponse = null
                    }
                });

                //AuditEventEntry.WriteEntry(new Exception(getSFCancelOrderServiceResponse.exception.ToString()));
                return Ok(getSFCancelOrderServiceResponse.exception);
            }

        }

        [Route("DeleteShipments")]
        [HttpPost]
        public async Task<ActionResult> DeleteShipments([FromBody] List<ShipmentDataRequest> shipmentDataRequests)
        {
            //ShipmentService shipmentService = new ShipmentService();
            ShipmentDataResponse shipmentDataResponse = shipmentService.DeleteShipments(shipmentDataRequests);
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
                        wfL_ID = _.WFL_ID
                    }).ToList();

                this._quincusAddressTranslationRequest.shipmentWorkFlowRequests = shipmentWorkFlowRequests;
                this._quincusAddressTranslationRequest.token = quincusTokenDataResponse.quincusTokenData.token;

                quincusTranslatedAddressResponse = QuincusService.GetTranslationAddress(this._quincusAddressTranslationRequest);

                if (quincusTranslatedAddressResponse.Response)
                {
                    var getAddressTranslation = quincusTranslatedAddressResponse.ResponseData;

                    List<string> batchIds = new List<string>();

                    quincusTranslatedAddressResponse.ResponseData.ForEach(batches =>
                    {
                        batchIds.Add(batches.batch_id);
                    });

                    var QuincusResponse = QuincusService.GetGeoCodeReponseFromQuincus(new UPS.Quincus.APP.Request.QuincusGeoCodeDataRequest()
                    {
                        endpoint = configuration["Quincus:GeoCodeEndPoint"],
                        batchIDList = batchIds,
                        quincusTokenData = quincusTokenDataResponse.quincusTokenData
                    });

                    if (QuincusResponse.ResponseStatus)
                    {
                        // Insert Address into AddressBook
                        addressBookService.InsertAddress(QuincusResponse.QuincusReponseDataList);
                        QuincusResponse.QuincusReponseDataList.ForEach(datalist =>
                        {
                            List<Geocode> geocodes = (List<Geocode>)((QuincusReponseData)datalist).geocode;
                            List<ShipmentDataRequest> shipmentDataRequestList = new List<ShipmentDataRequest>(geocodes.Count);

                            foreach (Geocode geocode in geocodes)
                            {
                                ShipmentDataRequest shipmentDataRequest =
                                _shipmentDataRequest.FirstOrDefault(_ => _.ID == Convert.ToInt32(geocode.id));
                                shipmentDataRequest.SHP_ADR_TR_TE = geocode.translated_adddress;
                                shipmentDataRequest.ACY_TE = geocode.accuracy;
                                shipmentDataRequest.CON_NR = geocode.confidence;

                                if (
                                            !string.IsNullOrEmpty(geocode.translated_adddress)
                                   //&& geocode.translated_adddress != "NA"
                                   //&& !string.Equals(_shipmentDataRequest.Where(s => s.ID == Convert.ToInt32(geocode.id)).FirstOrDefault().RCV_ADR_TE.Trim(),
                                   //    geocode.translated_adddress.Trim())
                                   )
                                {
                                    shipmentDataRequest.SMT_STA_NR = ((int)Enums.ATStatus.Translated);
                                }
                                else
                                {
                                    shipmentDataRequest.SMT_STA_NR = Convert.ToInt32(_shipmentDataRequest.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault().SMT_STA_NR);
                                }

                                shipmentDataRequestList.Add(shipmentDataRequest);
                            }
                            shipmentService.UpdateShipmentAddressByIds(shipmentDataRequestList);

                        //we need to update the workflow status
                        int? workflowstatus = shipmentService.SelectShipmentTotalStatusByWorkflowId(_workflowID);
                            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                            workflowDataRequest.ID = _workflowID;
                            workflowDataRequest.WFL_STA_TE = workflowstatus;
                            workflowService.UpdateWorkflowStatusById(workflowDataRequest);
                        });

                        iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                        {
                            apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                            dateTime = System.DateTime.Now,
                            LogInformation = new UPS.DataObjects.LogData.LogInformation()
                            {
                                LogException = null,
                                LogRequest = JsonConvert.SerializeObject(QuincusResponse.QuincusReponseDataList),
                                LogResponse = null
                            }
                        });

                        return Ok(QuincusResponse.QuincusReponseDataList);
                    }
                    else
                    {
                        iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                        {
                            apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                            dateTime = System.DateTime.Now,
                            LogInformation = new UPS.DataObjects.LogData.LogInformation()
                            {
                                LogException = QuincusResponse.Exception,
                                LogRequest = JsonConvert.SerializeObject(_shipmentDataRequest),
                                LogResponse = null
                            }
                        });
                        return Ok(QuincusResponse.Exception);
                    }

                }
                else
                {
                    iCustomLog.AddLogEntry(new UPS.DataObjects.LogData.LogDataModel()
                    {
                        apiTypes = UPS.DataObjects.LogData.APITypes.SFExpress,
                        dateTime = System.DateTime.Now,
                        LogInformation = new UPS.DataObjects.LogData.LogInformation()
                        {
                            LogException = quincusTranslatedAddressResponse.exception,
                            LogRequest = JsonConvert.SerializeObject(_shipmentDataRequest),
                            LogResponse = null
                        }
                    });
                    return Ok(quincusTranslatedAddressResponse.exception);
                }

            }
            else
            {
                return Ok(quincusTokenDataResponse.exception);
            }
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
        public ShipmentDataResponse GetMatchedShipmentsWithShipperCompanies(int wid)
        {
            ShipperCompnayService shipperCompanyService = new ShipperCompnayService();
            shipmentDataResponse = shipperCompanyService.SelectMatchedShipmentsWithShipperCompanies(wid);
            if (!shipmentDataResponse.Success)
            {
                //AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            //else
            //{
            //    var json = JsonConvert.SerializeObject(shipmentDataResponse.Shipments).ToString();
            //    AuditEventEntry.WriteEntry(new Exception(json));
            //}
            return shipmentDataResponse;
        }

        [Route("GetCompletedShipments")]
        [HttpGet]
        public ShipmentDataResponse GetCompletedShipments(int wid)
        {
            ShipperCompnayService shipperCompanyService = new ShipperCompnayService();
            shipmentDataResponse = shipperCompanyService.SelectCompletedShipments(wid);
            if (!shipmentDataResponse.Success)
            {
                //AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            return shipmentDataResponse;
        }
    }
}

