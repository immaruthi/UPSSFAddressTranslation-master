using System;
using System.Collections.Generic;
using System.Text;
using UPS.Quincus.APP.Request;

namespace UPS.Quincus.APP.Response
{
    public class QuincusTokenDataResponse
    {
        public QuincusTokenData quincusTokenData { get; set; }

        public bool ResponseStatus { get; set; }

        public Exception exception { get; set; }

    }
}
