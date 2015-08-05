using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Globalization;

namespace VirtoCommerce.Platform.Core.Common
{
	/// <summary>
	/// http://softwarebydefault.com/code-samples/extending-configurationmanager-get-values-by-type/
	/// </summary>
	public static class ConfigManagerExtensions
	{
		[CLSCompliant(false)]
		public static T GetValue<T>(this NameValueCollection nameValuePairs, string configKey, T defaultValue) where T : IConvertible
		{
			T retValue = default(T);

			if (nameValuePairs.AllKeys.Contains(configKey))
			{
				string tmpValue = nameValuePairs[configKey];

				retValue = (T)Convert.ChangeType(tmpValue, typeof(T));
			}
			else
			{
				return defaultValue;
			}

			return retValue;
		}

		public static TimeSpan GetValue(this NameValueCollection nameValuePairs, string configKey, TimeSpan defaultValue)
		{
			var retValue = defaultValue;

			if (nameValuePairs.AllKeys.Contains(configKey))
			{
				string tmpValue = nameValuePairs[configKey];
   				retValue = TimeSpan.Parse(tmpValue, CultureInfo.InvariantCulture);
			}
			else
			{
				return defaultValue;
			}

			return retValue;
		}
	}
}
