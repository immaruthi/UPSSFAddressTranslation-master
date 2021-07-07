using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    public class SFDataRequest
    {
        public int ID { get; set; }
        public int WFL_ID { get; set; }
        public int? QQS_TRA_LG_ID { get; set; }
        public int? SF_TRA_LG_ID { get; set; }
        public int? SMT_STA_NR { get; set; }
        public string SMT_NR_TE { get; set; }
        public int? PCS_QTY_NR { get; set; }
        public string PKG_NR_TE { get; set; }
        public decimal? PKG_WGT_DE { get; set; }
        public decimal? SMT_WGT_DE { get; set; }
        public decimal? DIM_WGT_DE { get; set; }
        public string WGT_UNT_TE { get; set; }
        public string SVL_NR { get; set; }
        public string PY_MT_TE { get; set; }
        public DateTime? SHP_DT { get; set; }
        public DateTime? PK_UP_TM { get; set; }
        public string BIL_TYP_TE { get; set; }
        public decimal? SMT_VAL_DE { get; set; }
        public string CCY_VAL_TE { get; set; }
        public string FST_INV_LN_DES_TE { get; set; }
        public string EXP_SLC_CD { get; set; }
        public string SHP_NR { get; set; }
        public string SHP_CPY_NA { get; set; }
        public string SHP_ADR_TR_TE { get; set; }
        public string SHP_ADR_TE { get; set; }
        public string ORG_CTY_TE { get; set; }
        public string ORG_PSL_CD { get; set; }
        public string SHP_CTC_TE { get; set; }
        public string SHP_PH_TE { get; set; }
        public string IMP_SLC_TE { get; set; }
        public string IMP_NR { get; set; }
        public string RCV_CPY_TE { get; set; }
        public string RCV_ADR_TE { get; set; }
        public string DST_CTY_TE { get; set; }
        public string DST_PSL_TE { get; set; }
        public string CSG_CTC_TE { get; set; }
        public string PH_NR { get; set; }
        public string IN_FLG_TE { get; set; }
        public string OU_FLG_TE { get; set; }
        public string PYM_MTD { get; set; }
        public string EXP_TYP { get; set; }
        public string COD_TE { get; set; }
        public string ACY_TE { get; set; }
        public string CON_NR { get; set; }
        public int? SPC_SLIC_NR { get; set; }
        public string POD_RTN_SVC { get; set; }
        public string SMT_STA_TE { get; set; }
        public string SPC_CST_ID_TE { get; set; }
        public string TranslationScore { get; set; }
        public List<CargoRequest> Cargos { get; set; }
    }
}
