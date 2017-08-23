using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public class ConnectionStringHelper
    {
        [Obsolete("Use ConfigurationHelper.GetConnectionStringValue()")]
        public static string GetConnectionString(string nameOrConnectionString)
        {
            return ConfigurationHelper.GetConnectionStringValue(nameOrConnectionString);
        }
    }
}
