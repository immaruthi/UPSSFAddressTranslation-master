using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.Shipment;

namespace UPS.ServicesAsyncActions
{
    public interface IShipmentAsync
    {
        int CreateShipment(ShipmentDataRequest shipmentData);
        int CreateShipmentList(List<ShipmentDataRequest> shipmentData);

        int DeleteShipment(ShipmentDataRequest shipmentData);
        int DeleteShipmentList(List<ShipmentDataRequest> shipmentData);

        ShipmentDataResponse GetShipmentData();
        List<ShipmentDataResponse> GetShipmentDataList();

        ShipmentDataResponse UpdateShipment(ShipmentDataRequest shipmentData);
        List<ShipmentDataResponse> UpdateShipmentList(List<ShipmentDataRequest> shipmentData);
    }
}
