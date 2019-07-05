using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.WR_FLW
{
    public class WorkflowDataResponse
    {
        public IEnumerable<Workflow> Workflows { get; set; }
        public Workflow Workflow { get; set; }
        public bool Success { get; set; }
        public Exception OperationException { get; set; }
    }
}
