using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    public class CargoRequest
    {
        public int MST_ID { get; set; }
        public string SMT_NR_TE { get; set; }
        public string FST_INV_LN_DES_TE { get; set; }
        public int? PCS_QTY_NR { get; set; }
        public decimal? PKG_WGT_DE { get; set; }
        public decimal? SMT_VAL_DE { get; set; }
        public decimal? DIM_WGT_DE { get; set; }
    }
}
