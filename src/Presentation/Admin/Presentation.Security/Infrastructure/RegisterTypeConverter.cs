using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Converters;
using VirtoCommerce.Foundation.Security.Model;

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
                    {RegisterType.Administrator, "Administrator"},
                    {RegisterType.GuestUser, "Guest User"},
                    {RegisterType.RegisteredUser, "Registered user"},
                    {RegisterType.SiteAdministrator, "Site Administrator"}
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
