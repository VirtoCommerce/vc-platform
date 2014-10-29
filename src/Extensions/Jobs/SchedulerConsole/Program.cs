using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Azure;
using VirtoCommerce.Scheduling.Azure;

namespace VirtoCommerce.Scheduling.Console
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    public class Program
    {
        static void Main()
        {
            new Program().Run();
        }

        private void Run()
        {
            try
            {
                var m = Helper.FormatTrace("Staring the Scheduler", "SchedulerWorkerRole", "Run");
                var traceSource = new TraceSource("VirtoCommerce.ScheduleService.Trace");
                traceSource.TraceEvent(TraceEventType.Information, 0, m);

                var cloudContext = Guid.NewGuid().ToString();

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
    }
}
