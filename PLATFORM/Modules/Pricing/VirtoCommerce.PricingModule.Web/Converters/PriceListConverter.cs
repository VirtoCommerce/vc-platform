using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using coreCatalogModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.PricingModule.Web.Model;

namespace VirtoCommerce.PricingModule.Web.Converters
{
	public static class PriceListConverter
	{
		public static webModel.Pricelist ToWebModel(this coreModel.Pricelist priceList, coreCatalogModel.CatalogProduct[] products = null)
		{
			var retVal = new webModel.Pricelist();
			retVal.InjectFrom(priceList);
			retVal.Currency = priceList.Currency;
			if (priceList.Prices != null)
			{
				retVal.ProductPrices = new List<webModel.ProductPrice>();
				foreach(var group in priceList.Prices.GroupBy(x=>x.ProductId))
				{
					var productPrice = new webModel.ProductPrice(group.Key, group.Select(x=> x.ToWebModel()));
					
					retVal.ProductPrices.Add(productPrice);
					if (products != null)
					{
						var product = products.FirstOrDefault(x => x.Id == productPrice.ProductId);
						if(product != null)
						{
							productPrice.ProductName = product.Name;
						}
					}
				}
				
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
			if (priceList.ProductPrices != null)
			{
				retVal.Prices = priceList.ProductPrices.SelectMany(x=>x.Prices).Select(x => x.ToCoreModel()).ToList();
			}
			if (priceList.Assignments != null)
			{
				retVal.Assignments = priceList.Assignments.Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}


	}
}
