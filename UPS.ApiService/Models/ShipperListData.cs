using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtService.Models
{
    public class ShipperListData
    {
        public int ID { get; set; }
        public string SPC_PSL_CD_TE { get; set; }
        public string SPC_CTY_TE { get; set; }
        public string SPC_CTR_TE { get; set; }
        public string SPC_CPY_TE { get; set; }
        public string SPC_NA { get; set; }
        public string SPC_SND_PTY_CTC_TE { get; set; }
        public string SPC_ADR_TE { get; set; }
        public string SPC_CTC_PH { get; set; }
        public int SPC_SLIC_NR { get; set; }
    }
}
