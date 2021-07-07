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
        private readonly ApplicationDbContext context;
        private IAddressBookService addressBookService;
        private IEntityValidationService entityValidationService;
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        //public WorkflowDataResponse CreateWorkflow(WorkflowDataRequest workflowData)
        //{
        //}
        public WorkflowService(ApplicationDbContext applicationDbContext,
            IAddressBookService addressBookService,
            IEntityValidationService entityValidationService)
        {
            this.context = applicationDbContext;
            this.addressBookService = addressBookService;
            this.entityValidationService = entityValidationService;
            this.optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            this.optionsBuilder.EnableSensitiveDataLogging(true);
        }


        public WorkflowDataResponse SelectWorkflows(User user)
        {
            WorkflowDataResponse workflowtDataResponse = new WorkflowDataResponse();
            try
            {
                workflowtDataResponse.Workflows = this.context.workflowDataRequests.Where(w => w.CRD_BY_NR == user.ID).ToList();
                workflowtDataResponse.Success = true;
                return workflowtDataResponse;
            }
            catch (Exception ex)
            {
                workflowtDataResponse.Success = false;
                workflowtDataResponse.OperationException = ex;
            }
            return workflowtDataResponse;
        }


        public WorkflowDataResponse SelectWorkflowById(int Id)
        {
            WorkflowDataResponse workflowtDataResponse = new WorkflowDataResponse();
            try
            {
                workflowtDataResponse.Workflow = this.context.workflowDataRequests.Where(w => w.ID == Id).FirstOrDefault();
                workflowtDataResponse.Success = true;
                return workflowtDataResponse;
            }
            catch (Exception ex)
            {
                workflowtDataResponse.Success = false;
                workflowtDataResponse.OperationException = ex;
            }
            return workflowtDataResponse;
        }

        public WorkflowDataResponse InsertWorkflow(WorkflowDataRequest workflowData)
        {
            WorkflowDataResponse workflowtDataResponse = new WorkflowDataResponse();

            try
            {
                WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                workflowDataRequest.FLE_NA = workflowData.FLE_NA;
                workflowDataRequest.WFL_STA_TE = workflowData.WFL_STA_TE;
                workflowDataRequest.CRD_BY_NR = workflowData.CRD_BY_NR;
                workflowDataRequest.CRD_DT = DateTime.Now;
                //DateTime.ParseExact(DateTime.Now.ToShortDateString(), "yyyy-MM-dd HH:mm.ss.ffffff", CultureInfo.InvariantCulture);
                this.context.workflowDataRequests.Add(workflowDataRequest);
                this.context.Entry(workflowDataRequest).State = EntityState.Added;
                this.context.SaveChanges();
                workflowtDataResponse.Workflow = workflowDataRequest;
                workflowtDataResponse.Success = true;
                return workflowtDataResponse;
            }
            catch (Exception ex)
            {
                workflowtDataResponse.Success = true;
                workflowtDataResponse.OperationException = ex;
            }
            return workflowtDataResponse;
        }

        public WorkflowDataResponse UpdateWorkflowStatusById(WorkflowDataRequest WorkflowDataRequest)
        {
            WorkflowDataResponse workflowDataResponse = new WorkflowDataResponse();
            try
            {
                WorkflowDataRequest data = this.context.workflowDataRequests.Where(s => s.ID == WorkflowDataRequest.ID).FirstOrDefault();
                if (data != null)
                {
                    data.ID = WorkflowDataRequest.ID;
                    data.WFL_STA_TE = WorkflowDataRequest.WFL_STA_TE;
                    this.context.workflowDataRequests.Update(data);
                    this.context.Entry(WorkflowDataRequest).State = EntityState.Detached;
                    this.context.SaveChanges();
                    workflowDataResponse.Workflow = data;
                    workflowDataResponse.Success = true;
                    return workflowDataResponse;
                }
                workflowDataResponse.Success = false;
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
                WorkflowDataRequest data = this.context.workflowDataRequests.Where(s => s.ID == wid).FirstOrDefault();
                this.context.workflowDataRequests.Remove(data);
                this.context.Entry(data).State = EntityState.Deleted;
                this.context.SaveChanges();
                workflowDataResponse.Success = true;
                return workflowDataResponse;
            }
            catch (Exception ex)
            {
                workflowDataResponse.Success = false;
                workflowDataResponse.OperationException = ex;
            }
            return workflowDataResponse;
        }

        public List<WorkflowDataRequest> getExcelData()
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            List<WorkflowDataRequest> workflowDataRequests = new List<WorkflowDataRequest>();
            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var workflowList = from wf in context.workflowDataRequests
                                   join us in context.UserData on (int?)wf.CRD_BY_NR equals us.ID
                                   orderby wf.ID descending
                                   select new
                                   {
                                       wf.ID,
                                       wf.WFL_STA_TE,
                                       wf.FLE_NA,
                                       wf.CRD_BY_NR,
                                       wf.CRD_DT,
                                       wf.UDT_DT,
                                       USR_FST_NA = us.FirstName + " " + us.LastName,
                                   };
                foreach (var wfList in workflowList)
                {
                    WorkflowDataRequest workflowDataRequest = new WorkflowDataRequest();
                    workflowDataRequest.ID = wfList.ID;
                    workflowDataRequest.FLE_NA = wfList.FLE_NA;
                    workflowDataRequest.USR_FST_NA = wfList.USR_FST_NA;
                    workflowDataRequest.CRD_BY_NR = wfList.CRD_BY_NR;
                    workflowDataRequest.CRD_DT = wfList.CRD_DT;
                    workflowDataRequest.WFL_STA_TE = wfList.WFL_STA_TE;
                    if (workflowDataRequest.WFL_STA_TE == 0)
                    {
                        workflowDataRequest.WFL_STA_TE_TEXT = "Created"; //Uploaded
                    }
                    else if (workflowDataRequest.WFL_STA_TE == 1 || workflowDataRequest.WFL_STA_TE == 2)
                    {
                        workflowDataRequest.WFL_STA_TE_TEXT = "InProgress"; //Curated || Translated
                    }
                    else if (workflowDataRequest.WFL_STA_TE == 3)
                    {
                        workflowDataRequest.WFL_STA_TE_TEXT = "Completed"; //Done
                    }
                    workflowDataRequests.Add(workflowDataRequest);
                }
            }

            return workflowDataRequests;
        }
    }
}
