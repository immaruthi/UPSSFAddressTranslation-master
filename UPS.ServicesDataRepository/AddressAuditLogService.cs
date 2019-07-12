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
            List<AddressAuditLogRequest> addressAuditLogRequests = new List<AddressAuditLogRequest>();
            var auditListByName = from a in context.AddressAuditLogRequests
                                  join us in context.UserData on (int?)a.UPD_BY equals us.ID
                                  orderby a.ID descending
                                  select new
                                  {
                                      a.ID,
                                      a.SMT_ID,
                                      a.CSG_ADR,
                                      a.BFR_ADR,
                                      a.AFR_ADR,
                                      a.UPD_BY,
                                      a.UPD_FRM,
                                      a.UPD_DT,
                                      UPD_BY_TE = us.USR_FST_NA + " " + us.USR_LST_NA
                                  };
            foreach (var aaloglist in auditListByName)
            {
                AddressAuditLogRequest addrauditlog = new AddressAuditLogRequest();
                addrauditlog.ID = aaloglist.ID;
                addrauditlog.SMT_ID = aaloglist.SMT_ID;
                addrauditlog.CSG_ADR = aaloglist.CSG_ADR;
                addrauditlog.BFR_ADR = aaloglist.BFR_ADR;
                addrauditlog.AFR_ADR = aaloglist.AFR_ADR;
                addrauditlog.UPD_BY = aaloglist.UPD_BY;
                addrauditlog.UPD_FRM = aaloglist.UPD_FRM;
                addrauditlog.UPD_DT = aaloglist.UPD_DT;
                addrauditlog.UPD_BY_TE = aaloglist.UPD_BY_TE;
                addressAuditLogRequests.Add(addrauditlog);
            }

            return addressAuditLogRequests;
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
