﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AtService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UPS.ServicesDataRepository;
using UPS.DataObjects.Shipment;
using Microsoft.EntityFrameworkCore;
using UPS.ServicesDataRepository.DataContext;
using UPS.Quincus.APP;
using UPS.Quincus.APP.Response;
using Microsoft.Extensions.Configuration;
using ExcelFileRead;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using UPS.AddressTranslationService.Controllers;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.WR_FLW;
using UPS.Quincus.APP.Request;
using UPS.DataObjects.SPC_LST;
using UPS.ServicesDataRepository.Common;
using System.Xml;
using UPS.Application.CustomLogs;
using UPS.Quincus.APP.ProxyConnections;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAtUIOrigin")]
    public class ShipmentController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment hostingEnvironment;
        private ShipmentDataResponse shipmentDataResponse;

        private ShipmentService shipmentService { get; set; }
        private WorkflowService workflowService { get; set; }

        private IQuincusAddressTranslationRequest _quincusAddressTranslationRequest { get; set; }

        public ShipmentController(IConfiguration Configuration, IHostingEnvironment HostingEnvironment, IQuincusAddressTranslationRequest QuincusAddressTranslationRequest)
        {
            this.configuration = Configuration;
            this.hostingEnvironment = HostingEnvironment;
            shipmentService = new ShipmentService();
            workflowService = new WorkflowService();
            _quincusAddressTranslationRequest = QuincusAddressTranslationRequest;

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
                                result = this.CreateShipments(excelDataObject2, _workflowID);
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
                                return Ok(excelExtensionReponse);
                            }
                        }
                    }
                }

                return Ok(shipmentDataResponse);
            }
            catch (Exception ex)
            {
                AuditEventEntry.WriteEntry(new Exception(ex.Message));
                return Ok(shipmentDataResponse.OperationExceptionMsg = ex.Message);
            }
        }


        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost]
        public ShipmentDataResponse CreateShipments(List<ExcelDataObject> excelDataObjects, int workflowID)
        {
            //int i = 0;
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                List<ShipmentDataRequest> shipmentData = new List<ShipmentDataRequest>();
                foreach (ExcelDataObject excelDataObject in excelDataObjects)
                {
                    bool allPropertiesNull = !excelDataObject.GetType().GetProperties().Any(prop => prop == null);
                    if (allPropertiesNull)
                    {

                        ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();

                        if (!string.IsNullOrWhiteSpace(excelDataObject.S_shipmentno))
                        {

                            shipmentDataRequest.BIL_TYP_TE = excelDataObject.S_billtype;
                            shipmentDataRequest.CCY_VAL_TE = string.Empty;
                            shipmentDataRequest.COD_TE = string.Empty;
                            shipmentDataRequest.CSG_CTC_TE = excelDataObject.S_cneectc;

                            decimal decimalvalue = 0;
                            shipmentDataRequest.DIM_WGT_DE = 0;
                            if (!string.IsNullOrEmpty(excelDataObject.S_dimwei))
                            {
                                if (decimal.TryParse(excelDataObject.S_dimwei, out decimalvalue))
                                {
                                    shipmentDataRequest.DIM_WGT_DE = decimalvalue;
                                }
                            }

                            //shipmentDataRequest.DIM_WGT_DE = null; //Convert.ToDecimal(excelDataObject.S_dimwei);
                            shipmentDataRequest.DST_CTY_TE = excelDataObject.S_dstcity;
                            shipmentDataRequest.DST_PSL_TE = excelDataObject.S_dstpsl;
                            shipmentDataRequest.EXP_SLC_CD = excelDataObject.S_expslic;
                            shipmentDataRequest.EXP_TYP = "顺丰即日";//excelDataObject.S_expslic;
                            shipmentDataRequest.IMP_NR = excelDataObject.S_impr;
                            shipmentDataRequest.IMP_SLC_TE = excelDataObject.S_impslic;
                            shipmentDataRequest.IN_FLG_TE = excelDataObject.S_inflight;
                            shipmentDataRequest.ORG_CTY_TE = excelDataObject.S_orgcity;

                            string pststring = Convert.ToString(excelDataObject.S_orgpsl);
                            if (InputValidations.IsDecimalFormat(pststring))
                            {
                                shipmentDataRequest.ORG_PSL_CD = Decimal.ToInt32(Decimal.Parse(pststring)).ToString();
                            }
                            else
                            {
                                shipmentDataRequest.ORG_PSL_CD = pststring;
                            }

                            // OU_FLG_TE = Convert.ToString(excelDataObject.S_outflight),

                            int intvalue = 0;
                            shipmentDataRequest.PCS_QTY_NR = 0;
                            if (!string.IsNullOrEmpty(excelDataObject.pcs))
                            {
                                if (int.TryParse(excelDataObject.pcs, out intvalue))
                                {
                                    shipmentDataRequest.PCS_QTY_NR = intvalue;
                                }
                            }

                            //shipmentDataRequest.PCS_QTY_NR = null;//Convert.ToInt32(Convert.ToDouble(excelDataObject.pcs));
                            shipmentDataRequest.PH_NR = excelDataObject.S_ph;
                            shipmentDataRequest.PKG_NR_TE = excelDataObject.S_packageno;
                            shipmentDataRequest.PKG_WGT_DE = Convert.ToDecimal(excelDataObject.S_pkgwei);
                            shipmentDataRequest.PK_UP_TM = null;//Convert.ToString(excelDataObject.S_pkuptime),
                            shipmentDataRequest.PYM_MTD = "寄付月结";//excelDataObject.pymt;
                            shipmentDataRequest.RCV_ADR_TE = excelDataObject.S_address1;
                            shipmentDataRequest.RCV_CPY_TE = excelDataObject.S_receivercompany;
                            shipmentDataRequest.SHP_ADR_TE = excelDataObject.address;
                            shipmentDataRequest.SHP_ADR_TR_TE = string.Empty;
                            shipmentDataRequest.SHP_CPY_NA = excelDataObject.S_shippercompany;
                            shipmentDataRequest.SHP_CTC_TE = excelDataObject.S_shptctc;

                            DateTime dDate;
                            int intdate;
                            shipmentDataRequest.SHP_DT = null;
                            if (!string.IsNullOrEmpty(excelDataObject.S_shipdate))
                            {
                                if (int.TryParse(excelDataObject.S_shipdate, out intdate))
                                {
                                    shipmentDataRequest.SHP_DT = null;
                                }
                                else if (DateTime.TryParse(excelDataObject.S_shipdate, out dDate))
                                {
                                    shipmentDataRequest.SHP_DT = Convert.ToDateTime(excelDataObject.S_shipdate);
                                }
                            }
                            //if(!string.IsNullOrEmpty(excelDataObject.S_shipdate))
                            //{
                            //    if (DateTime.TryParse(excelDataObject.S_shipdate, out dDate))
                            //    {
                            //        shipmentDataRequest.SHP_DT = Convert.ToDateTime(excelDataObject.S_shipdate);
                            //    }
                            //}
                            shipmentDataRequest.SHP_DT = null; //Convert.ToDateTime(excelDataObject.S_shipdate);
                            shipmentDataRequest.SHP_NR = excelDataObject.S_shpr;
                            shipmentDataRequest.SHP_PH_TE = excelDataObject.S_shptph;
                            shipmentDataRequest.SMT_NR_TE = excelDataObject.S_shipmentno;
                            shipmentDataRequest.SMT_STA_NR = 0;
                            shipmentDataRequest.SMT_STA_TE = "Uploaded";
                            shipmentDataRequest.SMT_VAL_DE = 0;
                            shipmentDataRequest.SMT_WGT_DE = Convert.ToDecimal(excelDataObject.S_shptwei);
                            shipmentDataRequest.SVL_NR = Convert.ToString(excelDataObject.svl);
                            shipmentDataRequest.WGT_UNT_TE = excelDataObject.S_weiunit;
                            shipmentDataRequest.WFL_ID = workflowID;
                            shipmentDataRequest.SF_TRA_LG_ID = null;
                            shipmentDataRequest.QQS_TRA_LG_ID = null;
                            shipmentDataRequest.FST_INV_LN_DES_TE = excelDataObject.S_1stinvoicelinedesc;
                            shipmentDataRequest.POD_RTN_SVC = "0";

                            shipmentData.Add(shipmentDataRequest);
                        }
                    }

                }
                shipmentService = new ShipmentService();
                shipmentDataResponse = shipmentService.CreateShipments(shipmentData);
                shipmentDataResponse.Success = true;
                return shipmentDataResponse;
            }
            catch (Exception exception)
            {
                shipmentDataResponse.OperationExceptionMsg = exception.Message;
                shipmentDataResponse.Success = true;
                AuditEventEntry.WriteEntry(new Exception(exception.Message));
            }
            return shipmentDataResponse;
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("UpdateShipmentStatusById")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateShipmentStatusById([FromBody] ShipmentDataRequest shipmentDataRequest)
        {
            shipmentService = new ShipmentService();
            ShipmentDataResponse shipmentDataResponse = shipmentService.UpdateShipmentStatusById(shipmentDataRequest);
            if (!shipmentDataResponse.Success)
            {
                AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            return Ok(shipmentDataResponse);
        }

        [Route("UpdateShipmentAddressById")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateShipmentAddressById([FromBody] ShipmentDataRequest shipmentDataRequest)
        {
            shipmentService = new ShipmentService();
            ShipmentDataResponse shipmentDataResponse = shipmentService.UpdateShipmentAddressById(shipmentDataRequest);

            //we need to update the workflow status
            int? workflowstatus = shipmentService.SelectShipmentTotalStatusByWorkflowId(shipmentDataRequest.WFL_ID);
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.ID = shipmentDataRequest.WFL_ID;
            workflowDataRequest.WFL_STA_TE = workflowstatus;
            workflowService.UpdateWorkflowStatusById(workflowDataRequest);

            return Ok(shipmentDataResponse);
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [Route("GetShipmentData")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public List<ShipmentDataRequest> GetShipmentData(int wid)
        {
            shipmentService = new ShipmentService();
            List<ShipmentDataRequest> shipmentDataRequests = shipmentService.GetShipment(wid);
            return shipmentDataRequests;
        }

        [Route("CreateOrderShipment")]
        [HttpPost]
        public async Task<ActionResult> CreateOrderShipment([FromBody] List<UIOrderRequestBodyData> uIOrderRequestBodyDatas)
        {
            _workflowID = uIOrderRequestBodyDatas[0].wfL_ID;
            CreateOrderShipmentResponse createOrderShipmentResponse = new CreateOrderShipmentResponse();
            createOrderShipmentResponse.FailedToProcessShipments = new List<string>();
            createOrderShipmentResponse.ProcessedShipments = new List<string>();
            ShipmentService shipmentService = new ShipmentService();

            //List<UIOrderRequestBodyData> uIOrderRequestBodyDatas = new List<UIOrderRequestBodyData>();

            foreach (var orderRequest in uIOrderRequestBodyDatas)
            {
                string XMLMessage = string.Empty;

                XMLMessage = "<Request lang=\"zh-CN\" service=\"OrderService\">";
                XMLMessage += "<Head>LJ_T6NVV</Head>";
                XMLMessage += "<Body>";
                XMLMessage += "<Order orderid=\"" + orderRequest.pkG_NR_TE + "\" custid=\"" + 7551234567 + "\"";
                XMLMessage += " j_tel=\"" + orderRequest.shP_CTC_TE + "\"";
                XMLMessage += " j_address=\"" + orderRequest.shP_ADR_TE + "\"";
                XMLMessage += " d_tel=\"" + orderRequest.pH_NR + "\"";
                XMLMessage += " d_address=\"" + orderRequest.shP_ADR_TR_TE + "\" cargo_total_weight=\"" + orderRequest.pkG_WGT_DE + "\"";
                XMLMessage += " pay_method=\"1\" is_docall=\"" + 1 + "\" need_return_tracking_no=\"" + orderRequest.poD_RTN_SVC + "\" express_type=\"6\"";
                XMLMessage += " >";
                XMLMessage += " </Order></Body></Request>";


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
                            createOrderShipmentResponse.FailedToProcessShipments.Add(orderRequest.pkG_NR_TE);
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
                    if (getSFCreateOrderServiceResponse.exception != null)
                        AuditEventEntry.WriteEntry(new Exception(getSFCreateOrderServiceResponse.exception.ToString()));
                }
            }
            //we need to update the workflow status
            int? workflowstatus = shipmentService.SelectShipmentTotalStatusByWorkflowId(_workflowID);
            WorkflowService workflowService = new WorkflowService();
            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
            workflowDataRequest.ID = _workflowID;
            workflowDataRequest.WFL_STA_TE = workflowstatus;
            workflowService.UpdateWorkflowStatusById(workflowDataRequest);
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
                return Ok(getSFCancelOrderServiceResponse.OrderResponse);
            }
            else
            {
                AuditEventEntry.WriteEntry(new Exception(getSFCancelOrderServiceResponse.exception.ToString()));
                return Ok(getSFCancelOrderServiceResponse.exception);
            }

        }

        [Route("DeleteShipments")]
        [HttpPost]
        public async Task<ActionResult> DeleteShipments([FromBody] List<ShipmentDataRequest> shipmentDataRequests)
        {
            ShipmentService shipmentService = new ShipmentService();
            ShipmentDataResponse shipmentDataResponse = shipmentService.DeleteShipments(shipmentDataRequests);
            return Ok(shipmentDataResponse);
        }

        [Route("GetTranslationAddress")]
        [HttpPost]
        public async Task<ActionResult> GetTranslationAddress([FromBody] List<ShipmentWorkFlowRequest> shipmentWorkFlowRequest)
        {

            int wid = 0;
            decimal requestCount = 0m;
            if (shipmentWorkFlowRequest.Any())
            {
                wid = shipmentWorkFlowRequest.FirstOrDefault().wfL_ID;
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
                //quincusTranslatedAddressResponse = QuincusService.GetTranslationAddress(new UPS.Quincus.APP.Request.QuincusAddressTranslationRequest()
                //{
                //    endpoint = configuration["Quincus:GeoCodeEndPoint"],
                //    shipmentWorkFlowRequests = shipmentWorkFlowRequest,
                //    token = quincusTokenDataResponse.quincusTokenData.token
                //});
                this._quincusAddressTranslationRequest.shipmentWorkFlowRequests = shipmentWorkFlowRequest;
                this._quincusAddressTranslationRequest.token = quincusTokenDataResponse.quincusTokenData.token;

                List<List<ShipmentWorkFlowRequest>> shipmentWorkFlowRequests = new List<List<ShipmentWorkFlowRequest>>();
                shipmentWorkFlowRequests = QuincusProxy.ChunkBy(shipmentWorkFlowRequest, 20);

                foreach(List<ShipmentWorkFlowRequest> requests in shipmentWorkFlowRequests)
                {

                    requestCount = requests.Count();

                    quincusTranslatedAddressResponse = QuincusService.GetTranslationAddress(this._quincusAddressTranslationRequest);

                    if (quincusTranslatedAddressResponse.Response)
                    {
                        //return Ok(quincusTranslatedAddressResponse.ResponseData);

                        var getAddressTranslation = quincusTranslatedAddressResponse.ResponseData;

                        var QuincusResponse = QuincusService.GetGeoCodeReponseFromQuincus(new UPS.Quincus.APP.Request.QuincusGeoCodeDataRequest()
                        {
                            endpoint = configuration["Quincus:GeoCodeEndPoint"],
                            id = quincusTranslatedAddressResponse.ResponseData.batch_id,
                            quincusTokenData = quincusTokenDataResponse.quincusTokenData
                        }, requestCount);

                        if (QuincusResponse.ResponseStatus)
                        {
                            ShipmentDataRequest shipment = new ShipmentDataRequest();
                            List<Geocode> geocodes = (List<Geocode>)((QuincusReponseData)QuincusResponse.QuincusReponseData).geocode;
                            List<ShipmentDataRequest> shipmentsDataRequest = new List<ShipmentDataRequest>(geocodes.Count);
                            for (int i = 0; i < geocodes.Count; i++)
                            {
                                ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                                shipmentDataRequest.ID = Convert.ToInt32(geocodes[i].id);
                                shipmentDataRequest.WFL_ID = wid;
                                shipmentDataRequest.SHP_ADR_TR_TE = geocodes[i].translated_adddress;
                                shipmentDataRequest.ACY_TE = geocodes[i].accuracy;
                                shipmentDataRequest.CON_NR = geocodes[i].confidence;

                                if (
                                        !string.IsNullOrEmpty(geocodes[i].translated_adddress)
                                        //&&  geocodes[i].translated_adddress != "NA"
                                        //&& !string.Equals(shipmentWorkFlowRequest.Where(s => s.id == shipmentDataRequest.ID).FirstOrDefault().rcV_ADR_TE.Trim(),
                                        //    geocodes[i].translated_adddress.Trim())
                                   )
                                {
                                    shipmentDataRequest.SMT_STA_NR = ((int)Enums.ATStatus.Translated);
                                    shipmentDataRequest.SMT_STA_TE = "Translated";
                                }
                                else
                                {
                                    shipmentDataRequest.SMT_STA_NR = Convert.ToInt32(shipmentWorkFlowRequest.Where(s => s.id == shipmentDataRequest.ID).FirstOrDefault().smT_STA_NR);
                                    shipmentDataRequest.SMT_STA_TE = Convert.ToString(shipmentWorkFlowRequest.Where(s => s.id == shipmentDataRequest.ID).FirstOrDefault().smT_STA_TE);
                                }
                                shipmentsDataRequest.Add(shipmentDataRequest);
                            }
                            ShipmentService shipmentService = new ShipmentService();
                            shipmentService.UpdateShipmentAddressByIds(shipmentsDataRequest);
                            _workflowID = shipmentsDataRequest.FirstOrDefault().WFL_ID;
                            //we need to update the workflow status
                            int? workflowstatus = shipmentService.SelectShipmentTotalStatusByWorkflowId(_workflowID);
                            WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                            workflowDataRequest.ID = _workflowID;
                            workflowDataRequest.WFL_STA_TE = workflowstatus;
                            workflowService.UpdateWorkflowStatusById(workflowDataRequest);

                        }
                        else
                        {
                            if (QuincusResponse.Exception == null)
                            {
                                AuditEventEntry.WriteEntry(new Exception("Translation failed..."));
                            }
                            else
                            {
                                AuditEventEntry.WriteEntry(new Exception(QuincusResponse.Exception.ToString()));
                            }
                            return Ok(QuincusResponse?.Exception);
                        }
                    }
                    else
                    {
                        if (quincusTranslatedAddressResponse.exception == null)
                        {
                            AuditEventEntry.WriteEntry(new Exception("Translation failed..."));
                        }
                        else
                        {
                            AuditEventEntry.WriteEntry(new Exception(quincusTranslatedAddressResponse.exception.ToString()));
                        }
                        return Ok(quincusTranslatedAddressResponse?.exception);
                    }
                }

                return Ok(quincusTranslatedAddressResponse.ResponseData);
            }
            else
            {
                if (quincusTokenDataResponse.exception == null)
                {
                    AuditEventEntry.WriteEntry(new Exception("Translation failed..."));
                }
                else
                {
                    AuditEventEntry.WriteEntry(new Exception(quincusTokenDataResponse.exception.ToString()));
                }
                return Ok(quincusTokenDataResponse?.exception);
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
                    id = shipmentGeoCodes.geoCode,
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
                AuditEventEntry.WriteEntry(new Exception(quincusTokenDataResponse.exception.ToString()));
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
                AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
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
                AuditEventEntry.WriteEntry(new Exception(shipmentDataResponse.OperationExceptionMsg));
            }
            return shipmentDataResponse;
        }
    }
}
