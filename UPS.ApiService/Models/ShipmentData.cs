using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtService.Models
{
    public class ShipmentData
    {
        public int SMT_NR_TE { get; set; }
        public string PKG_NR_TE { get; set; }
        public float PKG_WGT_DE { get; set; }
        public float SMT_WGT_DE { get; set; }
        public float DIM_WGT_DE { get; set; }
        public string WGT_UNT_TE { get; set; }
        public string SVL_NR { get; set; }
        public string PY_MT_TE { get; set; }
        public string SHP_DT { get; set; }
        public string PK_UP_TM { get; set; }
        public string BIL_TYP_TE { get; set; }
        public float SMT_VAL_DE { get; set; }
        public string CCY_VAL_TE { get; set; }
        public string FST_INV_LN_DES_TE { get; set; }
        public string EXP_SLC_CD { get; set; }
        public string SHP_NR { get; set; }
        public string SHP_CPY_NA { get; set; }
        public string SHP_ADR_TE { get; set; }
        public string ORG_CTY_TE { get; set; }
        public string ORG_PSL_CD { get; set; }
        public string SHP_CTC_TE { get; set; }
    }
}
