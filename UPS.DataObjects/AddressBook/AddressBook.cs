using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.AddressBook
{
    [Table("ADR-BK")]
    public class AddressBook
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("SMT-ID")]
        public int? ShipmentId { get; set; }
        [Column("BAT-ID-TE")]

        public string BatchId { get; set; }
        [Column("ORG-TE")]
        [StringLength(50)]
        public string Organization { get; set; }

        [Column("STA-CD-TE")]
        [StringLength(50)]
        public string StatusCode { get; set; }
        [Column("CSG-ADR-ID")]
        public string ConsigneeAddressId { get; set; }
        [Column("CSG-ADR")]
        [StringLength(500)]
        public string ConsigneeAddress { get; set; }
        [Column("CSG-ADR-TR")]
        [StringLength(500)]
        public string ConsigneeTranslatedAddress { get; set; }

        [Column("CSG-AD1-TE")]
        [StringLength(200)]
        public string Address_One { get; set; }
        [Column("CSG-AD2-TE")]
        [StringLength(200)]
        public string Address_Two { get; set; }
        [Column("CSG-AD3-TE")]
        [StringLength(200)]
        public string Address_Three { get; set; }
        [Column("CSG-AD4-TE")]
        [StringLength(200)]

        public string Address_Four { get; set; }
        [Column("CSG-RD-TE")]
        [StringLength(200)]
        public string Road { get; set; }
        [Column("CSG-CTY")]
        [StringLength(50)]
        public string City { get; set; }
        [Column("CSG-RGN-TE")]
        [StringLength(50)]
        public string Region { get; set; }
        [Column("CSG-CUN-TE")]
        [StringLength(50)]
        public string Country { get; set; }
        [Column("CSG-ADR-TYP-FLG-B")]
        public bool? AddressTypeFlag { get; set; }
        [Column("CSG-LNG-TE")]
        [StringLength(50)]
        public string Longitude { get; set; }
        [Column("CSG-LAT-TE")]
        [StringLength(50)]
        public string Latitude { get; set; }
        [Column("GEO-CD-TE")]
        public string GeoCode { get; set; }
        [Column("GEO-CD-ERR-TE")]
        public string GeoCodeError { get; set; }
        [Column("BLD-NR-TE")]
        [StringLength(50)]
        public string BuldingNumber { get; set; }
        [Column("BLD-NA-TE")]
        [StringLength(100)]
        public string BuildingName { get; set; }
        [Column("UN-TE")]
        [StringLength(100)]
        public string Unit { get; set; }
        [Column("AR-TE")]
        [StringLength(50)]
        public string Area { get; set; }
        [Column("BAT-ID")]
        [StringLength(50)]
        public string Bat_Id { get; set; }
        [Column("POS-CD-TE")]
        [StringLength(50)]
        public string PostalCode { get; set; }
        [Column("CON-NR")]
        [StringLength(50)]
        public string Confidence { get; set; }
        [Column("SMC-CHK")]
        [StringLength(50)]
        public string SemanticCheck { get; set; }
        [Column("ACY-TE")]
        [StringLength(50)]
        public string Accuracy { get; set; }
        [Column("VFY-MAT-NR")]
        [StringLength(50)]
        public string VerifyMatch { get; set; }
        [Column("CRD-DT")]
        public DateTime? CreatedDate { get; set; }
        [Column("MDF-DT")]
        public DateTime? ModifiedDate { get; set; }
        [Column("TR-SCR-NR")]
        [StringLength(50)]
        public string TranslationScore{get;set;}

        [Column("CSG-CPY-NA")]
        [StringLength(50)]
        public string ConsigneeCompany { get; set; }
        [NotMapped]
        [Column("WFL-ID")]
        public int? WFL_ID { get; set; }

    }
}
