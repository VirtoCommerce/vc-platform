using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
    using System.Collections.Concurrent;

    public class ConnectionHelper
    {
        private static ConcurrentDictionary<string,string> _dictionary = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="nameOrConnectionString">The name or connection string.</param>
        /// <returns>connection string</returns>
        public static string GetConnectionString(string nameOrConnectionString)
        {
            if (_dictionary.ContainsKey(nameOrConnectionString))
            {
                return _dictionary[nameOrConnectionString];
            }

            // try getting a settings first
            var settingValue = CloudConfigurationManager.GetSetting(nameOrConnectionString);

            if (String.IsNullOrEmpty(settingValue))
            {
                var connectionStringVal = ConfigurationManager.ConnectionStrings[nameOrConnectionString];

                if (connectionStringVal == null) // we haven't found any setting, so it must be a connection string
                {
                    settingValue = nameOrConnectionString;
                }
                else
                {
                    settingValue = connectionStringVal.ConnectionString;
                }
            }

            // add value to dictionary
            if (!String.IsNullOrEmpty(settingValue))
            {
                _dictionary.TryAdd(nameOrConnectionString, settingValue);
            }

            return settingValue;
        }
    }
}
