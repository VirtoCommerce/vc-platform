#region

using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Threading;
using System.Web.Configuration;

#endregion

namespace VirtoCommerce.ApiClient.Utilities
{

    #region

    #endregion

    public class ConnectionHelper
    {
        #region Static Fields

        private static readonly ConcurrentDictionary<string, string> Dictionary =
            new ConcurrentDictionary<string, string>();

        #endregion

        #region Public Methods and Operators
       /// <summary>
        ///     Gets the connection string.
        /// </summary>
        /// <param name="nameOrConnectionString">The name or connection string.</param>
        /// <returns>connection string</returns>
        public static string GetConnectionString(string nameOrConnectionString)
        {
            if (Dictionary.ContainsKey(nameOrConnectionString))
            {
                return Dictionary[nameOrConnectionString];
            }

            // try getting a settings first
            var settingValue = String.Empty;

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
                Dictionary.TryAdd(nameOrConnectionString, settingValue);
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

        #endregion
    }
}
