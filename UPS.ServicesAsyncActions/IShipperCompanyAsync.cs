using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.Shipment;
using UPS.DataObjects.SPC_LST;

namespace UPS.ServicesAsyncActions
{
    public interface IShipperCompanyAsync
    {
        SFRequest SelectMatchedShipmentsWithShipperCompanies(int workflowID, int userId=0);
        ShipmentDataResponse SelectCompletedShipments(int workflowID);
        ShipperCompanyResponse SelectShipperCompanies();
        ShipperCompanyResponse GetShipperList();
        ShipperCompanyResponse InsertShipper(ShipperCompanyList shipperCompanyRequest);
        ShipperCompanyResponse UpdateShipper(List<ShipperCompanyList> shipperCompanyRequests);
        ShipperCompanyResponse DeleteShipper(List<ShipperCompanyList> shipperCompanyRequests);
        Task<List<string>> GetShipmentCompanyCities();
        
    }
}
