namespace UPS.Quincus.APP.Request
{
    using System.Collections.Generic;

    public class QuincusAddressRequestData
    {
        public class QuincusAddressRequestDataObject
        {
            public string id { get; set; }
            public string recipient { get; set; }
            public string address { get; set; }
            public string addressline1 { get; set; }
            public string addressline2 { get; set; }
            public string addressline3 { get; set; }
            public string addressline4 { get; set; }
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
