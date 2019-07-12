using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.ADR_ADT_LG
{
    public class AddressAuditLogResponse
    {
        public IEnumerable<AddressAuditLogRequest> AddressAuditLogRequestList { get; set; }
        public AddressAuditLogRequest AddressAuditLogRequest { get; set; }
        public string OperatonExceptionMessage { get; set; }
        public bool Success { get; set; }

    }
}
