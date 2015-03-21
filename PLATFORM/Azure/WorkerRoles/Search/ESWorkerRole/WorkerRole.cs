using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace VirtoCommerce.Azure.WorkerRoles.ElasticSearch
{
    public class WorkerRole : RoleEntryPoint
    {
        Process _process;
        RunES _elastic;
        /// <summary>
        /// Run method to start tomcat process in the worker role instances
        /// </summary>
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            DiagnosticsHelper.TraceInformation("ElasticSearch entry point called");

            DiagnosticsHelper.TraceInformation("Configuring Elastic Search");
            ConfigureElasticSearch();
            DiagnosticsHelper.TraceInformation("Configured Elastic Search");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");

                try
                {
                    if ((_process != null) && _process.HasExited)
                    {
                        DiagnosticsHelper.TraceInformation("ElasticSearch Process Exited. Hence recycling role.");
                        RoleEnvironment.RequestRecycle();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    DiagnosticsHelper.TraceError(ex.Message + " " + ex.StackTrace);
                    throw new ApplicationException("Elastic Search Quit: " + ex.Message);
                }
            }
        }

        private void ConfigureElasticSearch()
        {
            try
            {
                // Initializing RunES
                _elastic = new RunES();

                var workerIPs = ListWorkerRoles();
                DiagnosticsHelper.TraceInformation("OnStart workerIPs: " + workerIPs);
                DiagnosticsHelper.TraceInformation("OnStart ESLocation: " + Settings.CacheDir);
                _process = _elastic.StartES(Settings.CacheDir, Settings.DefaultElasticPort, workerIPs);
            }
            catch (Exception ex)
            {
                DiagnosticsHelper.TraceInformation(ex.Message + " " + ex.StackTrace);
                DiagnosticsHelper.TraceError(ex.Message + " " + ex.StackTrace);
                Trace.Flush();
                throw new ApplicationException("Can't configure Elastic Search: " + ex.Message);
            }
        }

        private string ListWorkerRoles()
        {
            var current = RoleEnvironment.CurrentRoleInstance;
            var endPoints = current.Role.Instances
                            .Select(instance => instance.InstanceEndpoints["ElasticCloudServiceEndpoint"]).ToArray();

            if(!endPoints.Any())
            {
                DiagnosticsHelper.TraceInformation("no endpoints found");
                return String.Empty;
            }

            var builder = new StringBuilder();
            foreach (var endPointString in endPoints.Select(endPoint => String.Format("{0}:{1}", endPoint.IPEndpoint.Address, endPoint.IPEndpoint.Port)))
            {
                if (builder.Length > 0)
                    builder.Append(", ");

                builder.Append(endPointString);
                DiagnosticsHelper.TraceInformation("endpoint:" + endPointString);
            }


            DiagnosticsHelper.TraceInformation("endpoints:" + builder);
            return builder.ToString();
        }

        /// <summary>
        /// OnStart() which will start the DiagnosticMonitor
        /// </summary>
        /// <returns></returns>
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        public override void OnStop()
        {
            DiagnosticsHelper.TraceInformation("ElasticWorkerRole OnStop() called", "Information");

            if (_process != null)
            {
                try
                {
                    _process.Kill();
                    _process.WaitForExit(2000);
                }
                catch { }
            }

            if (_elastic != null)
            {
                _elastic.Dispose();
            }

            base.OnStop();
        }

        /// <summary>
        /// This method is to restart the role instance when a configuration setting is changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                RoleEnvironment.RequestRecycle();

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}