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
using ExcelFileRead;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using UPS.AddressTranslationService.Controllers;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private int _workflowID = 0;
        [Route("ExcelFileUpload")]
        [HttpPost]
        public async Task<ActionResult> ExcelFile(IList<IFormFile> excelFileName, int Emp_Id=1)
        {
            ActionResult result = null;
            //string response = string.Empty;
            if (excelFileName != null)
            {

                //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                foreach (var file in excelFileName)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(@"D:\UserExcels", file.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {

                            //FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                            //response = new ExcelExtension().Test(filePath);
                            await file.CopyToAsync(fileStream);
                        }

                        string JSONString = new ExcelExtension().Test(filePath);
                        var excelDataObject2 = JsonConvert.DeserializeObject<List<ExcelDataObject>>(JSONString);
                        WorkflowController workflowController = new WorkflowController();
                        _workflowID = workflowController.Post(file, Emp_Id);
                        result = this.Post(excelDataObject2, _workflowID);
                    }
                }

                //return Ok(excelFileName.FileName);
            }

            return Ok(result);
        }

        private ShipmentService shipmentService { get; set; }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult Post(List<ExcelDataObject> excelDataObjects, int workflowID)
        {
            List<ShipmentDataRequest> shipmentdata = null;
            try
            {
                List<ShipmentDataRequest> shipmentData = new List<ShipmentDataRequest>();
                foreach (ExcelDataObject excelDataObject in excelDataObjects)
                {
                    ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest()
                    {
                        BIL_TYP_TE = excelDataObject.S_billtype,
                        CCY_VAL_TE = string.Empty,
                        COD_TE = string.Empty,
                        CSG_CTC_TE = excelDataObject.S_cneectc,
                        DIM_WGT_DE = Convert.ToDecimal(excelDataObject.S_dimwei),
                        DST_CTY_TE = excelDataObject.S_dstcity,
                        DST_PSL_TE = excelDataObject.S_dstpsl,
                        EXP_SLC_CD = excelDataObject.S_expslic,
                        IMP_NR = excelDataObject.S_impr,
                        IMP_SLC_TE = excelDataObject.S_impslic,
                        IN_FLG_TE = excelDataObject.S_inflight,
                        ORG_CTY_TE = excelDataObject.S_orgcity,
                        ORG_PSL_CD = Convert.ToString(excelDataObject.S_orgpsl),
                        OU_FLG_TE = Convert.ToString(excelDataObject.S_outflight),
                        PCS_QTY_NR = Convert.ToInt32(excelDataObject.pcs),
                        PH_NR = excelDataObject.S_ph,
                        PKG_NR_TE = excelDataObject.S_packageno,
                        PKG_WGT_DE = Convert.ToDecimal(excelDataObject.S_pkgwei),
                        PK_UP_TM = null,//Convert.ToString(excelDataObject.S_pkuptime),
                        PYM_MTD = excelDataObject.pymt,
                        RCV_ADR_TE = excelDataObject.address,
                        RCV_CPY_TE = excelDataObject.S_receivercompany,
                        SHP_ADR_TE = excelDataObject.S_address1,
                        SHP_ADR_TR_TE = string.Empty,
                        SHP_CPY_NA = excelDataObject.S_shippercompany,
                        SHP_CTC_TE = excelDataObject.S_shptctc,
                        SHP_DT = null,//Convert.ToString(excelDataObject.S_shipdate),
                        SHP_NR = excelDataObject.S_shpr,
                        SHP_PH_TE = excelDataObject.S_shptph,
                        SMT_NR_TE = excelDataObject.S_shipmentno,
                        SMT_STA_NR = 0,
                        SMT_VAL_DE = 0,
                        SMT_WGT_DE = Convert.ToDecimal(excelDataObject.S_shptwei),
                        SVL_NR = Convert.ToString(excelDataObject.svl),
                        WGT_UNT_TE = excelDataObject.S_weiunit,
                        WFL_ID = workflowID,
                        SF_TRA_LG_ID = null,
                        QQS_TRA_LG_ID = null,

                    };
                    shipmentData.Add(shipmentDataRequest);
                }
                shipmentService = new ShipmentService();
                bool status = shipmentService.CreateShipments(shipmentData);
                //shipmentService.CreateShipment(new ShipmentDataRequest()
                //{
                //    SHP_ADR_TE = "test1",
                //    WFL_ID = 1,
                //    SF_TRA_LG_ID = null,
                //    QQS_TRA_LG_ID = null
                //});
                shipmentdata = this.GetShipmentData(workflowID);
            }
            catch(Exception ex)
            {

            }
            return Ok(shipmentdata);
        }

        //private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public List<ShipmentDataRequest> GetShipmentData(int wid)
        {
            shipmentService = new ShipmentService();
            List<ShipmentDataRequest> shipmentDataRequests = shipmentService.GetShipment(wid);
            return shipmentDataRequests;
        }
    }
}
