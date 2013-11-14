using System.Linq;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Web.Client.Helpers
{
    /// <summary>
    /// Class SettingsHelper.
    /// </summary>
	public class SettingsHelper
	{
        /// <summary>
        /// Gets the settings client.
        /// </summary>
        /// <value>The settings client.</value>
		public static SettingsClient SettingsClient
		{
			get
			{
				return ServiceLocator.Current.GetInstance<SettingsClient>();
			}
		}

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SettingValue[][].</returns>
		public static SettingValue[] GetSettings(string name)
		{
			return SettingsClient.GetSettings(name);
		}

        /// <summary>
        /// Gets a value indicating whether [output cache enabled].
        /// </summary>
        /// <value><c>true</c> if [output cache enabled]; otherwise, <c>false</c>.</value>
        public static bool OutputCacheEnabled
        {
            get
            {
                var retVal = true; // if there is no such setting we assume cache enabled

                var settings = GetSettings("OutputCacheEnabled");

                if (settings != null && settings.Length > 0)
                {
                    retVal = settings.First().BooleanValue;
                }

                return retVal;
            }
        }
	}
}
