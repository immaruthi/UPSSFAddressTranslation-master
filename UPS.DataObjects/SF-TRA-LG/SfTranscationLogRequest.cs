using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects
{
    [Table("SF-TRA-LG")]
    public class SfTranscationLogRequest
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("SMT-ID")]
        public int SMT_ID { get; set; }

        [Column("API-LG-ID")]
        public int API_LG_ID { get; set; }

        [Column("SMT-STA")]
        public string SMT_STA { get; set; }
    }
}
