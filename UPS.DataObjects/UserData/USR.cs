using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.UserData
{
    public class USR
    {
        [Key]
        [Column("ID")]
        public int? ID { get; set; }
        [Column("USR-FST-NA")]
        public string USR_FST_NA { get; set; }
        [Column("USR-LST-NA")]
        public string USR_LST_NA { get; set; }
        [Column("USR-EML-TE")]
        public string USR_EML_TE { get; set; }
        [Column("USR-PWD-TE")]
        public string USR_PWD_TE { get; set; }
        [Column("USR-PWD-HSH-TE")]
        public string USR_PWD_HSH_TE { get; set; }
        [Column("USR-CRT-DT")]
        public DateTime? USR_CRT_DT { get; set; }
        [Column("USR-UDT-DT")]
        public DateTime? USR_UDT_DT { get; set; }
        [Column("USR-CRT-BY-NR")]
        public int? USR_CRT_BY_NR { get; set; }
        [Column("USR-UDT-BY-NR")]
        public int? USR_UDT_BY_NR { get; set; }
        [Column("IS-ACT-B")]
        public bool? IS_ACT_B { get; set; }
        [Column("USR-ID-TE")]
        public string USR_ID_TE { get; set; }
    }
}
