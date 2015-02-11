using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using webModel = VirtoCommerce.PricingModule.Web.Model;

namespace VirtoCommerce.PricingModule.Web.Converters
{
	public static class PriceListConverter
	{
		public static webModel.PriceList ToWebModel(this coreModel.PriceList priceList)
		{
			var retVal = new webModel.PriceList();
			retVal.InjectFrom(priceList);
			retVal.Currency = priceList.Currency;
			if (priceList.Prices != null)
			{
				retVal.Prices = priceList.Prices.Select(x => x.ToWebModel()).ToList();
			}
			return retVal;
		}

		public static coreModel.PriceList ToCoreModel(this webModel.PriceList priceList)
		{
			var retVal = new coreModel.PriceList();
			retVal.InjectFrom(priceList);
			retVal.Currency = priceList.Currency;
			if (priceList.Prices != null)
			{
				retVal.Prices = priceList.Prices.Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}


	}
}
