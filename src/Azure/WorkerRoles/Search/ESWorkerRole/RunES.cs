using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Diagnostics;
using System.IO;

namespace VirtoSoftware.ElasticSearch
{
    public class RunES
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
            string cacheLocation = CreateElasticStorageVhd();

            // create storage directories if it is a new instance
            CreateElasticStoragerDirs(cacheLocation);
            
            // Call the RunCommand function to change the port in server.xml
            var response = RunCommand(Environment.GetEnvironmentVariable("RoleRoot") + @"\approot\setupElasticSearch.bat", esLocation, fsPort, Environment.GetEnvironmentVariable("RoleRoot") + @"\approot");
            
            // Call the StartTomcatProcess to start the tomcat process
            return StartESProcess(esLocation, cacheLocation, workerIPs);
        }

        private String CreateElasticStorageVhd()
        {
            Log("ElasticSearch - creating VHD", "Information");

            var localCache = RoleEnvironment.GetLocalResource("ESLocation");
            Log(String.Format("ESLocation {0} {1} MB", localCache.RootPath, localCache.MaximumSizeInMegabytes - 50), "Information");
            CloudDrive.InitializeCache(localCache.RootPath.TrimEnd('\\'), localCache.MaximumSizeInMegabytes - 50);

            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var client = storageAccount.CreateCloudBlobClient();

            var roleId = RoleEnvironment.CurrentRoleInstance.Id;
            Log(String.Format("Role ID {0}", roleId), "Information");
            var containerAddress = ContainerNameFromRoleId(roleId);
            Log(String.Format("Container {0}", containerAddress), "Information");
            var drives = client.GetContainerReference(containerAddress);

            Log("Creating drives now", "Information");
            try
            {
                drives.CreateIfNotExist();
            }
            catch
            {
                Log("Failed to create drive", "Information");
            };

            /*
            Log("Gettng VHD URL", "Information");
            var vhdUrl = client.GetContainerReference(containerAddress).GetBlobReference("ElasticStorage.vhd").Uri.ToString();
            Log(String.Format("VHD URL {0}", vhdUrl), "Information");
            Log(String.Format("ElasticStorage.vhd {0}", vhdUrl), "Information");
            _elasticStorageDrive = storageAccount.CreateCloudDrive(vhdUrl);
            */
            try
            {
                _elasticStorageDrive = storageAccount.CreateCloudDrive(String.Format("{0}/{1}", containerAddress, "ElasticStorage.vhd"));
            }
            catch (Exception ex)
            {
                Log(String.Format("{0}:{1}", ex.Message, ex.StackTrace), "Information");
                //throw;
            }
            Log(String.Format("ElasticStorage.vhd {0}", containerAddress), "Information");

            int cloudDriveSizeInMb = int.Parse(RoleEnvironment.GetConfigurationSettingValue("CloudDriveSize"));
            try { _elasticStorageDrive.Create(cloudDriveSizeInMb); }
            catch (CloudDriveException) { }

            Log(String.Format("CloudDriveSize {0} MB", cloudDriveSizeInMb), "Information");

            //var dataPath = _elasticStorageDrive.Mount(localCache.MaximumSizeInMegabytes - 50, DriveMountOptions.Force);
            var dataPath = _elasticStorageDrive.Mount(localCache.MaximumSizeInMegabytes - 50, DriveMountOptions.Force);
            Log(String.Format("Mounted as {0}", dataPath), "Information");

            Log("ElasticSearch - created VHD", "Information");

            return dataPath;
        }

        // follow container naming conventions to generate a unique container name
        private static string ContainerNameFromRoleId(string roleId)
        {
            return roleId.Replace('(', '-').Replace(").", "-").Replace('.', '-').Replace('_', '-').ToLower();
        }

        private void CreateElasticStoragerDirs(String vhdPath)
        {
            Log("ElasticSearch - creating Cache Directories, path="+vhdPath, "Information");

            var elasticStorageDir = Path.Combine(vhdPath, "ElasticStorage");
            var elasticDataDir = Path.Combine(elasticStorageDir, "data");
            Log("ElasticSearch - elasticStorageDir=" + elasticStorageDir, "Information");
            Log("ElasticSearch - elasticDataDir=" + elasticDataDir, "Information");

            if (Directory.Exists(elasticStorageDir) == false)
            {
                Directory.CreateDirectory(elasticStorageDir);
            }
            if (Directory.Exists(elasticDataDir) == false)
            {
                Directory.CreateDirectory(elasticDataDir);
            }
            Log("ElasticSearch - done creating Cache Directories, path=" + vhdPath, "Information");
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
            Log("ElasticSearch - starting pricess at esLocation:" + esLocation + ",cacheLocation:" + cacheLocation + ",workerIPs:" + workerIPs, "Information");

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
                newProc.StartInfo.FileName = esLocation + @"bin\elasticsearch.bat";
                 
                Log("ElasticSearch start command line: " + esLocation + @"bin\elasticsearch.bat", "Information");
                // starting process
                newProc.Start();
                Log("Done - Starting ElasticSearch", "Information");

                newProc.OutputDataReceived += processToExecuteCommand_OutputDataReceived;
                newProc.ErrorDataReceived += processToExecuteCommand_ErrorDataReceived;
                newProc.BeginOutputReadLine();
                newProc.BeginErrorReadLine();
                newProc.Exited += Process_Exited;

                // getting the process output
                //sr = newProc.StandardOutput;

                // storing the output in the string variable
                //returnDetails = sr.ReadToEnd();

                // Logging the output details
                //Trace.TraceInformation("Information", returnDetails);  

            }
            catch (Exception ex)
            {
                // Logging the exceptiom
                Trace.TraceError(ex.Message);
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

                string esHome = esLocation.Substring(0, esLocation.Length - 1);
                // setting the localsource path tomcatlocation to the environment variable catalina_home 
                newProc.StartInfo.EnvironmentVariables.Add("ES_HOME", esHome);
                
                // set the batchFile setupTomcat.bat in approot directory as process filename
                newProc.StartInfo.FileName = batchFile;
                
                // set the follwing three arguments for the process {0} catalina_home - environment variable which has the localresource path
                // {1} - port in which the tomcat has to be run and which needs to be changed in the server xml
                // {2} - approot path
                newProc.StartInfo.Arguments = String.Format("{0} {1} \"{2}\"", esHome, port, appRoot);

                Log("Arguments: " + newProc.StartInfo.Arguments, "Information");

                // starting the process
                newProc.Start();

                //getting the output details
                //sr = newProc.StandardOutput;

                // storing the output details in the string variable
                //returnDetails = sr.ReadToEnd();
               
                // Logging the output details
                //Log(returnDetails, "Information");

                newProc.OutputDataReceived += processToExecuteCommand_OutputDataReceived;
                newProc.ErrorDataReceived += processToExecuteCommand_ErrorDataReceived;
                newProc.BeginOutputReadLine();
                newProc.BeginErrorReadLine();

                newProc.WaitForExit();
                //returnDetails = sr.ReadToEnd();
                newProc.Close();

                /*
                returnDetails = string.Empty;
                
                //setting the serverxml Configpath
                string serverConfigPath = fsLocation + @"conf\server.xml"; 

                //Calling ConfigTomcatPort method to configure the port in server.xml
                ConfigTomcatPort(serverConfigPath,port);  
                 * */

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
            Log("ElasticSearch Exited", "Information");
            RoleEnvironment.RequestRecycle();
        }

