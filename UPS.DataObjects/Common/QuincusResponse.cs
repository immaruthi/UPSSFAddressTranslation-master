using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Common
{
    public class QuincusResponse
    {
        public List<QuincusReponseData> QuincusReponseDataList { get; set; }
        public QuincusReponseData QuincusReponseData { get; set; }

        public bool ResponseStatus { get; set; }

        public Exception Exception { get; set; }
    }
    public class Address
    {
        public string id { get; set; }
        public string city { get; set; }
        public string lang { get; set; }
        public string region { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string recipient { get; set; }
        public string addressline1 { get; set; }
        public string addressline2 { get; set; }
        public string addressline3 { get; set; }
        public string addressline4 { get; set; }
        public string address_type_flag { get; set; }
    }

    public class Geocode
    {
        public string id { get; set; }
        public string area { get; set; }
        public string city { get; set; }
        public string road { get; set; }
        public string unit { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string accuracy { get; set; }
        public string batch_id { get; set; }
        public string latitude { get; set; }
        public string postcode { get; set; }
        public string longitude { get; set; }
        public string confidence { get; set; }
        public string verify_match { get; set; }
        public string building_name { get; set; }
        public string corrected_city { get; set; }
        public string semantic_check { get; set; }
        public string building_number { get; set; }
        public string corrected_region { get; set; }
        public string corrected_street { get; set; }
        public string corrected_postcode { get; set; }
        public string translated_adddress { get; set; }
        public string corrected_house_number { get; set; }
        public string translation_score_number { get; set; }
    }

    public class QuincusReponseData
    {
        public string batch_id { get; set; }
        public int organisation { get; set; }
        public string status_code { get; set; }
        public IList<Address> addresses { get; set; }
        public IList<Geocode> geocode { get; set; }
        public object geocode_errors { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
    }
}
