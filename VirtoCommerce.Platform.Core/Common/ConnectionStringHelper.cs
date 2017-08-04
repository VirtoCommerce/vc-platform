using System;
using System.Configuration;

namespace VirtoCommerce.Platform.Core.Common
{
    public class ConnectionStringHelper
    {
        public static string GetConnectionString(string name)
        {
            var environmentConnectionString = Environment.GetEnvironmentVariable($"VIRTO_CONN_STR_{name}");

            return !string.IsNullOrEmpty(environmentConnectionString)
                ? environmentConnectionString
                : ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
        }
    }
}
