namespace UPS.ServicesDataRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EFCore.BulkExtensions;
    using Microsoft.EntityFrameworkCore;
    using NLog.Targets.Wrappers;
    using UPS.DataObjects.Shipment;
    using UPS.DataObjects.SPC_LST;
    using UPS.ServicesAsyncActions;
    using UPS.ServicesDataRepository.Common;
    using UPS.ServicesDataRepository.DataContext;

    public class ShipperCompanyService : IShipperCompanyAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        private readonly ApplicationDbContext context;
        private ShipperCompanyResponse response;
        public ShipperCompanyService(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
            this.optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            this.optionsBuilder.EnableSensitiveDataLogging(true);
            this.response = new ShipperCompanyResponse();
            this.response.Success = true;
        }

        public ShipmentDataResponse SelectMatchedShipmentsWithShipperCompanies(int workflowID)

        {
            ShipmentDataResponse mappedShipAndShipperCompanyResponse = new ShipmentDataResponse();
            List<ShipmentDataRequest> shipmentDataRequests = null;
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    shipmentDataRequests = new List<ShipmentDataRequest>();
                    var anonymousList =
                        (
                            from s in context.shipmentDataRequests
                            join c in context.shipperCompanyRequests on s.DST_PSL_TE equals c.SPC_PSL_CD_TE
                            where
                                s.WFL_ID == workflowID
                                &&  
                                (
                                        s.SMT_STA_NR == (int)Enums.ATStatus.Translated
                                    ||  s.SMT_STA_NR == (int)Enums.ATStatus.Curated
                                )
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
                                ORG_CTY_TE = c.SPC_CTY_TE,
                                ORG_PSL_CD = c.SPC_PSL_CD_TE,
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
                                SHP_ADR_TE = c.SPC_ADR_TE,
                                s.SHP_ADR_TR_TE,
                                SHP_CPY_NA = c.SPC_CPY_TE,
                                SHP_CTC_TE = c.SPC_NA,
                                s.SHP_DT,
                                s.SHP_NR,
                                SHP_PH_TE = c.SPC_SND_PTY_CTC_TE,
                                s.SMT_NR_TE,
                                s.SMT_STA_NR,
                                s.SMT_VAL_DE,
                                s.SMT_WGT_DE,
                                SPC_SLIC_NR = c.SPC_SLIC_NR,
                                s.SVL_NR,
                                s.WGT_UNT_TE,
                                s.POD_RTN_SVC,
                                c.SPC_CST_ID_TE,
                                s.TR_SCR_NR
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
                        shipmentDataRequest.SPC_CST_ID_TE = shipmentData.SPC_CST_ID_TE;
                        shipmentDataRequest.TR_SCR_NR = shipmentData.TR_SCR_NR;

                        shipmentDataRequests.Add(shipmentDataRequest);
                    }
                    //shipmentDataRequests = anonymousList.Cast<ShipmentDataRequest>().ToList();
                    mappedShipAndShipperCompanyResponse.Success = true;
                    mappedShipAndShipperCompanyResponse.Shipments = shipmentDataRequests;
                }
            }
            catch (Exception ex)
            {
                mappedShipAndShipperCompanyResponse.Success = false;
                mappedShipAndShipperCompanyResponse.OperationExceptionMsg = ex.Message;

            }
            return mappedShipAndShipperCompanyResponse;
        }
        public ShipmentDataResponse SelectCompletedShipments(int workflowID)

        {
            ShipmentDataResponse mappedShipAndShipperCompanyResponse = new ShipmentDataResponse();
            List<ShipmentDataRequest> shipmentDataRequests = null;
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    shipmentDataRequests = new List<ShipmentDataRequest>();
                    var anonymousList =
                        (
                            from s in context.shipmentDataRequests
                            join c in context.shipperCompanyRequests on s.DST_PSL_TE equals c.SPC_PSL_CD_TE
                            where s.WFL_ID == workflowID
                            && s.SMT_STA_NR == ((int)Enums.ATStatus.Completed)
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
                                ORG_CTY_TE = c.SPC_CTY_TE,
                                ORG_PSL_CD = c.SPC_PSL_CD_TE,
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
                                SHP_ADR_TE = c.SPC_ADR_TE,
                                s.SHP_ADR_TR_TE,
                                SHP_CPY_NA = c.SPC_CPY_TE,
                                SHP_CTC_TE = c.SPC_NA,
                                s.SHP_DT,
                                s.SHP_NR,
                                SHP_PH_TE = c.SPC_SND_PTY_CTC_TE,
                                s.SMT_NR_TE,
                                s.SMT_STA_NR,
                                s.SMT_VAL_DE,
                                s.SMT_WGT_DE,
                                SPC_SLIC_NR = c.SPC_SLIC_NR,
                                s.SVL_NR,
                                s.WGT_UNT_TE,
                                s.POD_RTN_SVC,
                                c.SPC_CST_ID_TE,
                                s.TR_SCR_NR
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
                        shipmentDataRequest.SPC_CST_ID_TE = shipmentData.SPC_CST_ID_TE;
                        shipmentDataRequest.TR_SCR_NR = shipmentData.TR_SCR_NR;

                        shipmentDataRequests.Add(shipmentDataRequest);
                    }
                    //shipmentDataRequests = anonymousList.Cast<ShipmentDataRequest>().ToList();
                    mappedShipAndShipperCompanyResponse.Success = true;
                    mappedShipAndShipperCompanyResponse.Shipments = shipmentDataRequests;
                }
            }
            catch (Exception ex)
            {
                mappedShipAndShipperCompanyResponse.Success = false;
                mappedShipAndShipperCompanyResponse.OperationExceptionMsg = ex.Message;

            }
            return mappedShipAndShipperCompanyResponse;

        }

        public ShipperCompanyResponse SelectShipperCompanies()
        {
            ShipperCompanyResponse shipperCompanyResponse = new ShipperCompanyResponse();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    shipperCompanyResponse.Success = true;
                    shipperCompanyResponse.ShipperCompanies = context.shipperCompanyRequests;
                    return shipperCompanyResponse;
                }
                catch (Exception ex)
                {
                    shipperCompanyResponse.Success = false;
                    shipperCompanyResponse.OperatonExceptionMessage = ex.Message;
                }
            }
            return shipperCompanyResponse;
        }

        public ShipperCompanyResponse GetShipperList()
        {
            ShipperCompanyResponse shipperCompanyResponse = new ShipperCompanyResponse();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    shipperCompanyResponse.ShipperCompanies = context.shipperCompanyRequests.ToList();
                    shipperCompanyResponse.Success = true;
                    return shipperCompanyResponse;
                }
                catch (Exception ex)
                {
                    shipperCompanyResponse.Success = false;
                    shipperCompanyResponse.OperatonExceptionMessage = ex.Message;
                }
            }
            return shipperCompanyResponse;
        }

        public ShipperCompanyResponse InsertShipper(ShipperCompanyList shipperCompanyRequest)
        {

            try
            {
                this.context.Add(shipperCompanyRequest);
                this.context.SaveChanges();
                this.response.ShipperCompany = shipperCompanyRequest;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.OperatonExceptionMessage = ex.Message;
            }
            return this.response;
        }

        public ShipperCompanyResponse UpdateShipper(ShipperCompanyList shipperCompanyRequest)
        {
            try
            {
                ShipperCompanyList data = this.context.shipperCompanyRequests.Where(s => s.ID == shipperCompanyRequest.ID).FirstOrDefault();
                data.ID = shipperCompanyRequest.ID;
                data.SPC_ADR_TE = shipperCompanyRequest.SPC_ADR_TE;
                data.SPC_CPY_TE = shipperCompanyRequest.SPC_CPY_TE;
                data.SPC_CST_ID_TE = shipperCompanyRequest.SPC_CST_ID_TE;
                data.SPC_CTC_PH = shipperCompanyRequest.SPC_CTC_PH;
                data.SPC_CTR_TE = shipperCompanyRequest.SPC_CTR_TE;
                data.SPC_CTY_TE = shipperCompanyRequest.SPC_CTY_TE;
                data.SPC_NA = shipperCompanyRequest.SPC_NA;
                data.SPC_PSL_CD_TE = shipperCompanyRequest.SPC_PSL_CD_TE;
                data.SPC_SLIC_NR = shipperCompanyRequest.SPC_SLIC_NR;
                data.SPC_SND_PTY_CTC_TE = shipperCompanyRequest.SPC_SND_PTY_CTC_TE;
                this.context.shipperCompanyRequests.Update(data);
                this.context.Entry(shipperCompanyRequest).State = EntityState.Detached;
                this.context.SaveChanges();
                this.response.ShipperCompany = this.context.shipperCompanyRequests.Where(s => s.ID == shipperCompanyRequest.ID).FirstOrDefault();
                this.response.Success = true;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.OperatonExceptionMessage = ex.Message;
            }
            return this.response;
        }

        public ShipperCompanyResponse DeleteShipper(List<ShipperCompanyList> shipperCompanyRequests)
        {
            try
            {
                this.context.BulkDelete(shipperCompanyRequests);
                this.context.Entry(shipperCompanyRequests).State = EntityState.Detached;
                this.context.SaveChanges();
                this.response.ShipperCompanies = shipperCompanyRequests;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.OperatonExceptionMessage = ex.Message;
            }
            return this.response;
        }

        public async  Task<List<string>> GetShipmentCompanyCities()
        {
            List<string> cities =
                await this.context.shipperCompanyRequests
                        .Select(
                            (ShipperCompanyList shipperList) =>
                                shipperList.SPC_CTY_TE)
                        .Distinct()
                        .OrderBy(city=>city)
                        .ToListAsync();

            return cities;
        }
    }
}
