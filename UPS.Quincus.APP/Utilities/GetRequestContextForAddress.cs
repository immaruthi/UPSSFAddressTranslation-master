using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPS.DataObjects.Shipment;
using UPS.Quincus.APP.Request;
using static UPS.Quincus.APP.Request.QuincusAddressRequestData;

namespace UPS.Quincus.APP.Utilities
{
    public class GetRequestContextForAddress
    {
        public static List<string> GetAddressStringFromRequest(List<ShipmentWorkFlowRequest> shipmentWorkFlowRequests)
        {
            List<string> addressesSearlizationList = new List<string>();
            
            if (shipmentWorkFlowRequests.Count > 0)
            {
                QuincusAddressRequestData.ListQuincusAddressRequestData quincusAddressRequestData = new QuincusAddressRequestData.ListQuincusAddressRequestData();
                quincusAddressRequestData.addresses = new List<QuincusAddressRequestData.QuincusAddressRequestDataObject>();

                shipmentWorkFlowRequests.ForEach(Quinc =>
                {
                    QuincusAddressRequestDataObject quincusAddressRequestDataObject = new QuincusAddressRequestDataObject();
                    quincusAddressRequestDataObject.id = Quinc.pkG_NR_TE.ToString();
                    quincusAddressRequestDataObject.recipient = string.Empty;
                    quincusAddressRequestDataObject.address = Quinc.rcV_ADR_TE;
                    quincusAddressRequestDataObject.addressline1 = string.Empty;
                    quincusAddressRequestDataObject.addressline2 = string.Empty;
                    quincusAddressRequestDataObject.addressline3 = string.Empty;
                    quincusAddressRequestDataObject.addressline4 = string.Empty;
                    quincusAddressRequestDataObject.address_type_flag = bool.TrueString;
                    quincusAddressRequestDataObject.city = Quinc.dsT_CTY_TE;
                    quincusAddressRequestDataObject.region = string.Empty;
                    quincusAddressRequestDataObject.country = "China";
                    quincusAddressRequestDataObject.lang = "CN";

                    quincusAddressRequestData.addresses.Add(quincusAddressRequestDataObject);
                });

                

                var getBatchList = quincusAddressRequestData.addresses.ChunkBy<QuincusAddressRequestDataObject>(10);

                
                getBatchList.ForEach(trans =>
                {
                    ListQuincusAddressRequestData listQuincusAddressRequestData = new ListQuincusAddressRequestData();
                    listQuincusAddressRequestData.addresses = trans;
                    addressesSearlizationList.Add(Newtonsoft.Json.JsonConvert.SerializeObject(listQuincusAddressRequestData));
                });
            }

            return addressesSearlizationList;
        }
    }


    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
