using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Workflow
{
    public class WorkflowResult
    {
        public List<WorkflowMessage> Warnings { get; set; }

        public WorkflowResult()
        {
            Warnings = new List<WorkflowMessage>();
        }
    }

    public class WorkflowMessage
    {
        public string Code { get; set; }
        public IDictionary<string, string> Parameters { get; set; }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            WorkflowMessage other = obj as WorkflowMessage;
            return other != null && Code.Equals(other.Code);
        }
    }
}
