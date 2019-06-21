using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    public class ShipmentWorkFlowRequest
    {
        public int id { get; set; }
        public int wfL_ID { get; set; }
        public int? qqS_TRA_LG_ID { get; set; }
        public int? sF_TRA_LG_ID { get; set; }
        public object smT_STA_NR { get; set; }
        public object smT_NR_TE { get; set; }
        public object pcS_QTY_NR { get; set; }
        public object pkG_NR_TE { get; set; }
        public int pkG_WGT_DE { get; set; }
        public int smT_WGT_DE { get; set; }
        public int diM_WGT_DE { get; set; }
        public object wgT_UNT_TE { get; set; }
        public object svL_NR { get; set; }
        public object pY_MT_TE { get; set; }
        public object shP_DT { get; set; }
        public object pK_UP_TM { get; set; }
        public object biL_TYP_TE { get; set; }
        public int smT_VAL_DE { get; set; }
        public object ccY_VAL_TE { get; set; }
        public object fsT_INV_LN_DES_TE { get; set; }
        public object exP_SLC_CD { get; set; }
        public object shP_NR { get; set; }
        public object shP_CPY_NA { get; set; }
        public object shP_ADR_TR_TE { get; set; }
        public string shP_ADR_TE { get; set; }
        public object orG_CTY_TE { get; set; }
        public object orG_PSL_CD { get; set; }
        public object shP_CTC_TE { get; set; }
        public object shP_PH_TE { get; set; }
        public object imP_SLC_TE { get; set; }
        public object imP_NR { get; set; }
        public object rcV_CPY_TE { get; set; }
        public object rcV_ADR_TE { get; set; }
        public object dsT_CTY_TE { get; set; }
        public object dsT_PSL_TE { get; set; }
        public object csG_CTC_TE { get; set; }
        public object pH_NR { get; set; }
        public object iN_FLG_TE { get; set; }
        public object oU_FLG_TE { get; set; }
        public object pyM_MTD { get; set; }
        public object exP_TYP { get; set; }
        public object coD_TE { get; set; }
    }
}
