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
		PricelistAssignment GetPricelistAssignmentById(string id);

		IEnumerable<Pricelist> GetPriceLists();
		IEnumerable<PricelistAssignment> GetPriceListAssignments();

		Price CreatePrice(Price price);
		Pricelist CreatePricelist(Pricelist priceList);
		PricelistAssignment CreatePriceListAssignment(PricelistAssignment assignment);

		void UpdatePrices(Price[] prices);
		void UpdatePricelists(Pricelist[] priceLists);
		void UpdatePricelistAssignments(PricelistAssignment[] assignments);

		void DeletePricelists(string[] ids);
		void DeletePrices(string[] ids);
		void DeletePricelistsAssignments(string[] ids);

		IEnumerable<Price> EvaluateProductPrices(PriceEvaluationContext evalContext);
	}
}
