using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.WR_FLW
{
    [Table("WR-FLW")]
    public class WorkflowDataRequest
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("FLE-NA")]
        public string FLE_NA { get; set; }
        [Column("WFL-STA-TE")]
        public int WFL_STA_TE { get; set; }
        [Column("CRD-DT")]
        public DateTime? CRD_DT { get; set; }
        [Column("UDT-DT")]
        public DateTime? UDT_DT { get; set; }
        [Column("CRD-BY-NR")]
        public int? CRD_BY_NR { get; set; }
        [Column("UDT-BY-NR")]
        public int? UDT_BY_NR { get; set; }

        public string WFL_STA_TE_TEXT { get; set; }
        public string USR_FST_NA { get; set; }
    }

}
