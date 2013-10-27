using System;
using System.Collections.Generic;
using Quartz;
using VirtoCommerce.Scheduling.LogicalCall;

namespace VirtoCommerce.Scheduling.Windows
{
	public class QuartzJob : IJob
	{
		public void Execute(IJobExecutionContext quartzContext)
		{
			var data = quartzContext.JobDetail.JobDataMap;
			var realization = data["realization"];
			var jobActivity = ((Func<IJobActivity>)realization)();
			var traceContext = (TraceContext)data["context"];
		    var parameters = (IDictionary<string, string>) data["parameters"];

            JobActivityTool.ControlledExecution(jobActivity, traceContext, null, parameters);
		}
	}
}
