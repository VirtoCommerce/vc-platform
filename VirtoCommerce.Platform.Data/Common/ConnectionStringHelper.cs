using System;
using System.Configuration;

namespace VirtoCommerce.Platform.Data.Common
{
    public class ConnectionStringHelper
    {
        public static string GetConnectionString(string connectionStringName)
        {
            var environmentConnectionString = Environment.GetEnvironmentVariable($"VIRTO_CONN_STR_{connectionStringName.ToUpperInvariant()}");

            return !string.IsNullOrEmpty(environmentConnectionString)
                ? environmentConnectionString
                : ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;
        }

        [Obsolete("Use GetConnectionString()")]
        public static string GetConnectionStringName(string connectionStringName)
        {
            var environmentConnectionStringName = Environment.GetEnvironmentVariable($"VIRTO_CONN_STR_NAME_{connectionStringName.ToUpperInvariant()}");

            return !string.IsNullOrEmpty(environmentConnectionStringName)
                ? environmentConnectionStringName
                : connectionStringName;
        }
    }
}
