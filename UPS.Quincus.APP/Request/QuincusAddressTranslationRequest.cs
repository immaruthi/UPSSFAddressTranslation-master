namespace UPS.Quincus.APP.Request
{
    using System.Collections.Generic;
    using UPS.DataObjects.Shipment;

    public class QuincusAddressTranslationRequest : IQuincusAddressTranslationRequest
    {
        public List<ShipmentWorkFlowRequest> shipmentWorkFlowRequests { get; set; }
        public string token { get; set; }
        public string endpoint { get; set; }
    }
}
