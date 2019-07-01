using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.Shipment;
using UPS.Quincus.APP.Request;
using static UPS.Quincus.APP.Request.QuincusAddressRequestData;

namespace UPS.Quincus.APP.Utilities
{
    public class GetRequestContextForAddress
    {
        public static string GetAddressStringFromRequest(List<ShipmentWorkFlowRequest> shipmentWorkFlowRequests)
        {
            int randomNumber = new Random().Next(1000);
            
            if (shipmentWorkFlowRequests.Count > 0)
            {
                QuincusAddressRequestData.ListQuincusAddressRequestData quincusAddressRequestData = new QuincusAddressRequestData.ListQuincusAddressRequestData();
                quincusAddressRequestData.addresses = new List<QuincusAddressRequestData.QuincusAddressRequestDataObject>();

                foreach (var shipmentCollection in shipmentWorkFlowRequests)
                {
                    QuincusAddressRequestDataObject quincusAddressRequestDataObject = new QuincusAddressRequestDataObject();
                    quincusAddressRequestDataObject.id = shipmentCollection.id.ToString();
                    quincusAddressRequestDataObject.recipient = string.Empty;
                    quincusAddressRequestDataObject.address = shipmentCollection.rcV_ADR_TE;
                    quincusAddressRequestDataObject.addressline1 = string.Empty;
                    quincusAddressRequestDataObject.addressline2 = string.Empty;
                    quincusAddressRequestDataObject.addressline3 = string.Empty;
                    quincusAddressRequestDataObject.addressline4 = string.Empty;
                    quincusAddressRequestDataObject.address_type_flag = bool.TrueString;
                    quincusAddressRequestDataObject.city = shipmentCollection.dsT_CTY_TE;
                    quincusAddressRequestDataObject.region = string.Empty;
                    quincusAddressRequestDataObject.country = "China";
                    quincusAddressRequestDataObject.lang = "CN";

                    quincusAddressRequestData.addresses.Add(quincusAddressRequestDataObject);
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(quincusAddressRequestData);
            }

            return string.Empty;
        }
    }
}
