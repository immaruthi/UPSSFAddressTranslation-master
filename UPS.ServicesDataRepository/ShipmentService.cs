using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class ShipmentService : IShipmentAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public List<ShipmentDataRequest> GetShipment(int workflowID)
        {

            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var context = new ApplicationDbContext(optionsBuilder.Options);
            var shipments = context.shipmentDataRequests.Where(w => w.WFL_ID == workflowID).OrderBy(s => s.SHP_ADR_TE).ToList();
            return shipments;
        }

        public int CreateShipment(ShipmentDataRequest shipmentData)
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
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
                context.shipmentDataRequests.Add(shipmentDataRequest);
                context.Entry(shipmentDataRequest).State = EntityState.Added;
                context.SaveChanges();
            }

            return 0;
        }

        public ShipmentDataResponse CreateShipments(List<ShipmentDataRequest> shipmentData)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    foreach (ShipmentDataRequest Data in shipmentData)
                    {
                        ShipmentDataRequest shipmentDataRequest = new ShipmentDataRequest();
                        shipmentDataRequest.BIL_TYP_TE = Data.BIL_TYP_TE;
                        shipmentDataRequest.CCY_VAL_TE = Data.CCY_VAL_TE;
                        shipmentDataRequest.COD_TE = Data.COD_TE;
                        shipmentDataRequest.CSG_CTC_TE = Data.CSG_CTC_TE;
                        shipmentDataRequest.DIM_WGT_DE = Data.DIM_WGT_DE;
                        shipmentDataRequest.DST_CTY_TE = Data.DST_CTY_TE;
                        shipmentDataRequest.DST_PSL_TE = Data.DST_PSL_TE;
                        shipmentDataRequest.EXP_SLC_CD = Data.EXP_SLC_CD;
                        shipmentDataRequest.EXP_TYP = Data.EXP_TYP;
                        shipmentDataRequest.FST_INV_LN_DES_TE = Data.FST_INV_LN_DES_TE;
                        shipmentDataRequest.IMP_NR = Data.IMP_NR;
                        shipmentDataRequest.IMP_SLC_TE = Data.IMP_SLC_TE;
                        shipmentDataRequest.IN_FLG_TE = Data.IN_FLG_TE;
                        shipmentDataRequest.ORG_CTY_TE = Data.ORG_CTY_TE;
                        shipmentDataRequest.ORG_PSL_CD = Data.ORG_PSL_CD;
                        shipmentDataRequest.OU_FLG_TE = Data.OU_FLG_TE;
                        shipmentDataRequest.PCS_QTY_NR = Data.PCS_QTY_NR;
                        shipmentDataRequest.PH_NR = Data.PH_NR;
                        shipmentDataRequest.PKG_NR_TE = Data.PKG_NR_TE;
                        shipmentDataRequest.PKG_WGT_DE = Data.PKG_WGT_DE;
                        shipmentDataRequest.PK_UP_TM = null; //Data.PK_UP_TM;
                        shipmentDataRequest.PYM_MTD = Data.PYM_MTD;
                        shipmentDataRequest.PY_MT_TE = Data.PY_MT_TE;
                        shipmentDataRequest.QQS_TRA_LG_ID = Data.QQS_TRA_LG_ID;
                        shipmentDataRequest.RCV_ADR_TE = Data.RCV_ADR_TE;
                        shipmentDataRequest.RCV_CPY_TE = Data.RCV_CPY_TE;
                        shipmentDataRequest.SF_TRA_LG_ID = Data.SF_TRA_LG_ID;
                        shipmentDataRequest.SHP_ADR_TE = Data.SHP_ADR_TE;
                        shipmentDataRequest.SHP_ADR_TR_TE = Data.SHP_ADR_TR_TE;
                        shipmentDataRequest.SHP_CPY_NA = Data.SHP_CPY_NA;
                        shipmentDataRequest.SHP_CTC_TE = Data.SHP_CTC_TE;
                        shipmentDataRequest.SHP_DT = null; //Data.SHP_DT;
                        shipmentDataRequest.SHP_NR = Data.SHP_NR;
                        shipmentDataRequest.SHP_PH_TE = Data.SHP_PH_TE;
                        shipmentDataRequest.SMT_NR_TE = Data.SMT_NR_TE;
                        shipmentDataRequest.SMT_STA_NR = Data.SMT_STA_NR;
                        shipmentDataRequest.SMT_VAL_DE = Data.SMT_VAL_DE;
                        shipmentDataRequest.SMT_WGT_DE = Data.SMT_WGT_DE;
                        shipmentDataRequest.SVL_NR = Data.SVL_NR;
                        shipmentDataRequest.WFL_ID = Data.WFL_ID;
                        shipmentDataRequest.WGT_UNT_TE = Data.WGT_UNT_TE;
                        context.shipmentDataRequests.Add(shipmentDataRequest);
                        context.Entry(shipmentDataRequest).State = EntityState.Added;
                    }

                    context.SaveChanges();
                    shipmentDataResponse.Shipments = context.shipmentDataRequests.ToList();
                    shipmentDataResponse.Success = true;
                    return shipmentDataResponse;


                }

                //return shipmentDataResponse;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        public ShipmentDataResponse UpdateShipmentStatusById(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.EnableSensitiveDataLogging(true);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    data.ID = shipmentDataRequest.ID;
                    data.WFL_ID = shipmentDataRequest.WFL_ID;
                    data.SMT_STA_NR = shipmentDataRequest.SMT_STA_NR;
                    context.shipmentDataRequests.Update(data);
                    context.Entry(shipmentDataRequest).State = EntityState.Detached;
                    context.SaveChanges();
                    shipmentDataResponse.ShipmentDataRequest = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    shipmentDataResponse.Success = true;
                    return shipmentDataResponse;
                }
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }

        public ShipmentDataResponse UpdateShipmentAddressById(ShipmentDataRequest shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    data.SHP_ADR_TR_TE = shipmentDataRequest.SHP_ADR_TR_TE;
                    data.COD_TE = shipmentDataRequest.COD_TE;
                    data.SMT_STA_NR = ((int)Enums.ShipmentStatus.Curated);
                    context.shipmentDataRequests.Update(data);
                    context.Entry(shipmentDataRequest).State = EntityState.Detached;
                    context.SaveChanges();
                    shipmentDataResponse.ShipmentDataRequest = context.shipmentDataRequests.Where(s => s.ID == shipmentDataRequest.ID).FirstOrDefault();
                    shipmentDataResponse.Success = true;
                    return shipmentDataResponse;
                }
            }
            catch (Exception ex)
            {
                shipmentDataResponse.Success = false;
                shipmentDataResponse.OperationExceptionMsg = ex.Message;
            }
            return shipmentDataResponse;
        }

        public ShipmentDataResponse UpdateShipmentAddressByIds(List<ShipmentDataRequest> shipmentDataRequest)
        {
            ShipmentDataResponse shipmentDataResponse = new ShipmentDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();


                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                var context = new ApplicationDbContext(optionsBuilder.Options);

                foreach (ShipmentDataRequest request in shipmentDataRequest)
                {
                    ShipmentDataRequest data = context.shipmentDataRequests.Where(s => s.ID == request.ID).FirstOrDefault();
                    data.ID = request.ID;
                    data.WFL_ID = request.WFL_ID;
                    data.SHP_ADR_TR_TE = request.SHP_ADR_TR_TE;
                    data.SMT_STA_NR = request.SMT_STA_NR;
                    context.shipmentDataRequests.Update(data);
                    context.Entry(request).State = EntityState.Detached;
                    context.SaveChanges();
                    shipmentDataResponse.Shipments = context.shipmentDataRequests;
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

    }
}
