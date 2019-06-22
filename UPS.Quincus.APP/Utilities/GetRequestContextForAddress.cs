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
                        address = shipmentCollection.shP_ADR_TE,
                        address_type_flag = bool.TrueString,
                        city = string.Empty,
                        country = string.Empty,
                        id = shipmentCollection.id,
                        lang = "CN",
                        region = string.Empty
                    }));
                }

                

                return Newtonsoft.Json.JsonConvert.SerializeObject(quincusAddressRequestData);
            }

            return string.Empty;
        }
    }
}
