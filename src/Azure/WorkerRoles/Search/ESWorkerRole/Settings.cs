using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Azure.WorkerRoles.ElasticSearch
{
    using Microsoft.WindowsAzure.ServiceRuntime;

    internal static class Settings
    {
        internal static string DefaultElasticPort = "9200";
        internal static int DefaultDriveSize; // in MB

        internal static string ElasticStartApp = @"bin\elasticsearch.bat";

        internal static string CacheDir = "";

        internal static string ElasticSetupCommand;

        internal static string ElasticAppRootDir;

        static Settings()
        {
            var localCache = RoleEnvironment.GetLocalResource(Constants.LocalCacheSetting);
            DefaultDriveSize = localCache.MaximumSizeInMegabytes;
            CacheDir = localCache.RootPath;
            ElasticSetupCommand = Environment.GetEnvironmentVariable("RoleRoot") + @"\approot\setupElasticSearch.bat";
            ElasticAppRootDir = Environment.GetEnvironmentVariable("RoleRoot") + @"\approot";
        }
    }
}
