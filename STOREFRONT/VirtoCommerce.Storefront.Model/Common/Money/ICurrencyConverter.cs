using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Storefront.Model.Common
{
	public interface ICurrencyConverter
	{
		double GetRate(CurrencyCodes fromCode, CurrencyCodes toCode, DateTime asOn);
		double GetRate(string fromCode, string toCode, DateTime asOn);
	}
}
