using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Quincus.APP.Request
{
    public class QuincusGeoCodeDataRequest
    {
        public string endpoint { get; set; }

        public QuincusTokenData quincusTokenData { get; set; }

        public string id { get; set; }
    }
}
