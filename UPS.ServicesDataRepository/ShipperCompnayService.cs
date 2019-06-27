using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;
using UPS.DataObjects.SPC_LST;
using UPS.ServicesDataRepository.Common;

namespace UPS.ServicesDataRepository
{
    public class ShipperCompnayService : IShipperCompanyAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public ShipmentDataResponse SelectMatchedShipmentsWithShipperCompanies(int workflowID)

        {
            ShipmentDataResponse mappedShipAndShipperCompanyResponse = new ShipmentDataResponse();
            List<ShipmentDataRequest> shipmentDataRequests = null;
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    int scount = context.shipmentDataRequests.Count();
                    int ccount = context.shipperCompanyRequests.Count();

                    List<ShipmentDataRequest> sRequest = new List<ShipmentDataRequest>();
                    sRequest = context.shipmentDataRequests.ToList();
                    List<ShipperCompanyRequest> cRequest = new List<ShipperCompanyRequest>();
                    cRequest = context.shipperCompanyRequests.ToList();

                    shipmentDataRequests = new List<ShipmentDataRequest>();
                    var anonymousList =
                        (
                            from s in context.shipmentDataRequests
                            join c in context.shipperCompanyRequests on s.DST_PSL_TE equals c.SPC_PSL_CD_TE where s.WFL_ID == workflowID
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
                                DST_CTY_TE = c.SPC_CTY_TE,
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
                                PH_NR = c.SPC_CTC_PH,
                                s.PKG_NR_TE,
                                s.PKG_WGT_DE,
                                s.PK_UP_TM,
                                s.PYM_MTD,
                                s.PY_MT_TE,
                                s.QQS_TRA_LG_ID,
                                RCV_ADR_TE = c.SPC_ADR_TE,
                                RCV_CPY_TE = c.SPC_CPY_TE,
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
                                SPC_SLIC_NR = c.SPC_SLIC_NR,
                                s.SVL_NR,
                                s.WGT_UNT_TE
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
                        shipmentDataRequest.SMT_VAL_DE = shipmentData.SMT_VAL_DE;
                        shipmentDataRequest.SMT_WGT_DE = shipmentData.SMT_WGT_DE;
                        shipmentDataRequest.SVL_NR = shipmentData.SVL_NR;
                        shipmentDataRequest.WFL_ID = shipmentData.WFL_ID;
                        shipmentDataRequest.WGT_UNT_TE = shipmentData.WGT_UNT_TE;
                        shipmentDataRequest.ACY_TE = shipmentData.ACY_TE;
                        shipmentDataRequest.CON_NR = shipmentData.CON_NR;
                        shipmentDataRequest.SPC_SLIC_NR = shipmentData.SPC_SLIC_NR;

                        shipmentDataRequests.Add(shipmentDataRequest);
                    }
                    //shipmentDataRequests = anonymousList.Cast<ShipmentDataRequest>().ToList();
                    mappedShipAndShipperCompanyResponse.Success = true;
                    mappedShipAndShipperCompanyResponse.Shipments = shipmentDataRequests;
                }
            }
            catch(Exception ex)
            {
                mappedShipAndShipperCompanyResponse.Success = false;
                mappedShipAndShipperCompanyResponse.OperationExceptionMsg = ex.Message;

            }
            return mappedShipAndShipperCompanyResponse;

            //context.shipmentDataRequests.Where(ship => ship.DST_PSL_TE)
            //var List = (from c in context.shipperCompanyRequests

            //            join l in context.workflowDataRequests
            //            on c.DST_PSL_TE equals l.SPC_PSL_CD_TE
            //            where c.WFL_ID == workflowID
            //            select new
            //            {
            //                shipmentID = c.ID,
            //                shipPostalID = l.ID,
            //                c.BIL_TYP_TE,
            //                c.CCY_VAL_TE,
            //                c.COD_TE,
            //                c.CSG_CTC_TE,
            //                c.DIM_WGT_DE,
            //                c.DST_CTY_TE,
            //                c.DST_PSL_TE,
            //                c.EXP_SLC_CD,
            //                c.EXP_TYP,
            //                c.FST_INV_LN_DES_TE,
            //                c.IMP_NR,
            //                c.IMP_SLC_TE,
            //                c.IN_FLG_TE,
            //                c.ORG_CTY_TE,
            //                c.ORG_PSL_CD,
            //                c.OU_FLG_TE,
            //                c.PCS_QTY_NR,
            //                c.PH_NR,
            //                c.PKG_NR_TE,
            //                c.PKG_WGT_DE,
            //                c.PK_UP_TM,
            //                c.PYM_MTD,
            //                c.PY_MT_TE,
            //                c.QQS_TRA_LG_ID,
            //                c.RCV_ADR_TE,
            //                c.RCV_CPY_TE,
            //                c.SF_TRA_LG_ID,
            //                c.SHP_ADR_TE,
            //                c.SHP_ADR_TR_TE,
            //                c.SHP_CPY_NA,
            //                c.SHP_CTC_TE,
            //                c.SHP_DT,
            //                c.SHP_NR,
            //                c.SHP_PH_TE,
            //                c.SMT_NR_TE,
            //                c.SMT_STA_NR,
            //                c.SMT_VAL_DE,
            //                c.SMT_WGT_DE,
            //                c.SVL_NR,
            //                c.WFL_ID,
            //                c.WGT_UNT_TE,
            //                l.SPC_ADR_TE,
            //                l.SPC_CPY_TE,
            //                l.SPC_CTC_PH,
            //                l.SPC_CTR_TE,
            //                l.SPC_CTY_TE,
            //                l.SPC_NA,
            //                l.SPC_PSL_CD_TE,
            //                l.SPC_SLIC_NR,
            //                l.SPC_SND_PTY_CTC_TE
            //            }).ToList();

            //foreach (var ship in List)
            //{
            //    ShipperCompanyRequest sLRequest = new ShipperCompanyRequest();
            //    sLRequest.SID = ship.shipmentID;
            //    sLRequest.ID = ship.shipPostalID;
            //    sLRequest.BIL_TYP_TE = ship.BIL_TYP_TE;
            //    sLRequest.CCY_VAL_TE = ship.CCY_VAL_TE;
            //    sLRequest.COD_TE = ship.COD_TE;
            //    sLRequest.CSG_CTC_TE = ship.CSG_CTC_TE;
            //    //  sLRequest.DIM_WGT_DE = ship.DIM_WGT_DE;
            //    sLRequest.DST_CTY_TE = ship.DST_CTY_TE;
            //    sLRequest.DST_PSL_TE = ship.DST_PSL_TE;
            //    sLRequest.EXP_SLC_CD = ship.EXP_SLC_CD;
            //    sLRequest.EXP_TYP = ship.EXP_TYP;
            //    sLRequest.FST_INV_LN_DES_TE = ship.FST_INV_LN_DES_TE;
            //    sLRequest.IMP_NR = ship.IMP_NR;
            //    sLRequest.IMP_SLC_TE = ship.IMP_SLC_TE;
            //    sLRequest.IN_FLG_TE = ship.IN_FLG_TE;
            //    sLRequest.ORG_CTY_TE = ship.ORG_CTY_TE;
            //    sLRequest.ORG_PSL_CD = ship.ORG_PSL_CD;
            //    sLRequest.OU_FLG_TE = ship.OU_FLG_TE;
            //    sLRequest.PCS_QTY_NR = ship.PCS_QTY_NR;
            //    sLRequest.PH_NR = ship.PH_NR;
            //    sLRequest.PKG_NR_TE = ship.PKG_NR_TE;
            //    sLRequest.PKG_WGT_DE = ship.PKG_WGT_DE;
            //    sLRequest.PK_UP_TM = ship.PK_UP_TM;
            //    sLRequest.PYM_MTD = ship.PYM_MTD;
            //    sLRequest.PY_MT_TE = ship.PY_MT_TE;
            //    sLRequest.QQS_TRA_LG_ID = ship.QQS_TRA_LG_ID;
            //    sLRequest.RCV_ADR_TE = ship.RCV_ADR_TE;
            //    sLRequest.RCV_CPY_TE = ship.RCV_CPY_TE;
            //    sLRequest.SF_TRA_LG_ID = ship.SF_TRA_LG_ID;
            //    sLRequest.SHP_ADR_TE = ship.SHP_ADR_TE;
            //    sLRequest.SHP_ADR_TR_TE = ship.SHP_ADR_TR_TE;
            //    sLRequest.SHP_CPY_NA = ship.SHP_CPY_NA;
            //    sLRequest.SHP_CTC_TE = ship.SHP_CTC_TE;
            //    sLRequest.SHP_DT = ship.SHP_DT;
            //    sLRequest.SHP_NR = ship.SHP_NR;
            //    sLRequest.SHP_PH_TE = ship.SHP_PH_TE;
            //    sLRequest.SMT_NR_TE = ship.SMT_NR_TE;
            //    sLRequest.SMT_STA_NR = ship.SMT_STA_NR;
            //    // sLRequest.SMT_VAL_DE = ship.SMT_VAL_DE;
            //    sLRequest.SMT_WGT_DE = ship.SMT_WGT_DE;
            //    sLRequest.SPC_ADR_TE = ship.SPC_ADR_TE;
            //    sLRequest.SPC_CPY_TE = ship.SPC_CPY_TE;
            //    sLRequest.SPC_CTC_PH = ship.SPC_CTC_PH;
            //    sLRequest.SPC_CTR_TE = ship.SPC_CTR_TE;
            //    sLRequest.SPC_CTY_TE = ship.SPC_CTY_TE;
            //    sLRequest.SPC_NA = ship.SPC_NA;
            //    sLRequest.SPC_PSL_CD_TE = ship.SPC_PSL_CD_TE;
            //    sLRequest.SPC_SLIC_NR = ship.SPC_SLIC_NR;
            //    sLRequest.SPC_SND_PTY_CTC_TE = ship.SPC_SND_PTY_CTC_TE;
            //    sLRequest.SVL_NR = ship.SVL_NR;
            //    sLRequest.WFL_ID = ship.WFL_ID;
            //    sLRequest.WGT_UNT_TE = ship.WGT_UNT_TE;

            //    shipperlist.Add(sLRequest);
            //}
            //return shipperlist;
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
                    int scount = context.shipmentDataRequests.Count();
                    int ccount = context.shipperCompanyRequests.Count();

                    List<ShipmentDataRequest> sRequest = new List<ShipmentDataRequest>();
                    sRequest = context.shipmentDataRequests.ToList();
                    List<ShipperCompanyRequest> cRequest = new List<ShipperCompanyRequest>();
                    cRequest = context.shipperCompanyRequests.ToList();

                    shipmentDataRequests = new List<ShipmentDataRequest>();
                    var anonymousList =
                        (
                            from s in context.shipmentDataRequests
                            join c in context.shipperCompanyRequests on s.DST_PSL_TE equals c.SPC_PSL_CD_TE
                            where s.WFL_ID == workflowID && s.SMT_STA_NR == ((int)Enums.ShipmentStatus.Completed)
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
                                DST_CTY_TE = c.SPC_CTY_TE,
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
                                PH_NR = c.SPC_CTC_PH,
                                s.PKG_NR_TE,
                                s.PKG_WGT_DE,
                                s.PK_UP_TM,
                                s.PYM_MTD,
                                s.PY_MT_TE,
                                s.QQS_TRA_LG_ID,
                                RCV_ADR_TE = c.SPC_ADR_TE,
                                RCV_CPY_TE = c.SPC_CPY_TE,
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
                                SPC_SLIC_NR = c.SPC_SLIC_NR,
                                s.SVL_NR,
                                s.WGT_UNT_TE
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
                        shipmentDataRequest.SMT_VAL_DE = shipmentData.SMT_VAL_DE;
                        shipmentDataRequest.SMT_WGT_DE = shipmentData.SMT_WGT_DE;
                        shipmentDataRequest.SVL_NR = shipmentData.SVL_NR;
                        shipmentDataRequest.WFL_ID = shipmentData.WFL_ID;
                        shipmentDataRequest.WGT_UNT_TE = shipmentData.WGT_UNT_TE;
                        shipmentDataRequest.ACY_TE = shipmentData.ACY_TE;
                        shipmentDataRequest.CON_NR = shipmentData.CON_NR;
                        shipmentDataRequest.SPC_SLIC_NR = shipmentData.SPC_SLIC_NR;

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
    }
}
