using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.ADR_ADT_LG
{
    [Table("ADR-ADT-LG")]
    public class AddressAuditLogRequest
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("SMT-ID")]
        public int? SMT_ID { get; set; }
        [Column("CSG-ADR")]
        public string CSG_ADR { get; set; }
        [Column("BFR-ADR")]
        public string BFR_ADR { get; set; }
        [Column("AFR-ADR")]
        public string AFR_ADR { get; set; }
        [Column("UPD-BY")]
        public int? UPD_BY { get; set; }
        [Column("UPD-FRM")]
        public string UPD_FRM { get; set; }
        [Column("UPD-DT")]
        public DateTime? UPD_DT { get; set; }
        [NotMapped]
        [Column("UPD-BY-TE")]
        public string UPD_BY_TE { get; set; }
    }
}
