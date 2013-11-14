using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Orders.Model;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Workflow;

namespace VirtoCommerce.Foundation.Orders
{
    [DataContract]
    public class OrderWorkflowResult
    {
        [DataMember]
        public OrderGroup OrderGroup { get; set; }

        [DataMember]
        public WorkflowResult WorkflowResult { get; private set; }

        public OrderWorkflowResult(WorkflowResult workflowResult)
        {
            WorkflowResult = workflowResult;
        }

        public OrderWorkflowResult()
        {
        }
    }
}
