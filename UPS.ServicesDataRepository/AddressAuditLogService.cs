using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.ADR_ADT_LG;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class AddressAuditLogService : IAddressAuditLogAsync
    {
        private ApplicationDbContext context;
        public AddressAuditLogService(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }

        public List<AddressAuditLogRequest> GetAll()
        {
            return this.context.AddressAuditLogRequests
                    .OrderBy((AddressAuditLogRequest aal) => aal.ID)
                    .ToList();
        }

        public AddressAuditLogResponse Insert(AddressAuditLogRequest addressAuditLogRequest)
        {
            AddressAuditLogResponse addressAuditLogResponse = new AddressAuditLogResponse();
            try
            {
                this.context.AddressAuditLogRequests.Add(addressAuditLogRequest);
                this.context.Entry(addressAuditLogRequest).State = EntityState.Added;
                this.context.SaveChanges();
                addressAuditLogResponse.AddressAuditLogRequestList = this.context.AddressAuditLogRequests.Where(a => a.ID == addressAuditLogRequest.ID);
                addressAuditLogResponse.Success = true;
                return addressAuditLogResponse;
            }
            catch (Exception exception)
            {
                addressAuditLogResponse.Success = false;
                addressAuditLogResponse.OperatonExceptionMessage = exception.Message;
            }
            return addressAuditLogResponse;
        }
    }
}
