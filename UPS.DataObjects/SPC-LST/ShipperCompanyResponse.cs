using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.SPC_LST
{
    public class ShipperCompanyResponse
    {
        public IEnumerable<ShipperCompanyRequest> ShipperCompanies { get; set; }
        public ShipperCompanyRequest ShipperCompany { get; set; }
        public string OperatonExceptionMessage { get; set; }
        public bool Success { get; set; }
    }
}
