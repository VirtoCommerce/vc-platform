using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace VirtoSoftware.ElasticSearch
{
    public class WorkerRole : RoleEntryPoint
    {
        Process _process = null;
        RunES _elastic = null;
        /// <summary>
        /// Run method to start tomcat process in the worker role instances
        /// </summary>
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("ElasticSearch entry point called", "Information");

            Trace.TraceInformation("Configuring Elastic Search");
            ConfigureElasticSearch();
            Trace.TraceInformation("Configured Elastic Search");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");

                try
                {
                    if ((_process != null) && (_process.HasExited == true))
                    {
                        _elastic.Log("ElasticSearch Process Exited. Hence recycling role.", "Information");
                        RoleEnvironment.RequestRecycle();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message + " " + ex.StackTrace);
                }
            }
        }

        private void ConfigureElasticSearch()
        {
            try
            {
                // Initializing RunES
                _elastic = new RunES();

                string workerIPs = ListWorkerRoles();
                Trace.TraceInformation("OnStart workerIPs: " + workerIPs);
                // Calling StartTomcat method to start the tomcat process
                // RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["ElasticSearch"].IPEndpoint.Port.ToString()

                var path = RoleEnvironment.GetLocalResource("ESLocation").RootPath;
                Trace.TraceInformation("OnStart ESLocation: " + path);
                _process = _elastic.StartES(RoleEnvironment.GetLocalResource("ESLocation").RootPath, "9200", workerIPs);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message + " " + ex.StackTrace);
                Trace.Flush();
                throw;
            }
        }

        private string ListWorkerRoles()
        {
            var current = RoleEnvironment.CurrentRoleInstance;
            var endPoints = current.Role.Instances
                            /*.Where(instance => instance != current)*/
                            .Select(instance => instance.InstanceEndpoints["ElasticCloudServiceEndpoint"]);

            if(endPoints == null)
            {
                Trace.WriteLine("no endpoints found");
                return String.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach (RoleInstanceEndpoint endPoint in endPoints)
            {                
                string endPointString = String.Format("{0}:{1}", endPoint.IPEndpoint.Address, endPoint.IPEndpoint.Port);
             
                if (builder.Length > 0)
                    builder.Append(", ");

                builder.Append(endPointString);
                Trace.WriteLine("endpoint:" + endPointString);
            }


            Trace.WriteLine("endpoints:" + builder.ToString());
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
            Trace.WriteLine("ElasticWorkerRole OnStop() called", "Information");

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
                try
                {
                    _elastic.Unmount();
                }
                catch (Exception ex) {
                    Trace.TraceError(ex.Message + " " + ex.StackTrace);
                    Trace.Flush();
                }
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