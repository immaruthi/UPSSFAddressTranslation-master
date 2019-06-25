using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.UserData;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;


namespace UPS.ServicesDataRepository
{
    public class ShipmentListService : IShipmentListAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public List<ShipperListRequest> GetShipmentWithPostalCode(int workflowID)

        {
            List<ShipperListRequest> shipperlist = new List<ShipperListRequest>();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var context = new ApplicationDbContext(optionsBuilder.Options);
            var List = (from c in context.shipmentDataRequests
                         
                             join l in context.ShipperListRequests
                             on c.DST_PSL_TE equals l.SPC_PSL_CD_TE
                        where c.WFL_ID == workflowID
                        select new
                             {
                                 shipmentID=c.ID,
                                 shipPostalID=l.ID,
                                 c.BIL_TYP_TE,
                                 c.CCY_VAL_TE,
                                 c.COD_TE,
                            c.CSG_CTC_TE,
                            c.DIM_WGT_DE,
                            c.DST_CTY_TE,
                            c.DST_PSL_TE,
                            c.EXP_SLC_CD,
                            c.EXP_TYP,
                            c.FST_INV_LN_DES_TE,
                            c.IMP_NR,
                            c.IMP_SLC_TE,
                            c.IN_FLG_TE,
                            c.ORG_CTY_TE,
                            c.ORG_PSL_CD,
                            c.OU_FLG_TE,
                            c.PCS_QTY_NR,
                            c.PH_NR,
                            c.PKG_NR_TE,
                            c.PKG_WGT_DE,
                            c.PK_UP_TM,
                            c.PYM_MTD,
                            c.PY_MT_TE,
                            c.QQS_TRA_LG_ID,
                            c.RCV_ADR_TE,
                            c.RCV_CPY_TE,
                            c.SF_TRA_LG_ID,
                            c.SHP_ADR_TE,
                            c.SHP_ADR_TR_TE,
                            c.SHP_CPY_NA,
                            c.SHP_CTC_TE,
                            c.SHP_DT,
                            c.SHP_NR,
                            c.SHP_PH_TE,
                            c.SMT_NR_TE,
                            c.SMT_STA_NR,
                            c.SMT_VAL_DE,
                            c.SMT_WGT_DE,
                            c.SVL_NR,
                            c.WFL_ID,
                            c.WGT_UNT_TE,
                            l.SPC_ADR_TE,
                            l.SPC_CPY_TE,
                            l.SPC_CTC_PH,
                            l.SPC_CTR_TE,
                            l.SPC_CTY_TE,
                            l.SPC_NA,
                            l.SPC_PSL_CD_TE,
                            l.SPC_SLIC_NR,
                            l.SPC_SND_PTY_CTC_TE
                        }).ToList();

           foreach(var ship in List)
            {
                ShipperListRequest sLRequest = new ShipperListRequest();
                sLRequest.SID = ship.shipmentID;
                sLRequest.ID = ship.shipPostalID;
                sLRequest.BIL_TYP_TE = ship.BIL_TYP_TE;
                sLRequest.CCY_VAL_TE = ship.CCY_VAL_TE;
                sLRequest.COD_TE = ship.COD_TE;
                sLRequest.CSG_CTC_TE = ship.CSG_CTC_TE;
              //  sLRequest.DIM_WGT_DE = ship.DIM_WGT_DE;
                sLRequest.DST_CTY_TE = ship.DST_CTY_TE;
                sLRequest.DST_PSL_TE = ship.DST_PSL_TE;
                sLRequest.EXP_SLC_CD = ship.EXP_SLC_CD;
                sLRequest.EXP_TYP = ship.EXP_TYP;
                sLRequest.FST_INV_LN_DES_TE = ship.FST_INV_LN_DES_TE;
                sLRequest.IMP_NR = ship.IMP_NR;
                sLRequest.IMP_SLC_TE = ship.IMP_SLC_TE;
                sLRequest.IN_FLG_TE = ship.IN_FLG_TE;
                sLRequest.ORG_CTY_TE = ship.ORG_CTY_TE;
                sLRequest.ORG_PSL_CD = ship.ORG_PSL_CD;
                sLRequest.OU_FLG_TE = ship.OU_FLG_TE;
                sLRequest.PCS_QTY_NR = ship.PCS_QTY_NR;
                sLRequest.PH_NR = ship.PH_NR;
                sLRequest.PKG_NR_TE = ship.PKG_NR_TE;
                sLRequest.PKG_WGT_DE = ship.PKG_WGT_DE;
                sLRequest.PK_UP_TM = ship.PK_UP_TM;
                sLRequest.PYM_MTD = ship.PYM_MTD;
                sLRequest.PY_MT_TE = ship.PY_MT_TE;
                sLRequest.QQS_TRA_LG_ID = ship.QQS_TRA_LG_ID;
                sLRequest.RCV_ADR_TE = ship.RCV_ADR_TE;
                sLRequest.RCV_CPY_TE = ship.RCV_CPY_TE;
                sLRequest.SF_TRA_LG_ID = ship.SF_TRA_LG_ID;
                sLRequest.SHP_ADR_TE = ship.SHP_ADR_TE;
                sLRequest.SHP_ADR_TR_TE = ship.SHP_ADR_TR_TE;
                sLRequest.SHP_CPY_NA = ship.SHP_CPY_NA;
                sLRequest.SHP_CTC_TE = ship.SHP_CTC_TE;
                sLRequest.SHP_DT = ship.SHP_DT;
                sLRequest.SHP_NR = ship.SHP_NR;sLRequest.SHP_PH_TE = ship.SHP_PH_TE;sLRequest.SMT_NR_TE = ship.SMT_NR_TE;
                sLRequest.SMT_STA_NR = ship.SMT_STA_NR;
               // sLRequest.SMT_VAL_DE = ship.SMT_VAL_DE;
                sLRequest.SMT_WGT_DE = ship.SMT_WGT_DE;sLRequest.SPC_ADR_TE = ship.SPC_ADR_TE;
                sLRequest.SPC_CPY_TE = ship.SPC_CPY_TE;sLRequest.SPC_CTC_PH = ship.SPC_CTC_PH;
                sLRequest.SPC_CTR_TE = ship.SPC_CTR_TE;sLRequest.SPC_CTY_TE = ship.SPC_CTY_TE;
                sLRequest.SPC_NA = ship.SPC_NA;
                sLRequest.SPC_PSL_CD_TE = ship.SPC_PSL_CD_TE;sLRequest.SPC_SLIC_NR = ship.SPC_SLIC_NR;
                sLRequest.SPC_SND_PTY_CTC_TE = ship.SPC_SND_PTY_CTC_TE;sLRequest.SVL_NR = ship.SVL_NR;
                sLRequest.WFL_ID = ship.WFL_ID;
                sLRequest.WGT_UNT_TE = ship.WGT_UNT_TE;

                shipperlist.Add(sLRequest);
            }
            return shipperlist;
        }

       
    }
}
