using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Quincus.APP.Response
{
    public class GetSFCreateOrderServiceResponse
    {
        public string OrderResponse { get; set; }

        public bool Response { get; set; }

        public Exception exception { get; set; }
    }

    public class GetSFCancelOrderServiceResponse
    {
        public string OrderResponse { get; set; }

        public bool Response { get; set; }

        public Exception exception { get; set; }
    }
}