/*
        private Process ExecuteShellCommand(String command, bool waitForExit, String workingDir = null)
        {
            var processToExecuteCommand = new Process();

            processToExecuteCommand.StartInfo.FileName = "cmd.exe";
            if (workingDir != null)
            {
                processToExecuteCommand.StartInfo.WorkingDirectory = workingDir;
            }

            processToExecuteCommand.StartInfo.Arguments = @"/C " + command;
            processToExecuteCommand.StartInfo.RedirectStandardInput = true;
            processToExecuteCommand.StartInfo.RedirectStandardError = true;
            processToExecuteCommand.StartInfo.RedirectStandardOutput = true;
            processToExecuteCommand.StartInfo.UseShellExecute = false;
            processToExecuteCommand.StartInfo.CreateNoWindow = true;
            processToExecuteCommand.EnableRaisingEvents = false;
            processToExecuteCommand.Start();

            processToExecuteCommand.OutputDataReceived += new DataReceivedEventHandler(processToExecuteCommand_OutputDataReceived);
            processToExecuteCommand.ErrorDataReceived += new DataReceivedEventHandler(processToExecuteCommand_ErrorDataReceived);
            processToExecuteCommand.BeginOutputReadLine();
            processToExecuteCommand.BeginErrorReadLine();

            if (waitForExit == true)
            {
                processToExecuteCommand.WaitForExit();
                processToExecuteCommand.Close();
                processToExecuteCommand.Dispose();
                processToExecuteCommand = null;
            }

            return processToExecuteCommand;
        }
*/

        private void processToExecuteCommand_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log(e.Data, "Message");
        }

        private void processToExecuteCommand_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log(e.Data, "Message");
        }

        public void Unmount()
        {
            if (_elasticStorageDrive != null)
            {
                try
                {
                    _elasticStorageDrive.Unmount();
                }
                catch { }
            }
        }

        public void Log(string message, string category)
        {
            message = RoleEnvironment.CurrentRoleInstance.Id + "=> " + message;

            /*
            try
            {
                if (String.IsNullOrWhiteSpace(_logFileLocation) == false)
                {
                    File.AppendAllText(_logFileLocation, String.Concat(message, Environment.NewLine));
                }
            }
            catch
            { }
             * */

            Trace.WriteLine(message, category);
        }

/*
        /// <summary>
        /// ConfigTomcatPort method is to configure the port in server.xml
        /// </summary>
        /// <param name="serverConfigPath">Full path location of server.xml</param>
        /// <param name="newPort">New value for connector</param>
        private void ConfigTomcatPort(string serverConfigPath, string newPort)
        {
            try
            {
                XmlDocument config = new XmlDocument();

                // Load \conf\server.xml
                config.Load(serverConfigPath);

                Trace.TraceInformation("Original Value = {0}", config.SelectSingleNode("/Server/Service/Connector").Attributes["port"].Value);

                //Change the port with the new port Arg 1
                config.SelectSingleNode("/Server/Service/Connector").Attributes["port"].Value = newPort;

                Trace.TraceInformation("Updating Server.xml...");

                config.Save(serverConfigPath);

                Trace.TraceInformation("Server.xml updated...");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.StackTrace);
            }
        }
*/

    }
}
