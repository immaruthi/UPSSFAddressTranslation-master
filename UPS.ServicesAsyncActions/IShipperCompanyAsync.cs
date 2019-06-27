using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.SPC_LST;

namespace UPS.ServicesAsyncActions
{
    public interface IShipperCompanyAsync
    {
        ShipmentDataResponse SelectMatchedShipmentsWithShipperCompanies(int workflowID);
        ShipmentDataResponse SelectCompletedShipments(int workflowID);
        ShipperCompanyResponse SelectShipperCompanies();
    }
}
