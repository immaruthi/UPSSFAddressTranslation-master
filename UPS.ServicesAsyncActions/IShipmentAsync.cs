using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.Shipment;

namespace UPS.ServicesAsyncActions
{
    public interface IShipmentAsync
    {
        List<ShipmentDataRequest> GetShipment(int workflowID);
        int CreateShipment(ShipmentDataRequest shipmentData);
        ShipmentDataResponse CreateShipments(List<ShipmentDataRequest> shipmentDataRequests);
        ShipmentDataResponse UpdateShipmentStatusById(ShipmentDataRequest shipmentDataRequest);
        ShipmentDataResponse UpdateShipmentAddressById(ShipmentDataRequest shipmentDataRequest);
    }
}
