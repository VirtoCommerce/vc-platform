using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Activities;

namespace VirtoCommerce.Foundation.Frameworks.Workflow.Providers
{
	public abstract class WFActivityProvider : ProviderBase, IWFWorkflowActivityProvider
	{
		#region IWFWorkflowActivityProvider Members

		public abstract Activity GetWorkflowActivity(string activityName);
	
		#endregion
	}
}
