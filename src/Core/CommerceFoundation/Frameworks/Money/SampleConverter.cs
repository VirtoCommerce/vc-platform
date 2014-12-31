using System;
using System.Globalization;

namespace VirtoCommerce.Foundation.Money
{
	public class SampleConverter : ICurrencyConverter
	{

		public double GetRate(CurrencyCodes fromCode, CurrencyCodes toCode, DateTime asOn)
		{
			// Don't use reflection if you want performance!
			return GetRate(Enum.GetName(typeof(CurrencyCodes), fromCode), Enum.GetName(typeof(CurrencyCodes), toCode), asOn);
		}

		public double GetRate(string fromCode, string toCode, DateTime asOn)
		{
			if (toCode.Equals(new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol))
				return 7.9;
			return 0.125;
		}
	}
}
