using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using VirtoCommerce.Foundation.Data;
using VirtoCommerce.Scheduling;
using VirtoCommerce.Scheduling.Azure;

namespace VirtoCommerce.WorkerRoles
{
    using VirtoCommerce.Foundation.Data.Azure;

    public class SchedulerWorkerRole : RoleEntryPoint
	{
		public override void Run()
		{
			try
			{
				var m = Helper.FormatTrace("Staring the Scheduler", "SchedulerWorkerRole", "Run");
				var traceSource = new TraceSource("VirtoCommerce.ScheduleService.Trace");
				traceSource.TraceEvent(TraceEventType.Information, 0, m);

				string cloudContext;
				if (RoleEnvironment.IsAvailable)
				{
					cloudContext = String.Format("{0}|{1}",
						RoleEnvironment.DeploymentId,
						RoleEnvironment.CurrentRoleInstance.Id);
				}
				else
				{
					cloudContext = Guid.NewGuid().ToString();
				}

				var jobScheduler = new JobScheduler(cloudContext, traceSource, () => AzureConfiguration.Instance.AzureStorageAccount, new Settings());

                traceSource.TraceEvent(TraceEventType.Information, 0, Helper.FormatTrace("Staring the Scheduler", "SchedulerWorkerRole", "Run", "Starting Paralel.Invoke", cloudContext));

				Parallel.Invoke(
					jobScheduler.JobsManagerProcess,
					jobScheduler.SchedulerProcess);
			}
			catch (Exception ex)
			{
				var m = Helper.FormatException(ex, "WorkerRole", "Run");
				var traceSource = new TraceSource("VirtoCommerce.ScheduleService.Trace");
				traceSource.TraceEvent(TraceEventType.Error, 0, m);
			}
		}

		public override bool OnStart()
		{
			ServicePointManager.DefaultConnectionLimit = 12;
			return base.OnStart();
		}
	}
}
