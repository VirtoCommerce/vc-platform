using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	public static class PromotionEvaluationContextExtension
	{
		#region Dynamic expression evaluation helper methods
		public static int GetCartItemsQuantity(this PromotionEvaluationContext context, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			var retVal = context.CartPromoEntries.ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public static int GetCartItemsOfCategoryQuantity(this PromotionEvaluationContext context, string categoryId, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			var retVal = context.CartPromoEntries.InCategories(new[] { categoryId })
								  .ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public static decimal GetCartItemsOfProductQuantity(this PromotionEvaluationContext context, string productId)
		{
			var retVal = context.CartPromoEntries.InProducts(new[] { productId })
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public static decimal GetCartTotalWithExcludings(this PromotionEvaluationContext context, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			var retVal = context.CartPromoEntries.ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .Sum(x => x.Price * x.Quantity);

			return retVal;
		}

		public static bool IsItemInCategory(this PromotionEvaluationContext context, string categoryId, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			return new ProductPromoEntry[] { context.PromoEntry }.InCategories(new[] { categoryId })
							   .ExcludeCategories(excludingCategoryIds)
							   .ExcludeProducts(excludingProductIds)
							   .Any();
		}

		public static bool IsItemCodeContains(this PromotionEvaluationContext context, string code)
		{
			return new ProductPromoEntry[] { context.PromoEntry }.Where(x => String.Equals(code, x.Code, StringComparison.InvariantCultureIgnoreCase)).Any();

		}

		public static bool IsAnyLineItemExtendedTotal(this PromotionEvaluationContext context, decimal lineItemTotal, bool isExactly, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			if (isExactly)
				return context.CartPromoEntries.Where(x => x.Price * x.Quantity == lineItemTotal)
					.ExcludeCategories(excludingCategoryIds)
					.ExcludeProducts(excludingProductIds)
					.Any();
			else
				return context.CartPromoEntries.Where(x => x.Price * x.Quantity >= lineItemTotal)
					.ExcludeCategories(excludingCategoryIds)
					.ExcludeProducts(excludingProductIds)
					.Any();
		}

		public static bool IsItemInProduct(this PromotionEvaluationContext context, string productId)
		{
			return new ProductPromoEntry[] { context.PromoEntry }.InProducts(new[] { productId }).Any();
		}
		#endregion

		#region ProductPromoEntry extensions
		public static IEnumerable<ProductPromoEntry> InCategory(this IEnumerable<ProductPromoEntry> entries, string categoryId)
		{
			var retVal = entries.InCategories(new[] { categoryId });
			return retVal;
		}

		public static IEnumerable<ProductPromoEntry> InCategories(this IEnumerable<ProductPromoEntry> entries, string[] categoryIds)
		{
			categoryIds = categoryIds.Where(x => x != null).ToArray();
			var promotionEntries = entries as ProductPromoEntry[] ?? entries.ToArray();
			return categoryIds.Any() ? promotionEntries.Where(x => IsLineItemInCategories(x, categoryIds)) : promotionEntries;
		}

		public static IEnumerable<ProductPromoEntry> InProduct(this IEnumerable<ProductPromoEntry> entries, string productId)
		{
			var retVal = entries.InProducts(new[] { productId });
			return retVal;
		}

		public static IEnumerable<ProductPromoEntry> InProducts(this IEnumerable<ProductPromoEntry> entries, string[] productIds)
		{
			productIds = productIds.Where(x => x != null).ToArray();
			var promotionEntries = entries as IList<ProductPromoEntry> ?? entries.ToList();
			return productIds.Any() ? promotionEntries.Where(x => IsLineItemInProducts(x, productIds)) : promotionEntries;
		}


		public static IEnumerable<ProductPromoEntry> ExcludeCategories(this IEnumerable<ProductPromoEntry> entries, string[] categoryIds)
		{
			var retVal = entries.Where(x => !IsLineItemInCategories(x, categoryIds));
			return retVal;
		}

		public static IEnumerable<ProductPromoEntry> ExcludeProducts(this IEnumerable<ProductPromoEntry> entries, string[] productIds)
		{
			var retval = entries.Where(x => !IsLineItemInProducts(x, productIds));
			return retval;
		}

		private static bool IsLineItemInCategories(ProductPromoEntry entry, IEnumerable<string> categoryIds)
		{
			var retVal = categoryIds.Contains(entry.CategoryId);
			if (!retVal && entry.Outline != null)
			{
				retVal = entry.Outline.Split(';').Any(x => x == entry.CategoryId);
			}
			return retVal;
		}

		private static bool IsLineItemInProducts(ProductPromoEntry entry, string[] productIds)
		{
			var retVal = productIds.Contains(entry.ProductId, StringComparer.OrdinalIgnoreCase);
			return retVal;
		} 
		#endregion

	}
}
