using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
	public static class PricelistRepositoryExtensions
	{
		public static Price FindLowestPrice(this IPricelistRepository repository, string[] pricelistIds, string itemId, decimal quantity)
		{
			var prices = FindLowestPrices(repository, pricelistIds, new [] { itemId }, quantity);
			var result = prices == null || prices.Length == 0 ? null : prices[0];
			return result;
		}

		public static Price FindLowestPriceDirect(this IPricelistRepository repository, string[] pricelistIds, string itemId, decimal quantity)
		{
			var lists = new List<dynamic>();

			for (int index = 0; index < pricelistIds.Length; index++)
			{
				lists.Add(new { name = pricelistIds[index], order = index });
			}
		
			// get all the prices
			var prices = repository.GetItemPrices(pricelistIds, itemId, quantity);
			// group and sort prices, returning lowest price found in the first price list examined for each item
			var retVal = (from p in prices
						  join l in lists on p.PricelistId.ToLower() equals l.name.ToLower()
						  select new { p, l.order })
							.GroupBy(pl => pl.p.ItemId)
							.Select(p => p.OrderBy(pl => pl.order)
							.ThenBy(x => Math.Min(x.p.Sale.HasValue ? x.p.Sale.Value : x.p.List, x.p.List))
							.First()).Select(p => p.p).FirstOrDefault();

			return retVal;
		}

		/// <summary>
		/// Finds the lowest price for each item using price lists, the price from the 1st price list will be returned, even if it is a higher price.
		/// </summary>
		/// <param name="repository">The repository.</param>
		/// <param name="pricelistIds">The pricelist ids.</param>
		/// <param name="itemIds">The item ids.</param>
		/// <param name="quantity">The quantity.</param>
		/// <returns></returns>
		public static Price[] FindLowestPrices(this IPricelistRepository repository, string[] pricelistIds, string[] itemIds, decimal quantity, bool returnAll = false)
		{
			if (pricelistIds == null || itemIds == null)
				return null;

			// create a dynamic price list object with ordering
			var lists = new List<dynamic>();

			int index = 0;
			foreach (var pl in pricelistIds)
			{
				lists.Add(new { name = pl, order = index });
				index++;
			}

			// get all the prices
			var prices = repository.GetItemPrices(pricelistIds, itemIds, quantity);

			// group and sort prices, returning lowest price found in the first price list examined for each item
		    var sortedPrices = (from p in prices
		        join l in lists on p.PricelistId.ToLower() equals l.name.ToLower()
		        select new { p, l.order })
		        .GroupBy(pl => pl.p.ItemId)
		        .Select(
		            p => p.OrderBy(pl => pl.order)
		                .ThenBy(x => Math.Min(x.p.Sale.HasValue ? x.p.Sale.Value : x.p.List, x.p.List))
		        );

		    Price[] pricesSorted = null;
		    if (returnAll)
		    {
                pricesSorted = sortedPrices.SelectMany(p => p).Select(a => a.p).ToArray();
		    }
		    else
		    {
                pricesSorted = sortedPrices.First().Select(p => p.p).ToArray();
		    }

            return pricesSorted;
		}

		/// <summary>
		/// Gets the item prices. Works from DSClient.
		/// </summary>
		/// <param name="repository">The repository.</param>
		/// <param name="pricelistIds">The pricelist ids.</param>
		/// <param name="itemId">The item id.</param>
		/// <param name="quantity">The quantity.</param>
		/// <returns></returns>
		public static Price[] GetItemPrices(this IPricelistRepository repository, string[] pricelistIds, string itemId, decimal quantity)
		{
		    var retVal = repository.Prices.Where(x => x.ItemId == itemId && (quantity >= x.MinQuantity || quantity == 0)).ToArray();
			if (pricelistIds != null)
			{
				pricelistIds = pricelistIds.Where(x => !String.IsNullOrEmpty(x)).ToArray();
				if (pricelistIds.Any())
				{
					retVal = retVal.Where(x => pricelistIds.Contains(x.PricelistId, StringComparer.OrdinalIgnoreCase)).ToArray();
				}
			}
			return retVal;
		}

		/// <summary>
		/// Gets the item prices. Doesn't work from DSClient.
		/// </summary>
		/// <param name="repository">The repository.</param>
		/// <param name="pricelistIds">The pricelist ids.</param>
		/// <param name="itemIds">The item ids.</param>
		/// <param name="quantity">The quantity.</param>
		/// <returns></returns>
		public static Price[] GetItemPrices(this IPricelistRepository repository, string[] pricelistIds, string[] itemIds, decimal quantity)
		{
		    var retVal = repository.Prices.Where(x => itemIds.Contains(x.ItemId) && (quantity >= x.MinQuantity || quantity == 0)).ToArray();
			if (pricelistIds != null)
			{
				pricelistIds = pricelistIds.Where(x => !String.IsNullOrEmpty(x)).ToArray();
				if (pricelistIds.Any())
				{
					retVal = retVal.Where(x => pricelistIds.Contains(x.PricelistId, StringComparer.OrdinalIgnoreCase)).ToArray();
				}
			}
			return retVal;
		}
	}
}
