using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Pricing.Model;

namespace VirtoCommerce.Domain.Pricing.Services
{
	public interface IPricingService
	{
		Price GetPriceById(string id);
		PriceList GetPriceListById(string id);
		Price CreatePrice(Price price);
		PriceList CreatePriceList(PriceList priceList);

		void UpdatePrices(Price[] prices);
		void UpdatePriceLists(PriceList[] priceLists);

		void DeletePriceLists(string[] ids);
		void DeletePrices(string[] ids);

		IEnumerable<Price> EvaluateProductPrices(PriceEvaluationContext evalContext);
	}
}
