using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.UserData
{
    public class USR
    {
        public int ID { get; set; }
        public string USR_FST_NA { get; set; }
        public string USR_LST_NA { get; set; }
        public string USR_EML_TE { get; set; }
        public string USR_PWD_TE { get; set; }
        public string USR_PWD_HSH_TE { get; set; }
        public DateTime USR_CRT_DT { get; set; }
        public DateTime USR_UDT_DT { get; set; }
        public int USR_CRT_BY_NR { get; set; }
        public int USR_UDT_BY_NR { get; set; }
        public bool IS_ACT_B { get; set; }
    }
}
