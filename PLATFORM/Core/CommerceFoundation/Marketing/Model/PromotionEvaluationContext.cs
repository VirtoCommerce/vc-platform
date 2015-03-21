using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Catalogs;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public static class PromotionEvaluationContextExtension
	{
		public static IEnumerable<PromotionEntry> InCategory(this IEnumerable<PromotionEntry> lineItems, string categoryId)
		{
			var retVal = lineItems.InCategories(new[] { categoryId });
			return retVal;
		}

		public static IEnumerable<PromotionEntry> InCategories(this IEnumerable<PromotionEntry> lineItems, string[] categoryIds)
		{
			categoryIds = categoryIds.Where(x => x != null).ToArray();
			var promotionEntries = lineItems as PromotionEntry[] ?? lineItems.ToArray();
			return categoryIds.Any() ? promotionEntries.Where(x => IsLineItemInCategories(x, categoryIds)) : promotionEntries;
		}

		public static IEnumerable<PromotionEntry> InProduct(this IEnumerable<PromotionEntry> lineItems, string productId)
		{
			var retVal = lineItems.InProducts(new[] { productId });
			return retVal;
		}

		public static IEnumerable<PromotionEntry> InProducts(this IEnumerable<PromotionEntry> lineItems, string[] productIds)
		{
			productIds = productIds.Where(x => x != null).ToArray();
			var promotionEntries = lineItems as IList<PromotionEntry> ?? lineItems.ToList();
			return productIds.Any() ? promotionEntries.Where(x => IsLineItemInProducts(x, productIds)) : promotionEntries;
		}

		public static IEnumerable<PromotionEntry> OfSku(this IEnumerable<PromotionEntry> lineItems, string skuId)
		{
			var promotionEntries = lineItems as IList<PromotionEntry> ?? lineItems.ToList();
			return skuId != null ? promotionEntries.OfSkus(new[] { skuId }) : promotionEntries;
		}

		public static IEnumerable<PromotionEntry> OfSkus(this IEnumerable<PromotionEntry> lineItems, string[] skuIds)
		{
            var retVal = lineItems.Where(x => skuIds.Contains(x.EntryId, StringComparer.OrdinalIgnoreCase));
			return retVal;
		}

		public static IEnumerable<PromotionEntry> ExcludeCategories(this IEnumerable<PromotionEntry> lineItems, string[] categoryIds)
		{
			var retVal = lineItems.Where(x => !IsLineItemInCategories(x, categoryIds));
			return retVal;
		}

		public static IEnumerable<PromotionEntry> ExcludeProducts(this IEnumerable<PromotionEntry> lineItems, string[] productIds)
		{
			var retval = lineItems.Where(x => !IsLineItemInProducts(x, productIds));
			return retval;
		}

		public static IEnumerable<PromotionEntry> ExcludeSkus(this IEnumerable<PromotionEntry> lineItems, string[] skuIds)
		{
            var retVal = lineItems.Where(x => !skuIds.Contains(x.EntryId, StringComparer.OrdinalIgnoreCase));
			return retVal;
		}

		private static bool IsLineItemInCategories(PromotionEntry lineItem, IEnumerable<string> categoryIds)
		{
            var retVal = CatalogOutlineBuilder.GetCategoriesHierarchy(lineItem.Outline).Any(x => categoryIds.Contains(x, StringComparer.OrdinalIgnoreCase));
			return retVal;
		}

		private static bool IsLineItemInProducts(PromotionEntry lineItem, string[] productIds)
		{
            var retVal = productIds.Contains(lineItem.ParentEntryId, StringComparer.OrdinalIgnoreCase) || productIds.Contains(lineItem.EntryId, StringComparer.OrdinalIgnoreCase);
			return retVal;
		}

	}

	[DataContract]
	public sealed class PromotionEvaluationContext : IPromotionEvaluationContext
	{
		public const string TargetSet = "TARGET";

		/// <summary>
		/// list of gifts from which the user has refused
		/// </summary>
		public string[] RefusedGiftIds { get; set; }

		public string Currency { get; set; }

		public decimal Total
		{
			get
			{
				return TargetEntriesSet.TotalCost;
			}
		}

		private IDictionary<string, object> Context
		{
			get
			{
				return ContextObject as IDictionary<string, object>;
			}
		}

		/// <summary>
		/// Gets or sets the entries.
		/// </summary>
		/// <value>The entries.</value>
		public PromotionEntrySet TargetEntriesSet
		{
			get
			{
				return Context[TargetSet] as PromotionEntrySet;
			}
		}

		private IEnumerable<PromotionEntry> LineItems
		{
			get
			{
				var items = new List<PromotionEntry>();

				if (TargetEntriesSet != null && TargetEntriesSet.Entries != null)
					items.AddRange(TargetEntriesSet.Entries);

				return items.ToArray();
			}
		}

		/// <summary>
		/// Is user currently loged in
		/// </summary>
		public bool IsRegisteredUser { get; set; }

		/// <summary>
		/// Has user made any orders
		/// </summary>
		public bool IsFirstTimeBuyer { get; set; }

		bool _isEveryone = true;
		public bool IsEveryone
		{
			get
			{
				return _isEveryone;
			}
			set
			{
				_isEveryone = value;
			}
		}


		public decimal GetItemsQuantity(string[] excludingCategoryIds, string[] excludingProductIds, string[] excludingSkuIds)
		{
			var retVal = LineItems.ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .ExcludeSkus(excludingSkuIds)
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public decimal GetItemsOfCategoryQuantity(string categoryId, string[] excludingCategoryIds, string[] excludingProductIds, string[] excludingSkuIds)
		{
			var retVal = LineItems.InCategories(new[] { categoryId })
								  .ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .ExcludeSkus(excludingSkuIds)
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public decimal GetItemsOfProductQuantity(string productId, string[] excludingSkuIds)
		{
			var retVal = LineItems.InProducts(new[] { productId })
								  .ExcludeSkus(excludingSkuIds)
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public decimal GetItemsOfSkuQuantity(string skuId)
		{
			var retVal = LineItems.Where(x => x.EntryId == skuId).Sum(x => x.Quantity);
			return retVal;
		}

		public decimal GetTotalWithExcludings(string[] excludingCategoryIds, string[] excludingProductIds, string[] excludingSkuIds)
		{
			var retVal = LineItems.ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .ExcludeSkus(excludingSkuIds)
								  .Sum(x => x.CostPerEntry * x.Quantity);

			return retVal;
		}

		public bool IsItemInCategory(string categoryId, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			return LineItems.InCategories(new[] { categoryId })
							   .ExcludeCategories(excludingCategoryIds)
							   .ExcludeProducts(excludingProductIds)
							   .Any();
		}

		public bool IsItemCodeContains(string skuCode, string[] excludingSkuIds)
		{
			return LineItems.Where(x => x.EntryCode.Contains(skuCode))
				.ExcludeSkus(excludingSkuIds)
				.Any();

		}

		public bool IsAnyLineItemTotal(decimal lineItemTotal, bool isExactly, string[] excludingCategoryIds, string[] excludingProductIds, string[] excludingSkuIds)
		{
			if (isExactly)
				return LineItems.Where(x => x.CostPerEntry * x.Quantity == lineItemTotal)
					.ExcludeCategories(excludingCategoryIds)
					.ExcludeProducts(excludingProductIds)
					.ExcludeSkus(excludingSkuIds)
					.Any();
			else
				return LineItems.Where(x => x.CostPerEntry * x.Quantity >= lineItemTotal)
					.ExcludeCategories(excludingCategoryIds)
					.ExcludeProducts(excludingProductIds)
					.ExcludeSkus(excludingSkuIds)
					.Any();
		}

		public bool IsItemInProduct(string productId)
		{
			return LineItems.InProducts(new[] { productId }).Any();
		}

		#region IPromotionEvaluationContext Members

		public string PromotionType { get; set; }
		public string Store { get; set; }
		public string CouponCode { get; set; }
		public string CustomerId { get; set; }
		public object ContextObject { get; set; }

		#endregion
	}
}
