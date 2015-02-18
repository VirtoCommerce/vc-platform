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
		Pricelist GetPricelistById(string id);
		Price CreatePrice(Price price);
		Pricelist CreatePricelist(Pricelist priceList);

		void UpdatePrices(Price[] prices);
		void UpdatePricelists(Pricelist[] priceLists);

		void DeletePricelists(string[] ids);
		void DeletePrices(string[] ids);

		IEnumerable<Price> EvaluateProductPrices(PriceEvaluationContext evalContext);
	}
}
