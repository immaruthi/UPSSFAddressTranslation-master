using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.Shipment;

namespace UPS.Quincus.APP.Request
{
    public class QuincusAddressTranslationRequest
    {
        public List<ShipmentWorkFlowRequest>  shipmentWorkFlowRequests { get; set; }

        public string token { get; set; }

        public string endpoint { get; set; }

    }
}
