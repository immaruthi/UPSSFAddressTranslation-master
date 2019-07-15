using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.SPC_LST
{
    public class ShipperCompanyResponse
    {
        public IEnumerable<ShipperCompanyList> ShipperCompanies { get; set; }
        public ShipperCompanyList ShipperCompany { get; set; }
        public string OperatonExceptionMessage { get; set; }
        public bool Success { get; set; }
    }
}
