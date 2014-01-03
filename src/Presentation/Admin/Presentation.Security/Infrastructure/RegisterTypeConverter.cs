using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Converters;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Security.Properties;

namespace VirtoCommerce.ManagementClient.Security.Infrastructure
{
    public class RegisterTypeConverter : EnumToIntConverter<RegisterType>
	{
        private IDictionary<RegisterType, string> textResources;

		public RegisterTypeConverter()
		{
			// TODO take localized texts from resources
            textResources = new Dictionary<RegisterType, string>
                {
                    {RegisterType.Administrator, Resources.Administrator},
                    {RegisterType.GuestUser, Resources.Guest_User},
                    {RegisterType.RegisteredUser, Resources.Registered_User},
                    {RegisterType.SiteAdministrator, Resources.Site_Administrator}
                };
		}

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object result;
            var item = (RegisterType)base.Convert(value, targetType, parameter, culture);
			if (targetType == typeof(string))
				result = textResources[item];
			else
				result = item;

			return result;
		}
	}
}
