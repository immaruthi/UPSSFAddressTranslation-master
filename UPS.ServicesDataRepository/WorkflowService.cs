using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPS.DataObjects.UserData;
using UPS.DataObjects.WR_FLW;
using UPS.ServicesAsyncActions;
using UPS.ServicesDataRepository.Common;
using UPS.ServicesDataRepository.DataContext;

namespace UPS.ServicesDataRepository
{
    public class WorkflowService : IWorkflowService
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        private readonly ApplicationDbContext context;
        public WorkflowService(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }
        public WorkflowDataResponse SelectWorkflows(USR user)
        {
            WorkflowDataResponse workflowtDataResponse = new WorkflowDataResponse();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    workflowtDataResponse.Workflows = context.Workflows.Where(w => w.CRD_BY_NR == user.ID).ToList();
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

        public WorkflowDataResponse InsertWorkflow(string fileName, int userId)
        {
            WorkflowDataResponse workflowtDataResponse = new WorkflowDataResponse();
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    Workflow workflowDataRequest = new Workflow();
                    workflowDataRequest.FLE_NA = fileName;
                    workflowDataRequest.CRD_BY_NR = userId;
                    workflowDataRequest.CRD_DT = DateTime.Parse(DateTime.Now.ToString()).ToLocalTime();
                    workflowDataRequest.WFL_STA_TE = 0;
                    workflowDataRequest.CRD_DT = DateTime.Now;
                    context.Workflows.Add(workflowDataRequest);
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

        public WorkflowDataResponse UpdateWorkflowStatusById(Workflow WorkflowDataRequest)
        {
            WorkflowDataResponse workflowDataResponse = new WorkflowDataResponse();
            try
            {
                optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();   
                optionsBuilder.EnableSensitiveDataLogging(true);

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    Workflow data = context.Workflows.Where(s => s.ID == WorkflowDataRequest.ID).FirstOrDefault();
                    if(data != null)
                    {
                        data.ID = WorkflowDataRequest.ID;
                        data.WFL_STA_TE = WorkflowDataRequest.WFL_STA_TE;
                        context.Workflows.Update(data);
                        context.Entry(WorkflowDataRequest).State = EntityState.Detached;
                        context.SaveChanges();
                        workflowDataResponse.Workflow = data;
                        workflowDataResponse.Success = true;
                        return workflowDataResponse;
                    }
                    workflowDataResponse.Success = false;
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
                    Workflow data = context.Workflows.Where(s => s.ID == wid).FirstOrDefault();
                    context.Workflows.Remove(data);
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

        public async Task<List<Workflow>> GetAllWorkFlow()
        {
            List<Workflow> workflows =
                await  context.Workflows
                      .Join(
                          context.UserData,
                          workflow => workflow.CRD_BY_NR,
                          user => user.ID,
                              (workflow, user) => 
                                  new Workflow
                                  {
                                     ID= workflow.ID,
                                     FLE_NA= workflow.FLE_NA,
                                     WFL_STA_TE= workflow.WFL_STA_TE,
                                     CRD_DT =workflow.CRD_DT,
                                     UDT_DT= workflow.UDT_DT,
                                     USR_FST_NA = string.Format("{0} {1}", user.USR_FST_NA,user.USR_LST_NA),
                                     WFL_STA_TE_TEXT = GetWorkflowStatusText(workflow.WFL_STA_TE??0)
                                  }
                              )
                      .Distinct()
                      .OrderByDescending(w => w.ID)
                      .ToListAsync();

            return workflows;
        }

        private string GetWorkflowStatusText(int workFlowStatus)
        {
            switch (workFlowStatus)
            {
                case 0:
                    return Convert.ToString( Enums.WorkflowStatus.Created);
                case 1:
                case 2:
                    return Convert.ToString(Enums.WorkflowStatus.InProgress);
                case 3:
                    return Convert.ToString(Enums.WorkflowStatus.Completed);
                default:
                    return Convert.ToString(Enums.WorkflowStatus.Created);
            }
        }
    }
}
