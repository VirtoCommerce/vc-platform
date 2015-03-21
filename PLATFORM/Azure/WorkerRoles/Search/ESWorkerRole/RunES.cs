using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Diagnostics;
using System.IO;

namespace VirtoCommerce.Azure.WorkerRoles.ElasticSearch
{
    public class RunES : IDisposable
    {
        private static CloudDrive _elasticStorageDrive = null;

        /// <summary>
        /// Function to start ES exe
        /// </summary>
        /// <param name="esLocation"></param>
        /// <param name="fsPort"></param>
        public Process StartES(string esLocation, string fsPort, string workerIPs)
        {
            // create VHD that will contain the instance
            var dataLocation = GetElasticDataDirectory();

            // create storage directories if it is a new instance
            CreateElasticStoragerDirs(dataLocation);
            
            // Call the RunCommand function to change the port in server.xml
            RunCommand(Settings.ElasticSetupCommand, esLocation, fsPort, Settings.ElasticAppRootDir);
            
            // Call the StartTomcatProcess to start the tomcat process
            return StartESProcess(esLocation, dataLocation, workerIPs);
        }

        private string GetElasticDataDirectory()
        {
            DiagnosticsHelper.TraceInformation("Getting db path");
            var roleId = RoleEnvironment.CurrentRoleInstance.Id;
            var containerName = ContainerNameFromRoleId(roleId);

            var dataDrivePath = Utilities.GetMountedPathFromBlob(
                Constants.LocalCacheSetting,
                containerName,
                Constants.ElasticSearchBlobName,
                Settings.DefaultDriveSize,
                out _elasticStorageDrive);

            DiagnosticsHelper.TraceInformation("Obtained data drive as {0}", dataDrivePath);
            return dataDrivePath;
        }

        // follow container naming conventions to generate a unique container name
        private static string ContainerNameFromRoleId(string roleId)
        {
            return roleId.Replace('(', '-').Replace(").", "-").Replace('.', '-').Replace('_', '-').ToLower();
        }

        private void CreateElasticStoragerDirs(String vhdPath)
        {
            DiagnosticsHelper.TraceInformation("ElasticSearch - creating Cache Directories, path=" + vhdPath);

            var elasticStorageDir = Path.Combine(vhdPath, "ElasticStorage");
            var elasticDataDir = Path.Combine(elasticStorageDir, "data");
            DiagnosticsHelper.TraceInformation("ElasticSearch - elasticStorageDir=" + elasticStorageDir);
            DiagnosticsHelper.TraceInformation("ElasticSearch - elasticDataDir=" + elasticDataDir);

            if (Directory.Exists(elasticStorageDir) == false)
            {
                Directory.CreateDirectory(elasticStorageDir);
            }
            if (Directory.Exists(elasticDataDir) == false)
            {
                Directory.CreateDirectory(elasticDataDir);
            }
            DiagnosticsHelper.TraceInformation("ElasticSearch - done creating Cache Directories, path=" + vhdPath);
        }

