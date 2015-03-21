using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace VirtoCommerce.Foundation.Frameworks.Workflow.Providers
{
	public interface  IWFWorkflowActivityProvider
	{
		Activity GetWorkflowActivity(string activityName);
	}
}
