using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.MarketingModule.Data
{

	public class PromotionEvaluationContext : IPromotionEvaluationContext
	{
		public PromotionEvaluationContext()
		{
			IsEveryone = true;
		}

		public string StoreId { get; set; }

		public CurrencyCodes? Currency { get; set; }

		/// <summary>
		/// Customer id
		/// </summary>
		public string CustomerId { get; set; }

		public bool IsRegisteredUser { get; set; }

		/// <summary>
		/// Has user made any orders
		/// </summary>
		public bool IsFirstTimeBuyer { get; set; }

		public bool IsEveryone { get; set; }
		
		/// <summary>
		/// Current shopping cart
		/// </summary>
		public ShoppingCart ShoppingCart { get; set; }

		/// <summary>
		/// Current shipment
		/// </summary>
		public ShipmentMethod Shipment { get; set; }

		/// <summary>
		/// Entered coupon
		/// </summary>
		public string Coupon { get; set; }

		/// <summary>
		/// Product in catalog
		/// </summary>
		public CatalogProduct Product { get; set; }
		public decimal? ProductPrice { get; set; }

		/// <summary>
		/// Selected product quantity
		/// </summary>
		public int? SelectedQuantity { get; set; }

		public ShipmentMethod[] AvailableShipments { get; set; }



		#region Dynamic expression evaluation helper methods
		private ProductPromoEntry[] ProductPromoEntries
		{
			get
			{
				ProductPromoEntry[] retVal = null;
				if(ShoppingCart != null)
				{
					retVal = ShoppingCart.Items.Select(x => new ProductPromoEntry(x)).ToArray();
				}
				else
				{
					retVal = new ProductPromoEntry[] { new ProductPromoEntry(Product, SelectedQuantity ?? 0, ProductPrice ?? 0) };
				}
				return retVal;
			}
		}
		
		public decimal GetItemsQuantity(string[] excludingCategoryIds, string[] excludingProductIds)
		{
			var retVal = ProductPromoEntries.ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public decimal GetItemsOfCategoryQuantity(string categoryId, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			var retVal = ProductPromoEntries.InCategories(new[] { categoryId })
								  .ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public decimal GetItemsOfProductQuantity(string productId)
		{
			var retVal = ProductPromoEntries.InProducts(new[] { productId })
								  .Sum(x => x.Quantity);
			return retVal;
		}

		public decimal GetTotalWithExcludings(string[] excludingCategoryIds, string[] excludingProductIds)
		{
			var retVal = ProductPromoEntries.ExcludeCategories(excludingCategoryIds)
								  .ExcludeProducts(excludingProductIds)
								  .Sum(x => x.Price * x.Quantity);

			return retVal;
		}

		public bool IsItemInCategory(string categoryId, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			return ProductPromoEntries.InCategories(new[] { categoryId })
							   .ExcludeCategories(excludingCategoryIds)
							   .ExcludeProducts(excludingProductIds)
							   .Any();
		}

		public bool IsItemCodeContains(string code)
		{
			return ProductPromoEntries.Where(x => String.Equals(code, x.Code, StringComparison.InvariantCultureIgnoreCase)).Any();

		}

		public bool IsAnyLineItemTotal(decimal lineItemTotal, bool isExactly, string[] excludingCategoryIds, string[] excludingProductIds)
		{
			if (isExactly)
				return ProductPromoEntries.Where(x => x.Price * x.Quantity == lineItemTotal)
					.ExcludeCategories(excludingCategoryIds)
					.ExcludeProducts(excludingProductIds)
					.Any();
			else
				return ProductPromoEntries.Where(x => x.Price * x.Quantity >= lineItemTotal)
					.ExcludeCategories(excludingCategoryIds)
					.ExcludeProducts(excludingProductIds)
					.Any();
		}

		public bool IsItemInProduct(string productId)
		{
			return ProductPromoEntries.InProducts(new[] { productId }).Any();
		} 
		#endregion
	}

	internal class ProductPromoEntry
	{
		public ProductPromoEntry(LineItem lineItem)
		{
			Code = lineItem.ProductCode;
			Quantity = lineItem.Quantity;
			Price = lineItem.PlacedPrice;
			CatalogId = lineItem.CatalogId;
			CategoryId = lineItem.CategoryId;
			ProductId = lineItem.ProductId;
		}
		public ProductPromoEntry(CatalogProduct product, int quantity,  decimal price)
		{
			Code = product.Code;
			Quantity = quantity;
			Price = price;
			CatalogId = product.CatalogId;
			CategoryId = product.CategoryId;
			ProductId = product.Id;
		}

		public string Code { get; set; }
		public decimal Quantity { get; set; }
		public decimal Price { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string ProductId { get; set; }
	}

	internal static class PromotionEvaluationContextExtension
	{
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
			//var retVal = CatalogOutlineBuilder.GetCategoriesHierarchy(entry.Outline).Any(x => categoryIds.Contains(x, StringComparer.OrdinalIgnoreCase));
			return categoryIds.Contains(entry.CategoryId);
		}

		private static bool IsLineItemInProducts(ProductPromoEntry entry, string[] productIds)
		{
			var retVal = productIds.Contains(entry.ProductId, StringComparer.OrdinalIgnoreCase);
			return retVal;
		}

	}


	
}
