namespace UPS.ServicesDataRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EFCore.BulkExtensions;
    using ExcelFileRead;
    using Microsoft.EntityFrameworkCore;
    using UPS.DataObjects.AddressBook;
    using UPS.DataObjects.CST_DTL;
    using UPS.DataObjects.Shipment;
    using UPS.DataObjects.WR_FLW;
    using UPS.ServicesAsyncActions;
    using UPS.ServicesDataRepository.Common;
    using UPS.ServicesDataRepository.DataContext;

    public class ShipmentService : IShipmentAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        private readonly ApplicationDbContext context;
        private IAddressBookService addressBookService;
        private IEntityValidationService entityValidationService;

        public ShipmentService(ApplicationDbContext applicationDbContext,
            IAddressBookService addressBookService,
            IEntityValidationService entityValidationService)
        {
            this.context = applicationDbContext;
            this.addressBookService = addressBookService;
            this.entityValidationService = entityValidationService;
            this.optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            this.optionsBuilder.EnableSensitiveDataLogging(true);
        }

        public CST_DTL GetShipmentCustomCodesInformation()
        {
            return this.context.CST_DTL.FirstOrDefault();
        }

        public List<ShipmentDataRequest> GetShipment(int workflowID)
        {

            List<ShipmentDataRequest> shipmentDataRequests = null;
            try
            {
                shipmentDataRequests = new List<ShipmentDataRequest>();
                var anonymousList =
                    (
                        from s in this.context.shipmentDataRequests
                        where s.WFL_ID == workflowID
                        && s.SMT_STA_NR != (int)Enums.ATStatus.Completed
                        && s.SMT_STA_NR != (int)Enums.ATStatus.Inactive
                        orderby s.ID
                        select new
                        {
                            s.ID,
                            s.WFL_ID,
                            s.ACY_TE,
                            s.BIL_TYP_TE,
                            s.CCY_VAL_TE,
                            s.COD_TE,
                            s.CON_NR,
                            s.CSG_CTC_TE,
                            s.DIM_WGT_DE,
                            s.DST_CTY_TE,
                            s.DST_PSL_TE,
                            s.EXP_SLC_CD,
                            s.EXP_TYP,
                            s.FST_INV_LN_DES_TE,
                            s.IMP_NR,
                            s.IMP_SLC_TE,
                            s.IN_FLG_TE,
                            s.ORG_CTY_TE,
                            s.ORG_PSL_CD,
                            s.OU_FLG_TE,
                            s.PCS_QTY_NR,
                            s.PH_NR,
                            s.PKG_NR_TE,
                            s.PKG_WGT_DE,
                            s.PK_UP_TM,
                            s.PYM_MTD,
                            s.PY_MT_TE,
                            s.QQS_TRA_LG_ID,
                            s.RCV_ADR_TE,
                            s.RCV_CPY_TE,
                            s.SF_TRA_LG_ID,
                            s.SHP_ADR_TE,
                            s.SHP_ADR_TR_TE,
                            s.SHP_CPY_NA,
                            s.SHP_CTC_TE,
                            s.SHP_DT,
                            s.SHP_NR,
                            s.SHP_PH_TE,
                            s.SMT_NR_TE,
                            s.SMT_STA_NR,
                            s.SMT_VAL_DE,
                            s.SMT_WGT_DE,
                            s.SPC_SLIC_NR,
                            s.SVL_NR,
                            s.WGT_UNT_TE,
                            s.POD_RTN_SVC,
                            s.TranslationScore
                        }).ToList();

                foreach (var shipmentData in anonymousList)
                {
                    ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                    shipmentDataRequest.ID = shipmentData.ID;
                    shipmentDataRequest.BIL_TYP_TE = shipmentData.BIL_TYP_TE;
                    shipmentDataRequest.CCY_VAL_TE = shipmentData.CCY_VAL_TE;
                    shipmentDataRequest.COD_TE = shipmentData.COD_TE;
                    shipmentDataRequest.CSG_CTC_TE = shipmentData.CSG_CTC_TE;
                    shipmentDataRequest.DIM_WGT_DE = shipmentData.DIM_WGT_DE;
                    shipmentDataRequest.DST_CTY_TE = shipmentData.DST_CTY_TE;
                    shipmentDataRequest.DST_PSL_TE = shipmentData.DST_PSL_TE;
                    shipmentDataRequest.EXP_SLC_CD = shipmentData.EXP_SLC_CD;
                    shipmentDataRequest.EXP_TYP = shipmentData.EXP_TYP;
                    shipmentDataRequest.FST_INV_LN_DES_TE = shipmentData.FST_INV_LN_DES_TE;
                    shipmentDataRequest.IMP_NR = shipmentData.IMP_NR;
                    shipmentDataRequest.IMP_SLC_TE = shipmentData.IMP_SLC_TE;
                    shipmentDataRequest.IN_FLG_TE = shipmentData.IN_FLG_TE;
                    shipmentDataRequest.ORG_CTY_TE = shipmentData.ORG_CTY_TE;
                    shipmentDataRequest.ORG_PSL_CD = shipmentData.ORG_PSL_CD;
                    shipmentDataRequest.OU_FLG_TE = shipmentData.OU_FLG_TE;
                    shipmentDataRequest.PCS_QTY_NR = shipmentData.PCS_QTY_NR;
                    shipmentDataRequest.PH_NR = shipmentData.PH_NR;
                    shipmentDataRequest.PKG_NR_TE = shipmentData.PKG_NR_TE;
                    shipmentDataRequest.PKG_WGT_DE = shipmentData.PKG_WGT_DE;
                    shipmentDataRequest.PK_UP_TM = shipmentData.PK_UP_TM;
                    shipmentDataRequest.PYM_MTD = shipmentData.PYM_MTD;
                    shipmentDataRequest.PY_MT_TE = shipmentData.PY_MT_TE;
                    shipmentDataRequest.QQS_TRA_LG_ID = shipmentData.QQS_TRA_LG_ID;
                    shipmentDataRequest.RCV_ADR_TE = shipmentData.RCV_ADR_TE;
                    shipmentDataRequest.RCV_CPY_TE = shipmentData.RCV_CPY_TE;
                    shipmentDataRequest.SF_TRA_LG_ID = shipmentData.SF_TRA_LG_ID;
                    shipmentDataRequest.SHP_ADR_TE = shipmentData.SHP_ADR_TE;
                    shipmentDataRequest.SHP_ADR_TR_TE = shipmentData.SHP_ADR_TR_TE;
                    shipmentDataRequest.SHP_CPY_NA = shipmentData.SHP_CPY_NA;
                    shipmentDataRequest.SHP_CTC_TE = shipmentData.SHP_CTC_TE;
                    shipmentDataRequest.SHP_DT = shipmentData.SHP_DT;
                    shipmentDataRequest.SHP_NR = shipmentData.SHP_NR;
                    shipmentDataRequest.SHP_PH_TE = shipmentData.SHP_PH_TE;
                    shipmentDataRequest.SMT_NR_TE = shipmentData.SMT_NR_TE;
                    shipmentDataRequest.SMT_STA_NR = shipmentData.SMT_STA_NR;

                    switch (shipmentDataRequest.SMT_STA_NR)
                    {
                        case 0:
                            shipmentDataRequest.SMT_STA_TE = "Uploaded";
                            break;
                        case 1:
                            shipmentDataRequest.SMT_STA_TE = "Curated";
                            break;
                        case 2:
                            shipmentDataRequest.SMT_STA_TE = "Translated";
                            break;
                        case 3:
                            shipmentDataRequest.SMT_STA_TE = "Completed";
                            break;
                        case 4:
                            shipmentDataRequest.SMT_STA_TE = "Inactive";
                            break;
                        default:
                            shipmentDataRequest.SMT_STA_TE = "Uploaded";
                            break;
                    }

                    shipmentDataRequest.SMT_VAL_DE = shipmentData.SMT_VAL_DE;
                    shipmentDataRequest.SMT_WGT_DE = shipmentData.SMT_WGT_DE;
                    shipmentDataRequest.SVL_NR = shipmentData.SVL_NR;
                    shipmentDataRequest.WFL_ID = shipmentData.WFL_ID;
                    shipmentDataRequest.WGT_UNT_TE = shipmentData.WGT_UNT_TE;
                    shipmentDataRequest.ACY_TE = shipmentData.ACY_TE;
                    shipmentDataRequest.CON_NR = shipmentData.CON_NR;
                    shipmentDataRequest.SPC_SLIC_NR = shipmentData.SPC_SLIC_NR;
                    shipmentDataRequest.POD_RTN_SVC = shipmentData.POD_RTN_SVC;
                    shipmentDataRequest.TranslationScore = shipmentData.TranslationScore;

                    shipmentDataRequests.Add(shipmentDataRequest);
                }
            }
            catch
            {

            }

            return shipmentDataRequests;
        }

        public List<ShipmentDataRequest> GetAllShipment(int workflowID)
        {

            List<ShipmentDataRequest> shipmentDataRequests = null;
            try
            {
                shipmentDataRequests = new List<ShipmentDataRequest>();
                var anonymousList =
                    (
                        from s in this.context.shipmentDataRequests
                        where s.WFL_ID == workflowID
                        && s.SMT_STA_NR != (int)Enums.ATStatus.Inactive
                        orderby s.ID
                        select new
                        {
                            s.ID,
                            s.WFL_ID,
                            s.ACY_TE,
                            s.BIL_TYP_TE,
                            s.CCY_VAL_TE,
                            s.COD_TE,
                            s.CON_NR,
                            s.CSG_CTC_TE,
                            s.DIM_WGT_DE,
                            s.DST_CTY_TE,
                            s.DST_PSL_TE,
                            s.EXP_SLC_CD,
                            s.EXP_TYP,
                            s.FST_INV_LN_DES_TE,
                            s.IMP_NR,
                            s.IMP_SLC_TE,
                            s.IN_FLG_TE,
                            s.ORG_CTY_TE,
                            s.ORG_PSL_CD,
                            s.OU_FLG_TE,
                            s.PCS_QTY_NR,
                            s.PH_NR,
                            s.PKG_NR_TE,
                            s.PKG_WGT_DE,
                            s.PK_UP_TM,
                            s.PYM_MTD,
                            s.PY_MT_TE,
                            s.QQS_TRA_LG_ID,
                            s.RCV_ADR_TE,
                            s.RCV_CPY_TE,
                            s.SF_TRA_LG_ID,
                            s.SHP_ADR_TE,
                            s.SHP_ADR_TR_TE,
                            s.SHP_CPY_NA,
                            s.SHP_CTC_TE,
                            s.SHP_DT,
                            s.SHP_NR,
                            s.SHP_PH_TE,
                            s.SMT_NR_TE,
                            s.SMT_STA_NR,
                            s.SMT_VAL_DE,
                            s.SMT_WGT_DE,
                            s.SPC_SLIC_NR,
                            s.SVL_NR,
                            s.WGT_UNT_TE,
                            s.POD_RTN_SVC,
                            s.TranslationScore
                        }).ToList();

                foreach (var shipmentData in anonymousList)
                {
                    ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                    shipmentDataRequest.ID = shipmentData.ID;
                    shipmentDataRequest.BIL_TYP_TE = shipmentData.BIL_TYP_TE;
                    shipmentDataRequest.CCY_VAL_TE = shipmentData.CCY_VAL_TE;
                    shipmentDataRequest.COD_TE = shipmentData.COD_TE;
                    shipmentDataRequest.CSG_CTC_TE = shipmentData.CSG_CTC_TE;
                    shipmentDataRequest.DIM_WGT_DE = shipmentData.DIM_WGT_DE;
                    shipmentDataRequest.DST_CTY_TE = shipmentData.DST_CTY_TE;
                    shipmentDataRequest.DST_PSL_TE = shipmentData.DST_PSL_TE;
                    shipmentDataRequest.EXP_SLC_CD = shipmentData.EXP_SLC_CD;
                    shipmentDataRequest.EXP_TYP = shipmentData.EXP_TYP;
                    shipmentDataRequest.FST_INV_LN_DES_TE = shipmentData.FST_INV_LN_DES_TE;
                    shipmentDataRequest.IMP_NR = shipmentData.IMP_NR;
                    shipmentDataRequest.IMP_SLC_TE = shipmentData.IMP_SLC_TE;
                    shipmentDataRequest.IN_FLG_TE = shipmentData.IN_FLG_TE;
                    shipmentDataRequest.ORG_CTY_TE = shipmentData.ORG_CTY_TE;
                    shipmentDataRequest.ORG_PSL_CD = shipmentData.ORG_PSL_CD;
                    shipmentDataRequest.OU_FLG_TE = shipmentData.OU_FLG_TE;
                    shipmentDataRequest.PCS_QTY_NR = shipmentData.PCS_QTY_NR;
                    shipmentDataRequest.PH_NR = shipmentData.PH_NR;
                    shipmentDataRequest.PKG_NR_TE = shipmentData.PKG_NR_TE;
                    shipmentDataRequest.PKG_WGT_DE = shipmentData.PKG_WGT_DE;
                    shipmentDataRequest.PK_UP_TM = shipmentData.PK_UP_TM;
                    shipmentDataRequest.PYM_MTD = shipmentData.PYM_MTD;
                    shipmentDataRequest.PY_MT_TE = shipmentData.PY_MT_TE;
                    shipmentDataRequest.QQS_TRA_LG_ID = shipmentData.QQS_TRA_LG_ID;
                    shipmentDataRequest.RCV_ADR_TE = shipmentData.RCV_ADR_TE;
                    shipmentDataRequest.RCV_CPY_TE = shipmentData.RCV_CPY_TE;
                    shipmentDataRequest.SF_TRA_LG_ID = shipmentData.SF_TRA_LG_ID;
                    shipmentDataRequest.SHP_ADR_TE = shipmentData.SHP_ADR_TE;
                    shipmentDataRequest.SHP_ADR_TR_TE = shipmentData.SHP_ADR_TR_TE;
                    shipmentDataRequest.SHP_CPY_NA = shipmentData.SHP_CPY_NA;
                    shipmentDataRequest.SHP_CTC_TE = shipmentData.SHP_CTC_TE;
                    shipmentDataRequest.SHP_DT = shipmentData.SHP_DT;
                    shipmentDataRequest.SHP_NR = shipmentData.SHP_NR;
                    shipmentDataRequest.SHP_PH_TE = shipmentData.SHP_PH_TE;
                    shipmentDataRequest.SMT_NR_TE = shipmentData.SMT_NR_TE;
                    shipmentDataRequest.SMT_STA_NR = shipmentData.SMT_STA_NR;

                    switch (shipmentDataRequest.SMT_STA_NR)
                    {
                        case 0:
                            shipmentDataRequest.SMT_STA_TE = "Uploaded";
                            break;
                        case 1:
                            shipmentDataRequest.SMT_STA_TE = "Curated";
                            break;
                        case 2:
                            shipmentDataRequest.SMT_STA_TE = "Translated";
                            break;
                        case 3:
                            shipmentDataRequest.SMT_STA_TE = "Completed";
                            break;
                        case 4:
                            shipmentDataRequest.SMT_STA_TE = "Inactive";
                            break;
                        default:
                            shipmentDataRequest.SMT_STA_TE = "Uploaded";
                            break;
                    }

                    shipmentDataRequest.SMT_VAL_DE = shipmentData.SMT_VAL_DE;
                    shipmentDataRequest.SMT_WGT_DE = shipmentData.SMT_WGT_DE;
                    shipmentDataRequest.SVL_NR = shipmentData.SVL_NR;
                    shipmentDataRequest.WFL_ID = shipmentData.WFL_ID;
                    shipmentDataRequest.WGT_UNT_TE = shipmentData.WGT_UNT_TE;
                    shipmentDataRequest.ACY_TE = shipmentData.ACY_TE;
                    shipmentDataRequest.CON_NR = shipmentData.CON_NR;
                    shipmentDataRequest.SPC_SLIC_NR = shipmentData.SPC_SLIC_NR;
                    shipmentDataRequest.POD_RTN_SVC = shipmentData.POD_RTN_SVC;
                    shipmentDataRequest.TranslationScore = shipmentData.TranslationScore;

                    shipmentDataRequests.Add(shipmentDataRequest);
                }
            }
            catch
            {

            }

            return shipmentDataRequests;
        }

        public int CreateShipment(ShipmentDataRequest shipmentData)
        {
            ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
            shipmentDataRequest.BIL_TYP_TE = shipmentData.BIL_TYP_TE;
            shipmentDataRequest.CCY_VAL_TE = shipmentData.CCY_VAL_TE;
            shipmentDataRequest.COD_TE = shipmentData.COD_TE;
            shipmentDataRequest.CSG_CTC_TE = shipmentData.CSG_CTC_TE;
            shipmentDataRequest.DIM_WGT_DE = shipmentData.DIM_WGT_DE;
            shipmentDataRequest.DST_CTY_TE = shipmentData.DST_CTY_TE;
            shipmentDataRequest.DST_PSL_TE = shipmentData.DST_PSL_TE;
            shipmentDataRequest.EXP_SLC_CD = shipmentData.EXP_SLC_CD;
            shipmentDataRequest.EXP_TYP = shipmentData.EXP_TYP;
            shipmentDataRequest.FST_INV_LN_DES_TE = shipmentData.FST_INV_LN_DES_TE;
            shipmentDataRequest.IMP_NR = shipmentData.IMP_NR;
            shipmentDataRequest.IMP_SLC_TE = shipmentData.IMP_SLC_TE;
            shipmentDataRequest.IN_FLG_TE = shipmentData.IN_FLG_TE;
            shipmentDataRequest.ORG_CTY_TE = shipmentData.ORG_CTY_TE;
            shipmentDataRequest.ORG_PSL_CD = shipmentData.ORG_PSL_CD;
            shipmentDataRequest.OU_FLG_TE = shipmentData.OU_FLG_TE;
            shipmentDataRequest.PCS_QTY_NR = shipmentData.PCS_QTY_NR;
            shipmentDataRequest.PH_NR = shipmentData.PH_NR;
            shipmentDataRequest.PKG_NR_TE = shipmentData.PKG_NR_TE;
            shipmentDataRequest.PKG_WGT_DE = shipmentData.PKG_WGT_DE;
            shipmentDataRequest.PK_UP_TM = shipmentData.PK_UP_TM;
            shipmentDataRequest.PYM_MTD = shipmentData.PYM_MTD;
            shipmentDataRequest.PY_MT_TE = shipmentData.PY_MT_TE;
            shipmentDataRequest.QQS_TRA_LG_ID = shipmentData.QQS_TRA_LG_ID;
            shipmentDataRequest.RCV_ADR_TE = shipmentData.RCV_ADR_TE;
            shipmentDataRequest.RCV_CPY_TE = shipmentData.RCV_CPY_TE;
            shipmentDataRequest.SF_TRA_LG_ID = shipmentData.SF_TRA_LG_ID;
            shipmentDataRequest.SHP_ADR_TE = shipmentData.SHP_ADR_TE;
            shipmentDataRequest.SHP_ADR_TR_TE = shipmentData.SHP_ADR_TR_TE;
            shipmentDataRequest.SHP_CPY_NA = shipmentData.SHP_CPY_NA;
            shipmentDataRequest.SHP_CTC_TE = shipmentData.SHP_CTC_TE;
            shipmentDataRequest.SHP_DT = shipmentData.SHP_DT;
            shipmentDataRequest.SHP_NR = shipmentData.SHP_NR;
            shipmentDataRequest.SHP_PH_TE = shipmentData.SHP_PH_TE;
            shipmentDataRequest.SMT_NR_TE = shipmentData.SMT_NR_TE;
            shipmentDataRequest.SMT_STA_NR = shipmentData.SMT_STA_NR;
            shipmentDataRequest.SMT_VAL_DE = shipmentData.SMT_VAL_DE;
            shipmentDataRequest.SMT_WGT_DE = shipmentData.SMT_WGT_DE;
            shipmentDataRequest.SVL_NR = shipmentData.SVL_NR;
            shipmentDataRequest.WFL_ID = shipmentData.WFL_ID;
            shipmentDataRequest.WGT_UNT_TE = shipmentData.WGT_UNT_TE;
            shipmentDataRequest.ACY_TE = shipmentData.ACY_TE;
            shipmentDataRequest.CON_NR = shipmentData.CON_NR;
            shipmentDataRequest.SPC_SLIC_NR = shipmentData.SPC_SLIC_NR;
            shipmentDataRequest.POD_RTN_SVC = shipmentData.POD_RTN_SVC;
            shipmentDataRequest.TranslationScore = shipmentData.TranslationScore;
            this.context.shipmentDataRequests.Add(shipmentDataRequest);
            this.context.Entry(shipmentDataRequest).State = EntityState.Added;
            this.context.SaveChanges();

            return 0;
        }

        public ShipmentDataResponse CreateShipments(List<ShipmentDataRequest> shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                List<ShipmentDataRequest> validEntities =
                    this.entityValidationService.FilterValidEntity<ShipmentDataRequest>(shipmentDataRequest);

                if (validEntities != null && validEntities.Any())
                {
                    this.context.BulkInsert(validEntities);
                    shipmentDataResponse.Shipments = shipmentDataRequest; //this.context.shipmentDataRequests.ToList();
                    shipmentDataResponse.Success = true;
                }

                return shipmentDataResponse;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public ShipmentDataResponse UpdateShipmentStatusById(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                ShipmentDataRequest data = this.context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                data.ID = shipmentDataRequest.ID;
                data.WFL_ID = shipmentDataRequest.WFL_ID;
                data.SMT_STA_NR = shipmentDataRequest.SMT_STA_NR;
                this.context.shipmentDataRequests.Update(data);
                this.context.Entry(shipmentDataRequest).State = EntityState.Detached;
                this.context.SaveChanges();
                shipmentDataResponse.ShipmentDataRequest = this.context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                shipmentDataResponse.Success = true;
                return shipmentDataResponse;
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }

            return shipmentDataResponse;
        }

        public int? SelectShipmentTotalStatusByWorkflowId(int wid)
        {
            int? result = 0;
            decimal? totalCount = 1;
            try
            {
                int inprogressCount = this.context.shipmentDataRequests.Where(ship => (ship.WFL_ID == wid && ship.SMT_STA_NR == 1) || (ship.WFL_ID == wid && ship.SMT_STA_NR == 2)).Count();
                int completedCount = this.context.shipmentDataRequests.Where(ship => ship.WFL_ID == wid && ship.SMT_STA_NR == 3).Count();
                totalCount = this.context.shipmentDataRequests.Where(ship => ship.WFL_ID == wid).Count();
                if (completedCount == totalCount)
                {
                    result = 3;
                }
                else if (inprogressCount > 0)
                {
                    result = 2;
                }
                else
                {
                    result = 0;
                }
            }
            catch
            {
                return result;
            }
            return result;
        }

        public ShipmentDataResponse UpdateShipmentAddressById(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                //Shipment Update
                ShipmentDataRequest data = this.context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();

                string beforeAddress = data.SHP_ADR_TR_TE;
                int? shipmentStaus =
                   data.SHP_ADR_TR_TE.ToLower() != shipmentDataRequest.SHP_ADR_TR_TE.ToLower()
                   ? ((int)Enums.ATStatus.Curated)
                   : (data.SMT_STA_NR);

                data.SHP_ADR_TR_TE = shipmentDataRequest.SHP_ADR_TR_TE;
                data.COD_TE = shipmentDataRequest.COD_TE;
                data.POD_RTN_SVC = shipmentDataRequest.POD_RTN_SVC;
                data.SMT_STA_NR = shipmentStaus;

                switch (shipmentStaus)
                {
                    case 0:
                        shipmentDataRequest.SMT_STA_TE = "Uploaded";
                        break;
                    case 1:
                        shipmentDataRequest.SMT_STA_TE = "Curated";
                        break;
                    case 2:
                        shipmentDataRequest.SMT_STA_TE = "Translated";
                        break;
                    case 3:
                        shipmentDataRequest.SMT_STA_TE = "Completed";
                        break;
                    case 4:
                        shipmentDataRequest.SMT_STA_TE = "Inactive";
                        break;
                    default:
                        shipmentDataRequest.SMT_STA_TE = "Uploaded";
                        break;
                }

                this.context.shipmentDataRequests.Update(data);
                this.context.Entry(shipmentDataRequest).State = EntityState.Detached;
                this.context.SaveChanges();
                shipmentDataResponse.ShipmentDataRequest = this.context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                shipmentDataResponse.Success = true;
                shipmentDataResponse.BeforeAddress = string.Empty;
                if (!string.Equals(beforeAddress, data.SHP_ADR_TR_TE))
                {
                    shipmentDataResponse.BeforeAddress = beforeAddress;
                    if(shipmentDataRequest.RCV_ADR_TE != null)
                    {
                        var matchedShipments = this.context.shipmentDataRequests.Where(s => s.RCV_ADR_TE == shipmentDataRequest.RCV_ADR_TE).ToList();
                        if (matchedShipments.Any())
                        {
                            matchedShipments.ForEach(shipment =>
                            {
                                shipment.SHP_ADR_TR_TE = data.SHP_ADR_TR_TE;
                                shipment.SMT_STA_NR = shipmentStaus;
                            });

                            this.context.BulkUpdate(matchedShipments);
                        }


                        List<AddressBook> addressBookElements = this.context.AddressBooks.Where(s => s.ConsigneeAddress == shipmentDataRequest.RCV_ADR_TE).ToList();

                        if (addressBookElements.Any())
                        {
                            addressBookElements.FirstOrDefault().ConsigneeTranslatedAddress = data.SHP_ADR_TR_TE;
                            addressBookElements.FirstOrDefault().ModifiedDate = DateTime.Parse(DateTime.Now.ToString()).ToLocalTime();

                            this.context.BulkUpdate(addressBookElements);
                        }
                    }
                }
                return shipmentDataResponse;
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }

            return shipmentDataResponse;
        }

        public void UpdateShipmentAddressByIds(List<ShipmentDataRequest> shipmentDataRequest)
        {
            try
            {
                this.context.BulkUpdate(shipmentDataRequest);
            }
            catch (Exception ex)
            {

            }
        }
        //public ShipmentDataResponse UpdateShipmentAddressByIds(List<ShipmentDataRequest> shipmentDataRequest)
        //{
        //    ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
        //    try
        //    {
        //        optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();


        //        optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        //        var this.context = new ApplicationDbContext(optionsBuilder.Options);

        //        List<ShipmentDataRequest> shipmentDetailsToUpdate = new List<ShipmentDataRequest>();
        //        foreach (ShipmentDataRequest request in shipmentDataRequest)
        //        {
        //            ShipmentDataRequest data = this.context.shipmentDataRequests.Where(s => s.ID == request.ID).FirstOrDefault();
        //            data.ID = request.ID;
        //            data.WFL_ID = request.WFL_ID;
        //            data.SHP_ADR_TR_TE = request.SHP_ADR_TR_TE;
        //            data.SMT_STA_NR = request.SMT_STA_NR;
        //            data.ACY_TE = request.ACY_TE;
        //            data.CON_NR = request.CON_NR;
        //            data.POD_RTN_SVC = request.POD_RTN_SVC;
        //            shipmentDetailsToUpdate.Add(data);
        //            //this.context.shipmentDataRequests.Update(data);
        //            //this.context.Entry(request).State = EntityState.Detached;
        //            //this.context.SaveChanges();
        //            shipmentDataResponse.Shipments = this.context.shipmentDataRequests;
        //        }
        //        this.context.BulkUpdate(shipmentDetailsToUpdate);
        //        //shipmentDataResponse.Shipments = this.context.shipmentDataRequests;
        //        shipmentDataResponse.Success = true;
        //        return shipmentDataResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        shipmentDataResponse.Success = false;
        //        shipmentDataResponse.OperationExceptionMsg = ex.Message;
        //    }
        //    return shipmentDataResponse;
        //}

        public ShipmentDataResponse DeleteShipments(List<ShipmentDataRequest> shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                int? workflowID = shipmentDataRequest?.FirstOrDefault().WFL_ID;
                int shipmentCount = 0;

                foreach (ShipmentDataRequest request in shipmentDataRequest)
                {
                    ShipmentDataRequest data = this.context.shipmentDataRequests.Where(s => s.ID == request.ID).FirstOrDefault();
                    this.context.shipmentDataRequests.Remove(data);
                    this.context.Entry(request).State = EntityState.Detached;
                    this.context.SaveChanges();
                }

                if (workflowID.HasValue)
                {
                    shipmentCount = this.context.shipmentDataRequests.Where(ship => ship.WFL_ID == workflowID).Count();
                }

                if (shipmentCount == 0)
                {
                    WorkflowService workflowService = new WorkflowService(this.context, this.addressBookService, this.entityValidationService);
                    WorkflowDataResponse workflowDataResponse = workflowService.DeleteWorkflowById(workflowID.Value);
                    if(workflowDataResponse.Success)
                    {
                        shipmentDataResponse.HasWorkflow = false;
                    }
                }
                else
                {
                    shipmentDataResponse.HasWorkflow = true;
                }

                shipmentDataResponse.Success = true;
                return shipmentDataResponse;
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }

        public ShipmentDataResponse DeleteShipment(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                ShipmentDataRequest data = this.context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                this.context.shipmentDataRequests.Remove(data);
                this.context.Entry(shipmentDataRequest).State = EntityState.Deleted;
                this.context.SaveChanges();
                shipmentDataResponse.Shipments = this.context.shipmentDataRequests;
                shipmentDataResponse.Success = true;
                return shipmentDataResponse;
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }

        public ShipmentDataResponse CreateShipments(List<ExcelDataObject> excelDataObjects, int workflowID, out int? workflowStatus)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            workflowStatus = 0;
            try
            {
                List<ShipmentDataRequest> shipmentDataRequest = ExtractShipmentDataRequest(excelDataObjects, workflowID,out workflowStatus);
                shipmentDataResponse = CreateShipments(shipmentDataRequest);
                shipmentDataResponse.Success = true;
                return shipmentDataResponse;
            }
            catch (Exception exception)
            {
                shipmentDataResponse.OperationExceptionMsg = exception.Message;
                shipmentDataResponse.Success = true;
                //AuditEventEntry.WriteEntry(new Exception(exception.Message));
            }
            return shipmentDataResponse;
        }

        private List<ShipmentDataRequest> ExtractShipmentDataRequest(List<ExcelDataObject> excelDataObjects, int workflowID, out int? workflowStatus)
        {
            List<ShipmentDataRequest> shipmentData = new List<ShipmentDataRequest>();
            List<AddressBook> addressBooks = this.addressBookService.GetAddressBooks();
            int? wfStatus = 0;
            //foreach (ExcelDataObject excelDataObject in excelDataObjects)
            excelDataObjects.ForEach(excelDataObject =>
            {
                bool allPropertiesNull = !excelDataObject.GetType().GetProperties().Any(prop => prop == null);
                if (allPropertiesNull)
                {

                    ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();

                    if (!string.IsNullOrWhiteSpace(excelDataObject.S_packageno))
                    {

                        shipmentDataRequest.BIL_TYP_TE = excelDataObject.S_billtype != null && excelDataObject.S_billtype.Contains('.') ? excelDataObject.S_billtype.Split('.')[0] : excelDataObject.S_billtype;
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
                        shipmentDataRequest.DST_PSL_TE = excelDataObject.S_dstpsl != null && excelDataObject.S_dstpsl.Contains('.') ? excelDataObject.S_dstpsl.Split('.')[0] : excelDataObject.S_dstpsl;
                        shipmentDataRequest.EXP_SLC_CD = excelDataObject.S_expslic != null && excelDataObject.S_expslic.Contains('.') ? excelDataObject.S_expslic.Split('.')[0] : excelDataObject.S_expslic; 
                        shipmentDataRequest.EXP_TYP = "顺丰即日";//excelDataObject.S_expslic;
                        shipmentDataRequest.IMP_NR = excelDataObject.S_impr;
                        shipmentDataRequest.IMP_SLC_TE = excelDataObject.S_impslic != null && excelDataObject.S_impslic.Contains('.') ? excelDataObject.S_impslic.Split('.')[0] : excelDataObject.S_impslic;
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
                            excelDataObject.pcs = excelDataObject.pcs.Contains('.') ? excelDataObject.pcs.Split('.')[0] : excelDataObject.pcs;
                            if (int.TryParse(excelDataObject.pcs, out intvalue))
                            {
                                shipmentDataRequest.PCS_QTY_NR = intvalue;
                            }
                        }

                        //shipmentDataRequest.PCS_QTY_NR = null;//Convert.ToInt32(Convert.ToDouble(excelDataObject.pcs));
                        shipmentDataRequest.PH_NR = excelDataObject.S_ph != null && excelDataObject.S_ph.Contains('.') ? excelDataObject.S_ph.Split('.')[0] : excelDataObject.S_ph;
                        shipmentDataRequest.PKG_NR_TE = excelDataObject.S_packageno;
                        //shipmentDataRequest.PKG_WGT_DE = Convert.ToDecimal(excelDataObject.S_pkgwei);

                        shipmentDataRequest.PKG_WGT_DE = 0;
                        if (!string.IsNullOrEmpty(excelDataObject.S_pkgwei))
                        {
                            if (decimal.TryParse(excelDataObject.S_pkgwei, out decimalvalue))
                            {
                                shipmentDataRequest.PKG_WGT_DE = decimalvalue;
                            }
                        }

                        shipmentDataRequest.PK_UP_TM = null;//Convert.ToString(excelDataObject.S_pkuptime),
                        shipmentDataRequest.PYM_MTD = "寄付月结";//excelDataObject.pymt;
                        shipmentDataRequest.RCV_ADR_TE = excelDataObject.S_address1;
                        shipmentDataRequest.RCV_CPY_TE = excelDataObject.S_receivercompany;
                        shipmentDataRequest.SHP_ADR_TE = excelDataObject.address;

                        //AddressBook translatedAddress =
                        //    addressBooks
                        //    ?.FirstOrDefault(
                        //        (AddressBook address) =>
                        //            address.ConsigneeAddress.Replace(" ","").ToLower().Trim().Equals(
                        //            excelDataObject.S_address1.Replace(" ", "").ToLower().Trim(), StringComparison.OrdinalIgnoreCase));

                        //if (translatedAddress != null)
                        //{
                        //shipmentDataRequest.SHP_ADR_TR_TE = translatedAddress.ConsigneeTranslatedAddress;
                        //shipmentDataRequest.SMT_STA_NR = (int)Enums.ATStatus.Translated;
                        //wfStatus = shipmentDataRequest.SMT_STA_NR;
                        //shipmentDataRequest.SMT_STA_TE = Convert.ToString(Enums.ATStatus.Translated);
                        //shipmentDataRequest.CON_NR = translatedAddress.Confidence;
                        //shipmentDataRequest.ACY_TE = translatedAddress.Accuracy;
                        //shipmentDataRequest.TranslationScore = translatedAddress.TranslationScore;
                        //}
                        //else
                        //{
                            shipmentDataRequest.SHP_ADR_TR_TE = excelDataObject.S_address1; ;
                            shipmentDataRequest.SMT_STA_NR = (int)Enums.ATStatus.Translated;
                            shipmentDataRequest.SMT_STA_TE = Convert.ToString(Enums.ATStatus.Translated);

                        //}

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
                        shipmentDataRequest.SHP_PH_TE = excelDataObject.S_shptph != null && excelDataObject.S_shptph.Contains('.') ? excelDataObject.S_shptph.Split('.')[0] : excelDataObject.S_shptph;
                        shipmentDataRequest.SMT_NR_TE = excelDataObject.S_shipmentno;

                        shipmentDataRequest.SMT_VAL_DE = 0;
                        if (!string.IsNullOrEmpty(excelDataObject.value))
                        {
                            if (decimal.TryParse(excelDataObject.value, out decimalvalue))
                            {
                                shipmentDataRequest.SMT_VAL_DE = decimalvalue;
                            }
                        }
                        shipmentDataRequest.SMT_WGT_DE = 0;
                        if (!string.IsNullOrEmpty(excelDataObject.S_shptwei))
                        {
                            if (decimal.TryParse(excelDataObject.S_shptwei, out decimalvalue))
                            {
                                shipmentDataRequest.SMT_WGT_DE = decimalvalue;
                            }
                        }
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

            });
            workflowStatus = wfStatus;
            return shipmentData;
        }
    }
}
