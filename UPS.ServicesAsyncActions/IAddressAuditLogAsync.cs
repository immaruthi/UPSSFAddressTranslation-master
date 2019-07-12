using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.ADR_ADT_LG;

namespace UPS.ServicesAsyncActions
{
    public interface IAddressAuditLogAsync
    {
        AddressAuditLogResponse Insert(AddressAuditLogRequest addressAuditLogRequest);
        List<AddressAuditLogRequest> GetAll();
    }
}
