using System.Web.Configuration;
using Microsoft.WindowsAzure;
using System;
using System.Configuration;

namespace VirtoCommerce.Platform.Data.Common
{
	using System.Collections.Concurrent;

	public class ConnectionHelper
	{
		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <param name="nameOrConnectionString">The name or connection string.</param>
		/// <returns>connection string</returns>
		public static string GetConnectionString(string nameOrConnectionString)
		{
		
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

			return settingValue;
		}

	
	}
}
