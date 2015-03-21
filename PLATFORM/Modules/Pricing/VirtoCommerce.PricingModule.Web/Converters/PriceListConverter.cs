using System.Linq;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using webModel = VirtoCommerce.PricingModule.Web.Model;

namespace VirtoCommerce.PricingModule.Web.Converters
{
	public static class PriceListConverter
	{
		public static webModel.Pricelist ToWebModel(this coreModel.Pricelist priceList)
		{
			var retVal = new webModel.Pricelist();
			retVal.InjectFrom(priceList);
			retVal.Currency = priceList.Currency;
			if (priceList.Prices != null)
			{
				retVal.Prices = priceList.Prices.Select(x => x.ToWebModel()).ToList();
			}
			if(priceList.Assignments != null)
			{
				retVal.Assignments = priceList.Assignments.Select(x => x.ToWebModel()).ToList();
			}
			return retVal;
		}

		public static coreModel.Pricelist ToCoreModel(this webModel.Pricelist priceList)
		{
			var retVal = new coreModel.Pricelist();
			retVal.InjectFrom(priceList);
			retVal.Currency = priceList.Currency;
			if (priceList.Prices != null)
			{
				retVal.Prices = priceList.Prices.Select(x => x.ToCoreModel()).ToList();
			}
			if (priceList.Assignments != null)
			{
				retVal.Assignments = priceList.Assignments.Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}


	}
}
