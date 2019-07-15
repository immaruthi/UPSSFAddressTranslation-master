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
        ShipmentDataResponse SelectMatchedShipmentsWithShipperCompanies(int workflowID);
        ShipmentDataResponse SelectCompletedShipments(int workflowID);
        ShipperCompanyResponse SelectShipperCompanies();
        ShipperCompanyResponse GetShipperList();
        ShipperCompanyResponse InsertShipper(ShipperCompanyList shipperCompanyRequest);
        ShipperCompanyResponse UpdateShipper(ShipperCompanyList shipperCompanyRequest);
        ShipperCompanyResponse DeleteShipper(ShipperCompanyList shipperCompanyRequest);
        Task<List<string>> GetShipmentCompanyCities();
        
    }
}
