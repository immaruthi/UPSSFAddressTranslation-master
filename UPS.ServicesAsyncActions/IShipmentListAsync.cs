using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.Shipment;

namespace UPS.ServicesAsyncActions
{
    public interface IShipmentListAsync
    {
        List<ShipperListRequest> GetShipmentWithPostalCode(int workflowID);
    }
}
