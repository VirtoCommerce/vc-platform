using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using VirtoCommerce.Scheduling.LogicalCall;
using VirtoCommerce.Scheduling.LogicalCall.Configuration;

namespace VirtoCommerce.Scheduling
{
	public static class JobActivityTool
	{
        public static void ControlledExecution(Func<IJobActivity> activityFactory, TraceSource traceSource, Action<string> audit, IDictionary<string, string> parameters)
		{

			var section = ConfigurationManager.GetSection("traceContextConfiguration") ??
						  new TraceContextConfigurationSection();
			var configurator = (TraceContextConfigurationSection)section;
			var activity = activityFactory();
			var contextName = new ContextName(activity.GetType().FullName, "");
			var configuration = configurator.GetDefault(contextName.Service, contextName.Method);
			var traceContext = new TraceContext(configuration, contextName, Guid.NewGuid(), traceSource);
            ControlledExecution(activity, traceContext, audit, parameters);
		}

		public static bool ControlledExecution(IJobActivity activity, TraceContext traceContext, Action<string> audit, IDictionary<string,string> parameters)
		{
			var success = false;
			try
			{
				traceContext.ActivityStart();
                var jobContext = new JobContext(traceContext, parameters);
				activity.Execute(jobContext);
				success = true;
			    if (audit != null)
			    {
			        audit(null);
			    }
			}
			catch (Exception ex)
			{
				traceContext.Error(ex.ToString());
				if (audit != null)
					audit(ex.Message);
			}
			finally
			{
				traceContext.ActivityFinish(success);
				traceContext.FlashTraceBuffer();
			}
			return success;
		}
	}
}
