using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.WR_FLW
{
    public class WorkflowDataResponse
    {
        public IEnumerable<WorkflowDataRequest> Workflows { get; set; }
        public WorkflowDataRequest Workflow { get; set; }
        public bool Success { get; set; }
        public Exception OperationException { get; set; }
    }
}
