using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Quincus.APP.Request
{
    public class QuincusAddressRequestData
    {
        public class QuincusAddressRequestDataObject
        {
            public int id { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public string address_type_flag { get; set; }
            public string lang { get; set; }
        }


        public class ListQuincusAddressRequestData
        {
            public List<QuincusAddressRequestDataObject> addresses { get; set; }
        }
    }
}
