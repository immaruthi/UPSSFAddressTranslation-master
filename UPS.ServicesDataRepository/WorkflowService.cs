using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class WorkflowService : IWorkflowAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        public int CreateWorkflow(WorkflowDataRequest workflowData)
        {
            int workflowid = 0;
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                    workflowDataRequest.FLE_NA = workflowData.FLE_NA;
                    workflowDataRequest.WFL_STA_TE = workflowData.WFL_STA_TE;
                    workflowDataRequest.CRD_BY_NR = workflowData.CRD_BY_NR;
                    //workflowDataRequest.CRD_DT = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "yyyy-MM-dd HH:mm.ss.ffffff", CultureInfo.InvariantCulture);
                    context.workflowDataRequests.Add(workflowDataRequest);
                    context.Entry(workflowDataRequest).State = EntityState.Added;
                    context.SaveChanges();
                    workflowid = workflowDataRequest.ID;
                }
                catch(Exception ex)
                {
                    return workflowid;
                }
            }
            return workflowid;
        }
    }
}
