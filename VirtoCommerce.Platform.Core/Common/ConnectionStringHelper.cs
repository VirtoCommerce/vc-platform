using System;
using System.Configuration;

namespace VirtoCommerce.Platform.Core.Common
{
    public class ConnectionStringHelper
    {
        public static string GetConnectionString(string nameOrConnectionString)
        {
            var result = nameOrConnectionString;

            if (nameOrConnectionString.IndexOf('=') < 0)
            {
                result = Environment.GetEnvironmentVariable($"VIRTO_CONN_STR_{nameOrConnectionString}");

                if (string.IsNullOrEmpty(result))
                {
                    result = ConfigurationManager.ConnectionStrings[nameOrConnectionString]?.ConnectionString;
                }
            }

            return result;
        }
    }
}
