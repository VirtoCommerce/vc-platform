using System.Data.SqlClient;
using System.Diagnostics;
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

        private static bool? _databaseInstalled = null;
        public static bool IsDatabaseInstalled
        {
            get
            {
                if (_databaseInstalled == null)
                {
                    TestSqlConnection();
                }

                return _databaseInstalled != null && _databaseInstalled.Value;
            }
        }

        public static bool TestSqlConnection()
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    connection.Open();
                    connection.Close();
                    _databaseInstalled = true;
                }
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
                _databaseInstalled = false;
            }

            return _databaseInstalled.Value;
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

        public static void SetConnectionString(string name, string connectionString)
        {
            var configFile = WebConfigurationManager.OpenWebConfiguration("~");
            var section = (ConnectionStringsSection)configFile.GetSection("connectionStrings");
            section.ConnectionStrings[name].ConnectionString = connectionString;
            configFile.Save(ConfigurationSaveMode.Modified);
        }
    }
}
