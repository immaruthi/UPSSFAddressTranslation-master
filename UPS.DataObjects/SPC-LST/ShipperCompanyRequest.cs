using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.SPC_LST
{
    public class ShipperCompanyList
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("SPC-PSL-CD-TE")]
        public string SPC_PSL_CD_TE { get; set; }

        [Column("SPC-CTY-TE")]
        public string SPC_CTY_TE { get; set; }

        [Column("SPC-CTR-TE")]
        public string SPC_CTR_TE { get; set; }

        [Column("SPC-CPY-TE")]
        public string SPC_CPY_TE { get; set; }

        [Column("SPC-NA")]
        public string SPC_NA { get; set; }

        [Column("SPC-SND-PTY-CTC-TE")]
        public string SPC_SND_PTY_CTC_TE { get; set; }

        [Column("SPC-ADR-TE")]
        public string SPC_ADR_TE { get; set; }

        [Column("SPC-CTC-PH")]
        public string SPC_CTC_PH { get; set; }

        [Column("SPC-SLIC-NR")]
        public int? SPC_SLIC_NR { get; set; }
    }
}
