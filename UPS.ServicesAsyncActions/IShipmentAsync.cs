using ExcelFileRead;
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
        ShipmentDataResponse DeleteShipments(List<ShipmentDataRequest> shipmentDataRequests);
        ShipmentDataResponse DeleteShipment(ShipmentDataRequest shipmentDataRequest);
        ShipmentDataResponse UpdateShipmentAddressById(ShipmentDataRequest shipmentDataRequest);
        int? SelectShipmentTotalStatusByWorkflowId(int wid);
        void UpdateShipmentAddressByIds(List<ShipmentDataRequest> shipmentDataRequest);
        ShipmentDataResponse CreateShipments(List<ExcelDataObject> excelDataObjects, int workflowID);
        string GetShipmentCustomCodesInformation();
    }
}
