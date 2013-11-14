using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Workflow.Services
{
	public interface IWorkflowService
	{
		WorkflowResult RunWorkflow(string workflowName, Dictionary<string, object> parameters, object[] extensions);
	}
}
