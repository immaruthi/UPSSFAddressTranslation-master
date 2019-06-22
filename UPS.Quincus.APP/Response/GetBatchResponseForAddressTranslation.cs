using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Quincus.APP.Response
{
   
    public class GetBatchResponseForAddressTranslation
    {
        public string batch_id { get; set; }
        public int organisation { get; set; }
        public string status_code { get; set; }
        public List<Address> addresses { get; set; }
        public object geocode { get; set; }
        public object geocode_errors { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
    }
}
