namespace UPS.Quincus.APP.Request
{
    using System.Collections.Generic;
    using UPS.DataObjects.Shipment;

    public interface IQuincusAddressTranslationRequest
    {
        List<ShipmentWorkFlowRequest>  shipmentWorkFlowRequests { get; set; }

        string token { get; set; }

        string endpoint { get; set; }
    }
}
