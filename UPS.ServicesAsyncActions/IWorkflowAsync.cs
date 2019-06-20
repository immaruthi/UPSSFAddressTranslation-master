using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.WR_FLW;

namespace UPS.ServicesAsyncActions
{
    public interface IWorkflowAsync
    {
        int CreateWorkflow(WorkflowDataRequest workflowData);

        //WorkflowDataResponse GetWorkflowData(int shipid);
        //List<WorkflowDataResponse> GetWorkflowDataList(int workid);

    }
}
