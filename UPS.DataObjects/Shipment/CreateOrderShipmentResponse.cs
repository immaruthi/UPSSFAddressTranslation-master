using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    public class CreateOrderShipmentResponse
    {
        public List<string> ProcessedShipments { get; set; }

        public List<string> FailedToProcessShipments { get; set; }

        public bool Response { get; set; }

        public Exception exception { get; set; }
    }
}
