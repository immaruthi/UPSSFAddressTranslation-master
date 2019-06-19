using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    [Table("SMT-DTL-FRM-XL")]
    public class ShipmentDataRequest
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("WFL-ID")]
        public int WFL_ID { get; set; }
        [Column("QQS-TRA-LG-ID")]
        public int? QQS_TRA_LG_ID { get; set; }
        [Column("SF-TRA-LG-ID")]
        public int? SF_TRA_LG_ID { get; set; }
        [Column("SMT-STA-NR")]
        public string SMT_STA_NR { get; set; }
        [Column("SMT-NR-TE")]
        public string SMT_NR_TE { get; set; }
        [Column("PCS-QTY-NR")]
        public string PCS_QTY_NR { get; set; }
        [Column("PKG-NR-TE")]
        public string PKG_NR_TE { get; set; }
        [Column("PKG-WGT-DE")]
        public decimal PKG_WGT_DE { get; set; }
        [Column("SMT-WGT-DE")]
        public decimal SMT_WGT_DE { get; set; }
        [Column("DIM-WGT-DE")]
        public decimal DIM_WGT_DE { get; set; }
        [Column("WGT-UNT-TE")]
        public string WGT_UNT_TE { get; set; }
        [Column("SVL-NR")]
        public string SVL_NR { get; set; }
        [Column("PY-MT-TE")]
        public string PY_MT_TE { get; set; }
        [Column("SHP-DT")]
        public string SHP_DT { get; set; }
        [Column("PK-UP-TM")]
        public string PK_UP_TM { get; set; }
        [Column("BIL-TYP-TE")]
        public string BIL_TYP_TE { get; set; }
        [Column("SMT-VAL-DE")]
        public decimal SMT_VAL_DE { get; set; }
        [Column("CCY-VAL-TE")]
        public string CCY_VAL_TE { get; set; }
        [Column("FST-INV-LN-DES-TE")]
        public string FST_INV_LN_DES_TE { get; set; }
        [Column("EXP-SLC-CD")]
        public string EXP_SLC_CD { get; set; }
        [Column("SHP-NR")]
        public string SHP_NR { get; set; }
        [Column("SHP-CPY-NA")]
        public string SHP_CPY_NA { get; set; }
        [Column("SHP-ADR-TR-TE")]
        public string SHP_ADR_TR_TE { get; set; }
        [Column("SHP-ADR-TE")]
        public string SHP_ADR_TE { get; set; }
        [Column("ORG-CTY-TE")]
        public string ORG_CTY_TE { get; set; }
        [Column("ORG-PSL-CD")]
        public string ORG_PSL_CD { get; set; }
        [Column("SHP-CTC-TE")]
        public string SHP_CTC_TE { get; set; }
        [Column("SHP-PH-TE")]
        public string SHP_PH_TE { get; set; }
        [Column("IMP-SLC-TE")]
        public string IMP_SLC_TE { get; set; }
        [Column("IMP-NR")]
        public string IMP_NR { get; set; }
        [Column("RCV-CPY-TE")]
        public string RCV_CPY_TE { get; set; }
        [Column("RCV-ADR-TE")]
        public string RCV_ADR_TE { get; set; }
        [Column("DST-CTY-TE")]
        public string DST_CTY_TE { get; set; }
        [Column("DST-PSL-TE")]
        public string DST_PSL_TE { get; set; }
        [Column("CSG-CTC-TE")]
        public string CSG_CTC_TE { get; set; }
        [Column("PH-NR")]
        public string PH_NR { get; set; }
        [Column("IN-FLG-TE")]
        public string IN_FLG_TE { get; set; }
        [Column("OU-FLG-TE")]
        public string OU_FLG_TE { get; set; }
        [Column("PYM-MTD")]
        public string PYM_MTD { get; set; }
        [Column("EXP-TYP")]
        public string EXP_TYP { get; set; }
        [Column("COD-TE")]
        public string COD_TE { get; set; }


    }
}
