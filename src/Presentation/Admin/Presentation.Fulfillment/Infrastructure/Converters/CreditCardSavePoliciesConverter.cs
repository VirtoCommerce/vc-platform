using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Converters;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.Infrastructure
{
	public class CreditCardSavePoliciesConverter : EnumToIntConverter<CreditCardSavePolicy>
	{
		private IDictionary<CreditCardSavePolicy, string> textResources;

		public CreditCardSavePoliciesConverter()
		{
			// TODO take localized texts from resources
			textResources = new Dictionary<CreditCardSavePolicy, string>();
			textResources.Add(CreditCardSavePolicy.None, "Do not save");
			textResources.Add(CreditCardSavePolicy.LastFourDigits, "Save last 4 digits");
			textResources.Add(CreditCardSavePolicy.Full, "Save full credit card number");
		}

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object result;
			CreditCardSavePolicy item = (CreditCardSavePolicy)base.Convert(value, targetType, parameter, culture);
			if (targetType == typeof(string))
				result = textResources[item];
			else
				result = item;

			return result;
		}
	}
}
