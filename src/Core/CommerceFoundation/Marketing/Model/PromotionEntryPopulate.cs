using System;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public class PromotionEntryPopulate : IPromotionEntryPopulate
	{

		#region IPromotionEntryPopulate Members

		/// <summary>
		/// Populates the specified promotion entry with attribute values from the data object. Automatically adds all the meta fields.
		/// The objects supported are LineItem and Entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <param name="data">The data. Can be LineItem or Entry.</param>
		public void Populate(ref PromotionEntry entry, object data)
		{
			var lineItem = data as LineItem;
			var item = data as Item;

			if (lineItem == null && item == null)
				throw new InvalidCastException("This interface only support data types of LineItem or Item");

			if (lineItem != null)
				PopulateLineItem(ref entry, lineItem);
			else
				PopulateItem(ref entry, item);
		}
		#endregion

		/// <summary>
		/// Populates the catalog entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <param name="item">The cat entry.</param>
		private void PopulateItem(ref PromotionEntry entry, Item item)
		{
			entry.Quantity = 1;
			entry.Owner = item;
			entry.CatalogId = item.CatalogId;
			entry.EntryId = item.ItemId;
			entry.EntryCode = item.Code;

			// Save line item id, so it is easier to distinguish to which item discount is applied
			entry["Id"] = item.ItemId;
		}

		/// <summary>
		/// Populates the line item.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <param name="lineItem">The line item.</param>
		private void PopulateLineItem(ref PromotionEntry entry, LineItem lineItem)
		{
			entry.Quantity = lineItem.Quantity;
			entry.Owner = lineItem;
			entry.Outline = lineItem.CatalogOutline;
			entry.EntryId = lineItem.CatalogItemId;
			entry.ParentEntryId = lineItem.ParentCatalogItemId;
			entry.EntryCode = lineItem.CatalogItemCode;

			// Save line item id, so it is easier to distinguish to which item discount is applied
			entry["LineItemId"] = lineItem.LineItemId;
			entry["ShippingAddressId"] = lineItem.ShippingAddressId;
			
			entry["MinQuantity"] = lineItem.MinQuantity;
			entry["MaxQuantity"] = lineItem.MaxQuantity;
			entry["LineItemDiscountAmount"] = lineItem.LineItemDiscountAmount;
			//entry["OrderLevelDiscountAmount"] = lineItem.OrderLevelDiscountAmount;
			entry["ShippingMethodName"] = lineItem.ShippingMethodName ?? string.Empty;
			entry["ExtendedPrice"] = lineItem.ExtendedPrice;
			entry["Description"] = lineItem.Description ?? string.Empty;
			entry["Status"] = lineItem.Status ?? string.Empty;
			entry["DisplayName"] = lineItem.DisplayName ?? string.Empty;
			entry["AllowBackordersAndPreorders"] = lineItem.AllowBackorders && lineItem.AllowPreorders;
			entry["InStockQuantity"] = lineItem.InStockQuantity;
			entry["PreorderQuantity"] = lineItem.PreorderQuantity;
			entry["BackorderQuantity"] = lineItem.BackorderQuantity;
			entry["InventoryStatus"] = lineItem.InventoryStatus;
		}

	}
}
