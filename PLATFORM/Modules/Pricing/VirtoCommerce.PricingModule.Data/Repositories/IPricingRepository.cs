using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.PricingModule.Data.Model;
using foundation = VirtoCommerce.PricingModule.Data.Model;

namespace VirtoCommerce.PricingModule.Data.Repositories
{
	public interface IPricingRepository : IRepository
	{
		IQueryable<Pricelist> Pricelists { get; }
		IQueryable<Price> Prices { get; }
		IQueryable<PricelistAssignment> PricelistAssignments { get; }

		foundation.Price GetPriceById(string priceId);
		foundation.Pricelist GetPricelistById(string priceListId);
		foundation.PricelistAssignment GetPricelistAssignmentById(string assignmentId);
		foundation.PricelistAssignment[] GetAllPricelistAssignments(string pricelistId);
	}
}
