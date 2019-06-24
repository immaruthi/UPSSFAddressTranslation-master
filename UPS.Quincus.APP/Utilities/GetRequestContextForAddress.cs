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
                    quincusAddressRequestData.addresses.Add((new QuincusAddressRequestDataObject()
                    {
                        id = shipmentCollection.id.ToString(),
                        recipient = string.Empty,
                        address = shipmentCollection.rcV_ADR_TE,
                        addressline1 = string.Empty,
                        addressline2 = string.Empty,
                        addressline3 = string.Empty,
                        addressline4 = string.Empty,
                        address_type_flag = bool.TrueString,
                        city = shipmentCollection.dsT_CTY_TE,
                        region = string.Empty,
                        country = "China",
                        lang = "CN"
                    }));
                }

                

                return Newtonsoft.Json.JsonConvert.SerializeObject(quincusAddressRequestData);
            }

            return string.Empty;
        }
    }
}
