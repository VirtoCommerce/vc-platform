using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;

namespace VirtoCommerce.Foundation.Data.Azure.Common
{
    /// <summary>
    /// Class BaseWebRole.
    /// </summary>
    public abstract class BaseWebRole : RoleEntryPoint
    {
        /// <summary>
        /// The _warm up task
        /// </summary>
        private WebRoleWarmUp _warmUpTask;

        /// <summary>
        /// Called by Windows Azure to initialize the role instance.
        /// </summary>
        /// <returns>True if initialization succeeds, False if it fails. The default implementation returns True.</returns>
        /// <remarks><para>
        /// Override the OnStart method to run initialization code for your role.
        ///   </para>
        ///   <para>
        /// Before the OnStart method returns, the instance's status is set to Busy and the instance is not available
        /// for requests via the load balancer.
        ///   </para>
        ///   <para>
        /// If the OnStart method returns false, the instance is immediately stopped. If the method
        /// returns true, then Windows Azure starts the role by calling the <see cref="M:Microsoft.WindowsAzure.ServiceRuntime.RoleEntryPoint.Run" /> method.
        ///   </para>
        ///   <para>
        /// A web role can include initialization code in the ASP.NET Application_Start method instead of the OnStart method.
        /// Application_Start is called after the OnStart method.
        ///   </para>
        ///   <para>
        /// Any exception that occurs within the OnStart method is an unhandled exception.
        ///   </para></remarks>
        public override bool OnStart()
        {
            System.Diagnostics.Trace.Listeners.Add(new DiagnosticMonitorTraceListener());
            System.Diagnostics.Trace.TraceInformation("In OnStart");

            ConfigureDiagnosticMonitor();
            _warmUpTask = new WebRoleWarmUp();
            _warmUpTask.Start();
            return base.OnStart();
        }

        /// <summary>
        /// Configures the diagnostic monitor.
        /// </summary>
        private void ConfigureDiagnosticMonitor()
        {
            var diagnosticMonitorConfiguration = DiagnosticMonitor.GetDefaultInitialConfiguration();

            diagnosticMonitorConfiguration.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            diagnosticMonitorConfiguration.Directories.BufferQuotaInMB = 100;

            diagnosticMonitorConfiguration.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            diagnosticMonitorConfiguration.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;

            diagnosticMonitorConfiguration.WindowsEventLog.DataSources.Add("Application!*");
            diagnosticMonitorConfiguration.WindowsEventLog.DataSources.Add("System!*");
            diagnosticMonitorConfiguration.WindowsEventLog.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);

            var performanceCounterConfiguration = new PerformanceCounterConfiguration
                {
                    CounterSpecifier = @"\Processor(_Total)\% Processor Time",
                    SampleRate = TimeSpan.FromSeconds(10d)
                };
            diagnosticMonitorConfiguration.PerformanceCounters.DataSources.Add(performanceCounterConfiguration);
            diagnosticMonitorConfiguration.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);

            // TODO: removed due to reference to the old storage client 
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticMonitorConfiguration);
        }
    }
}
