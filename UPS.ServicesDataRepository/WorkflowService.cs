using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.UserData;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class WorkflowService : IWorkflowAsync
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        //public WorkflowDataResponse CreateWorkflow(WorkflowDataRequest workflowData)
        //{
        //}

        public WorkflowDataResponse SelectWorkflows(USR user)
        {
            WorkflowDataResponse workflowtDataResponse = new WorkflowDataResponse();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    workflowtDataResponse.Workflows = context.workflowDataRequests.Where(w => w.CRD_BY_NR == user.ID).ToList();
                    workflowtDataResponse.Success = true;
                    return workflowtDataResponse;
                }
                catch (Exception ex)
                {
                    workflowtDataResponse.Success = false;
                    workflowtDataResponse.OperationException = ex;
                }
            }
            return workflowtDataResponse;
        }

        public WorkflowDataResponse InsertWorkflow(WorkflowDataRequest workflowData)
        {
            WorkflowDataResponse workflowtDataResponse = new WorkflowDataResponse();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                    workflowDataRequest.FLE_NA = workflowData.FLE_NA;
                    workflowDataRequest.WFL_STA_TE = workflowData.WFL_STA_TE;
                    workflowDataRequest.CRD_BY_NR = workflowData.CRD_BY_NR;
                    workflowDataRequest.CRD_DT = DateTime.Now;
                    //DateTime.ParseExact(DateTime.Now.ToShortDateString(), "yyyy-MM-dd HH:mm.ss.ffffff", CultureInfo.InvariantCulture);
                    context.workflowDataRequests.Add(workflowDataRequest);
                    context.Entry(workflowDataRequest).State = EntityState.Added;
                    context.SaveChanges();
                    workflowtDataResponse.Workflow = workflowDataRequest;
                    workflowtDataResponse.Success = true;
                    return workflowtDataResponse;
                }
                catch (Exception ex)
                {
                    workflowtDataResponse.Success = true;
                    workflowtDataResponse.OperationException = ex;
                }
            }
            return workflowtDataResponse;
        }

        public WorkflowDataResponse UpdateWorkflowStatusById(WorkflowDataRequest WorkflowDataRequest)
        {
            WorkflowDataResponse workflowDataResponse = new WorkflowDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();   
                optionsBuilder.EnableSensitiveDataLogging(true);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    WorkflowDataRequest data = context.workflowDataRequests.Where(s => s.ID == WorkflowDataRequest.ID).FirstOrDefault();
                    data.ID = WorkflowDataRequest.ID;
                    data.WFL_STA_TE = WorkflowDataRequest.WFL_STA_TE;
                    context.workflowDataRequests.Update(data);
                    context.Entry(WorkflowDataRequest).State = EntityState.Modified;
                    context.SaveChanges();
                    workflowDataResponse.Workflow = data;
                    workflowDataResponse.Success = true;
                    return workflowDataResponse;
                }
            }
            catch (Exception ex)
            {
                workflowDataResponse.Success = false;
                workflowDataResponse.OperationException = ex;
            }
            return workflowDataResponse;
        }

        public WorkflowDataResponse DeleteWorkflowById(int wid)
        {
            WorkflowDataResponse workflowDataResponse = new WorkflowDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.EnableSensitiveDataLogging(true);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    WorkflowDataRequest data = context.workflowDataRequests.Where(s => s.ID == wid).FirstOrDefault();
                    context.workflowDataRequests.Remove(data);
                    context.Entry(data).State = EntityState.Deleted;
                    context.SaveChanges();
                    workflowDataResponse.Success = true;
                    return workflowDataResponse;
                }
            }
            catch (Exception ex)
            {
                workflowDataResponse.Success = false;
                workflowDataResponse.OperationException = ex;
            }
            return workflowDataResponse;
        }
    }
}
