using System.Collections.Generic;

namespace UPS.Quincus.APP.Request
{
    public class QuincusGeoCodeDataRequest
    {
        public string endpoint { get; set; }

        public QuincusTokenData quincusTokenData { get; set; }

        public List<string> batchIDList { get; set; }
    }
}
