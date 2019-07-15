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
        public int? SMT_STA_NR { get; set; }
        [Column("SMT-NR-TE")]
        [StringLength(100)]
        public string SMT_NR_TE { get; set; }
        [Column("PCS-QTY-NR")]
        public int? PCS_QTY_NR { get; set; }
        [Column("PKG-NR-TE")]
        [StringLength(100)]
        public string PKG_NR_TE { get; set; }
        [Column("PKG-WGT-DE")]
        public decimal? PKG_WGT_DE { get; set; }
        [Column("SMT-WGT-DE")]
        
        public decimal? SMT_WGT_DE { get; set; }
        [Column("DIM-WGT-DE")]
        public decimal? DIM_WGT_DE { get; set; }
        [Column("WGT-UNT-TE")]
        [StringLength(10)]
        public string WGT_UNT_TE { get; set; }
        [Column("SVL-NR")]
        [StringLength(10)]
        public string SVL_NR { get; set; }
        [Column("PY-MT-TE")]
        [StringLength(10)]
        public string PY_MT_TE { get; set; }
        [Column("SHP-DT")]
        public DateTime? SHP_DT { get; set; }
        [Column("PK-UP-TM")]
        public DateTime? PK_UP_TM { get; set; }
        [Column("BIL-TYP-TE")]
        [StringLength(10)]
        public string BIL_TYP_TE { get; set; }
        [Column("SMT-VAL-DE")]
        public decimal? SMT_VAL_DE { get; set; }
        [Column("CCY-VAL-TE")]
        [StringLength(10)]
        public string CCY_VAL_TE { get; set; }
        [Column("FST-INV-LN-DES-TE")]
        [StringLength(500)]
        public string FST_INV_LN_DES_TE { get; set; }
        [Column("EXP-SLC-CD")]
        [StringLength(50)]
        public string EXP_SLC_CD { get; set; }
        [Column("SHP-NR")]
        [StringLength(50)]
        public string SHP_NR { get; set; }
        [Column("SHP-CPY-NA")]
        [StringLength(50)]
        public string SHP_CPY_NA { get; set; }
        [Column("SHP-ADR-TR-TE")]
        [StringLength(500)]
        public string SHP_ADR_TR_TE { get; set; }
        [Column("SHP-ADR-TE")]
        [StringLength(500)]
        public string SHP_ADR_TE { get; set; }
        [Column("ORG-CTY-TE")]
        [StringLength(50)]
        public string ORG_CTY_TE { get; set; }
        [Column("ORG-PSL-CD")]
        [StringLength(50)]
        public string ORG_PSL_CD { get; set; }
        [Column("SHP-CTC-TE")]
        [StringLength(50)]
        public string SHP_CTC_TE { get; set; }
        [Column("SHP-PH-TE")]
        public string SHP_PH_TE { get; set; }
        [Column("IMP-SLC-TE")]
        public string IMP_SLC_TE { get; set; }
        [Column("IMP-NR")]
        [StringLength(50)]
        public string IMP_NR { get; set; }
        [Column("RCV-CPY-TE")]
        [StringLength(50)]
        public string RCV_CPY_TE { get; set; }
        [Column("RCV-ADR-TE")]
        [StringLength(500)]
        public string RCV_ADR_TE { get; set; }
        [Column("DST-CTY-TE")]
        [StringLength(50)]
        public string DST_CTY_TE { get; set; }
        [Column("DST-PSL-TE")]
        [StringLength(50)]
        public string DST_PSL_TE { get; set; }
        [Column("CSG-CTC-TE")]
        [StringLength(50)]
        public string CSG_CTC_TE { get; set; }
        [Column("PH-NR")]
        [StringLength(50)]
        public string PH_NR { get; set; }
        [Column("IN-FLG-TE")]
        [StringLength(50)]
        public string IN_FLG_TE { get; set; }
        [Column("OU-FLG-TE")]
        [StringLength(50)]
        public string OU_FLG_TE { get; set; }
        [Column("PYM-MTD")]
        [StringLength(200)]
        public string PYM_MTD { get; set; }
        [Column("EXP-TYP")]
        [StringLength(200)]
        public string EXP_TYP { get; set; }
        [Column("COD-TE")]
        [StringLength(50)]
        public string COD_TE { get; set; }
        [Column("ACY-TE")]
        [StringLength(50)]
        public string ACY_TE { get; set; }
        [Column("CON-NR")]
        [StringLength(50)]
        public string CON_NR { get; set; }
        [Column("SPC-SLIC-NR")]
        public int? SPC_SLIC_NR { get; set; }
        [Column("POD-RTN-SVC")]
        [StringLength(10)]
        public string POD_RTN_SVC { get; set;}
        [NotMapped]
        public string SMT_STA_TE { get; set; }
        [NotMapped]
        public string SPC_CST_ID_TE { get; set; }
    }
}