        /// <summary>
        /// Function to start Tomcat Process
        /// </summary>
        /// <param name="esLocation">The es location.</param>
        /// <param name="cacheLocation">The cache location.</param>
        /// <param name="workerIPs">The worker IP's.</param>
        /// <returns></returns>
        private Process StartESProcess(string esLocation, string cacheLocation, string workerIPs)
        {
            DiagnosticsHelper.TraceInformation("ElasticSearch - starting pricess at esLocation:" + esLocation + ",cacheLocation:" + cacheLocation + ",workerIPs:" + workerIPs);

            // initiating process
            var newProc = new Process();
            
            // stream reader to get the output details of the running command

            try
            {
                //setting the required properties for the process
                newProc.StartInfo.UseShellExecute = false;
                newProc.StartInfo.RedirectStandardOutput = true;
                newProc.StartInfo.RedirectStandardError = true;
                newProc.StartInfo.RedirectStandardOutput = true;
#if DEBUG
                newProc.StartInfo.CreateNoWindow = false;
#endif
#if !DEBUG
                newProc.StartInfo.CreateNoWindow = true;
#endif
                newProc.EnableRaisingEvents = false;

                // setting the localsource path tomcatlocation to the environment variable catalina_home 
                // newProc.StartInfo.EnvironmentVariables.Remove("CATALINA_HOME");  
                // newProc.StartInfo.EnvironmentVariables.Add("CATALINA_HOME", fsLocation.Substring(0, fsLocation.Length - 1));
                newProc.StartInfo.EnvironmentVariables.Remove("JAVA_HOME");  
                newProc.StartInfo.EnvironmentVariables.Add("JAVA_HOME", esLocation.Substring(0, esLocation.Length - 1) + @"\jre7");

                // setting the localsource path tomcatlocation to the environment variable catalina_home 
                newProc.StartInfo.EnvironmentVariables.Add("ES_DATA", Path.Combine(Path.Combine(cacheLocation, "ElasticStorage"), "data"));
                newProc.StartInfo.EnvironmentVariables.Add("ES_HOSTS", workerIPs);
                
                // setting the file name  bin\startup.bat in tomcatlocation of localresourcepath 
                newProc.StartInfo.FileName = esLocation + Settings.ElasticStartApp;

                DiagnosticsHelper.TraceInformation("ElasticSearch start command line: " + esLocation + Settings.ElasticStartApp);
                // starting process
                newProc.Start();
                DiagnosticsHelper.TraceInformation("Done - Starting ElasticSearch");

                newProc.OutputDataReceived += processToExecuteCommand_OutputDataReceived;
                newProc.ErrorDataReceived += processToExecuteCommand_ErrorDataReceived;
                newProc.BeginOutputReadLine();
                newProc.BeginErrorReadLine();
                newProc.Exited += Process_Exited;

            }
            catch (Exception ex)
            {
                // Logging the exceptiom
                DiagnosticsHelper.TraceError(ex.Message);
                throw;
            }

            return newProc;
        }
        /// <summary>
        /// Function to run setupTomcat.bat which in runnin TomcatConfigManager.exe
        /// </summary>
        /// <param name="batchFile"></param>
        /// <param name="esLocation"></param>
        /// <param name="port"></param>
        /// <param name="appRoot"></param>
        /// <returns></returns>
        private string RunCommand(string batchFile, string esLocation, string port, string appRoot)
        {
            // initiating process
            var newProc = new Process();
            
            // initiating stream reader to get the output details
            // string variable to store the output details

            try
            {
                // setting the required properties for the process
                newProc.StartInfo.UseShellExecute = false;
                newProc.StartInfo.RedirectStandardOutput = true;
                newProc.StartInfo.RedirectStandardError = true;
                newProc.StartInfo.RedirectStandardOutput = true;
#if DEBUG
                newProc.StartInfo.CreateNoWindow = false;
#endif
#if !DEBUG
                newProc.StartInfo.CreateNoWindow = true;
#endif
                newProc.EnableRaisingEvents = false;

                var esHome = esLocation.Substring(0, esLocation.Length - 1);
                // setting the localsource path tomcatlocation to the environment variable catalina_home 
                newProc.StartInfo.EnvironmentVariables.Add("ES_HOME", esHome);
                
                // set the batchFile setupTomcat.bat in approot directory as process filename
                newProc.StartInfo.FileName = batchFile;
                
                // set the follwing three arguments for the process {0} catalina_home - environment variable which has the localresource path
                // {1} - port in which the tomcat has to be run and which needs to be changed in the server xml
                // {2} - approot path
                newProc.StartInfo.Arguments = String.Format("{0} {1} \"{2}\"", esHome, port, appRoot);

                DiagnosticsHelper.TraceInformation("Arguments: " + newProc.StartInfo.Arguments);

                // starting the process
                newProc.Start();

                newProc.OutputDataReceived += processToExecuteCommand_OutputDataReceived;
                newProc.ErrorDataReceived += processToExecuteCommand_ErrorDataReceived;
                newProc.BeginOutputReadLine();
                newProc.BeginErrorReadLine();

                newProc.WaitForExit();
                newProc.Close();

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                throw;
            }

            return String.Empty;
        }

        void Process_Exited(object sender, EventArgs e)
        {
            DiagnosticsHelper.TraceInformation("ElasticSearch Exited");
            RoleEnvironment.RequestRecycle();
        }

        private void processToExecuteCommand_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            DiagnosticsHelper.TraceError(e.Data);
        }

        private void processToExecuteCommand_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            DiagnosticsHelper.TraceVerbose(e.Data);
        }

        public void Dispose()
        {
            try
            {
                if (_elasticStorageDrive != null)
                {
                    DiagnosticsHelper.TraceInformation("Unmount called on data drive");
                    _elasticStorageDrive.Unmount();
                }
                DiagnosticsHelper.TraceInformation("Unmount completed on data drive");
            }
            catch (Exception e)
            {
                //Ignore any and all exceptions here
                DiagnosticsHelper.TraceWarning(
                    "Exception in onstop - unmount failed with {0} {1}",
                    e.Message, e.StackTrace);
            }
        }
    }
}
