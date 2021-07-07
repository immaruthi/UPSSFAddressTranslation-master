using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    public class SFRequest
    {
        public IEnumerable<SFDataRequest> Shipments { get; set; }
        public bool Success { get; set; }
        public string OperationExceptionMsg { get; set; }
        public string BeforeAddress { get; set; }
        public bool HasWorkflow { get; set; }
    }
}
