﻿using System;
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
                    shipmentDataRequests = new List<ShipmentDataRequest>();
                    var anonymousList =
                        (
                            from s in context.shipmentDataRequests
                            join c in context.shipperCompanyRequests on s.DST_PSL_TE equals c.SPC_PSL_CD_TE where s.WFL_ID == workflowID orderby s.ID 
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
                                SHP_PH_TE = c.SPC_CTC_PH,
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
                            join c in context.shipperCompanyRequests on s.DST_PSL_TE equals c.SPC_PSL_CD_TE where s.WFL_ID == workflowID orderby s.ID
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
                                SHP_PH_TE = c.SPC_CTC_PH,
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
