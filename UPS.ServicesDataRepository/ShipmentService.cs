using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class ShipmentService : IShipmentAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

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
                context.shipmentDataRequests.Add(shipmentDataRequest);
                context.Entry(shipmentDataRequest).State = EntityState.Added;
                context.SaveChanges();
            }

            return 0;
        }
    }
}
