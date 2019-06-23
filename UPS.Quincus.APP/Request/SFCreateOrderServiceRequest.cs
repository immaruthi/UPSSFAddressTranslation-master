using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Quincus.APP.Request
{
    public class SFOrderXMLRequest
    {
        public string XMLMessage { get; set; }
    }

    public class SFCreateOrderServiceRequest
    {
        public string RequestOrderXMLMessage { get; set; }

        public string Checkword { get; set; }

        public string AccessNumber { get; set; }

        public string BaseURI { get; set; }

        public string RequestURI { get; set; }

    }

    public class SFCancelOrderServiceRequest
    {
        public string RequestOrderXMLMessage { get; set; }

        public string Checkword { get; set; }

        public string AccessNumber { get; set; }

        public string BaseURI { get; set; }

        public string RequestURI { get; set; }

    }
}
