using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    public class ShipmentDataResponse
    {
        public IEnumerable<ShipmentDataRequest> Catalogs { get; set; }
        public bool Success { get; set; }
        public Exception OperationException { get; set; }
    }
}
