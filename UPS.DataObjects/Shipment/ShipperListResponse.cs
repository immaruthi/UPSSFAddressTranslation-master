using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Shipment
{
    public class ShipperListResponse
    {
        public IEnumerable<ShipperListRequest> ShipperList { get; set; }
    }
}
