using System.Web.Configuration;
using Microsoft.WindowsAzure;
using System;
using System.Configuration;
using VirtoCommerce.Foundation.AppConfig;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
    using System.Collections.Concurrent;

    public class ConnectionHelper
    {
        private static ConcurrentDictionary<string,string> _dictionary = new ConcurrentDictionary<string, string>();
        public static string SqlConnectionString
        {
            get
            {
                return GetConnectionString(AppConfigConfiguration.Instance.Connection.SqlConnectionStringName);
            }
            set
            {
                SetConnectionString(AppConfigConfiguration.Instance.Connection.SqlConnectionStringName, value);
            }
        }

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
            var settingValue = String.Empty;

            // check if we running in azure, since the code below cause EF6 to stop disposing of objects
            //if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("RoleRoot")))
            {
                try
                {
                    settingValue = CloudConfigurationManager.GetSetting(nameOrConnectionString);
                }
                catch
                {
                             
                }
            }

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

        public static void SetConnectionString(string name, string connectionString)
        {
            var configFile = WebConfigurationManager.OpenWebConfiguration("~");
            var section = (ConnectionStringsSection)configFile.GetSection("connectionStrings");
            section.ConnectionStrings[name].ConnectionString = connectionString;
            configFile.Save(ConfigurationSaveMode.Modified);
        }
    }
}
