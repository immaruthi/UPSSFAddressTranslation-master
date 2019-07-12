using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtService.Models
{
    public class AddressBookData
    {
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string BatchId { get; set; }
        public string Organization { get; set; }
        public string StatusCode { get; set; }
        public int ConsigneeAddressId { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeTranslatedAddress { get; set; }
        public string Address_One { get; set; }
        public string Address_Two { get; set; }
        public string Address_Three { get; set; }
        public string Address_Four { get; set; }
        public string Road { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public bool AddressTypeFlag { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string GeoCode { get; set; }
        public string GeoCodeError { get; set; }
        public string BuldingNumber { get; set; }
        public string BuildingName { get; set; }
        public string Unit { get; set; }
        public string Area { get; set; }
        public string Bat_Id { get; set; }
        public string PostalCode { get; set; }
        public string Confidence { get; set; }
        public string SemanticCheck { get; set; }
        public string Accuracy { get; set; }
        public string VerifyMatch { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
