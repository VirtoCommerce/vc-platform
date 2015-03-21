using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
namespace VirtoCommerce.PricingModule.Data.Repositories
{
	public interface IFoundationPricingRepository : IPricelistRepository
	{
		foundation.Price GetPriceById(string priceId);
		foundation.Pricelist GetPricelistById(string priceListId);
		foundation.PricelistAssignment GetPricelistAssignmentById(string assignmentId);
		foundation.PricelistAssignment[] GetAllPricelistAssignments(string pricelistId);
	}
}
