using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Azure.WorkerRoles.ElasticSearch
{
    using System.Text.RegularExpressions;

    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.WindowsAzure.StorageClient;

    internal static class Utilities
    {

        private static readonly Regex logLevelRegex = new Regex("^(-?)([v]*)$");
        private static string currentRoleName = null;

        static Utilities()
        {
            currentRoleName = RoleEnvironment.CurrentRoleInstance.Role.Name;
        }

        internal static string GetMountedPathFromBlob(
            string localCachePath,
            string containerName,
            string blobName,
            int driveSize,
            out CloudDrive elasticDrive)
        {

            DiagnosticsHelper.TraceInformation(
                "In mounting cloud drive for on {0} with {1}",
                containerName,
                blobName);

            var connectionString = RoleEnvironment.GetConfigurationSettingValue("DataConnectionString");
            connectionString = connectionString.Replace("DefaultEndpointsProtocol=https", "DefaultEndpointsProtocol=http");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            DiagnosticsHelper.TraceInformation("Get container");
            // this should be the name of your replset
            var driveContainer = blobClient.GetContainerReference(containerName);

            // create blob container (it has to exist before creating the cloud drive)
            try
            {
                driveContainer.CreateIfNotExist();
            }
            catch (StorageException e)
            {
                DiagnosticsHelper.TraceInformation(
                    "Container creation failed with {0} {1}",
                    e.Message,
                    e.StackTrace);
            }

            var blobUri = blobClient.GetContainerReference(containerName).GetPageBlobReference(blobName).Uri.ToString();
            DiagnosticsHelper.TraceInformation("Blob uri obtained {0}", blobUri);

            // create the cloud drive
            elasticDrive = storageAccount.CreateCloudDrive(blobUri);
            try
            {
                elasticDrive.CreateIfNotExist(driveSize);
            }
            catch (CloudDriveException e)
            {
                DiagnosticsHelper.TraceInformation(
                    "Drive creation failed with {0} {1}",
                    e.Message,
                    e.StackTrace);

            }

            DiagnosticsHelper.TraceInformation("Initialize cache");
            var localStorage = RoleEnvironment.GetLocalResource(localCachePath);

            CloudDrive.InitializeCache(localStorage.RootPath.TrimEnd('\\'),
                localStorage.MaximumSizeInMegabytes);

            // mount the drive and get the root path of the drive it's mounted as
            try
            {
                DiagnosticsHelper.TraceInformation(
                    "Trying to mount blob as azure drive");
                var driveLetter = elasticDrive.Mount(localStorage.MaximumSizeInMegabytes,
                    DriveMountOptions.None);
                DiagnosticsHelper.TraceInformation(
                    "Write lock acquired on azure drive, mounted as {0}",
                    driveLetter);
                return driveLetter;
            }
            catch (CloudDriveException e)
            {
                DiagnosticsHelper.TraceCritical(
                    "Failed to mount cloud drive with {0} {1}",
                    e.Message,
                    e.StackTrace);
                throw;
            }
        }

        internal static string GetLogVerbosity(string configuredLogLevel)
        {
            string logLevel = null;
            if (!string.IsNullOrEmpty(configuredLogLevel))
            {
                var m = logLevelRegex.Match(configuredLogLevel);
                if (m.Success)
                {
                    logLevel = string.IsNullOrEmpty(m.Groups[1].ToString()) ?
                        "-" + m.Groups[0].ToString() :
                        m.Groups[0].ToString();
                }

            }
            return logLevel;
        }

        internal static bool GetRecycleFlag(string configuredRecycle)
        {
            var recycle = System.String.Compare("true", configuredRecycle.ToLowerInvariant(), System.StringComparison.Ordinal) == 0;
            return recycle;
        }

    }
}
