using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.UserData;
using UPS.DataObjects.WR_FLW;

namespace UPS.ServicesAsyncActions
{
    public interface IWorkflowService
    {
        WorkflowDataResponse SelectWorkflows(USR user);
        WorkflowDataResponse InsertWorkflow(string fileName, int userId);
        WorkflowDataResponse UpdateWorkflowStatusById(Workflow WorkflowDataRequest);
        Task<List<Workflow>> GetAllWorkFlow();
        WorkflowDataResponse DeleteWorkflowById(int wid);

    }
}
