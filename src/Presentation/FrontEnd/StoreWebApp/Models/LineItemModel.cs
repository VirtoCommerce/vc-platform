using System;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Models
{

	/// <summary>
	/// Class LineItemModel.
	/// </summary>
	public class LineItemModel
	{
		/// <summary>
		/// The _item
		/// </summary>
		private readonly ItemModel _item;
		/// <summary>
		/// The _line item
		/// </summary>
		private readonly LineItem _lineItem;
		/// <summary>
		/// The _parent item
		/// </summary>
		private readonly ItemModel _parentItem;

		/// <summary>
		/// Initializes a new instance of the <see cref="LineItemModel"/> class.
		/// </summary>
		/// <param name="lineItem">The line item.</param>
		/// <param name="catalogItem">The catalog item.</param>
		/// <param name="parentCatalogItem">The parent catalog item.</param>
		/// <exception cref="System.ArgumentNullException">catalogItem</exception>
		public LineItemModel(LineItem lineItem, Item catalogItem, Item parentCatalogItem)
		{
			_lineItem = lineItem;

			if (catalogItem == null)
			{
				throw new ArgumentNullException("catalogItem");
			}
			
			{
				var propertySet = CatalogHelper.CatalogClient.GetPropertySet(catalogItem.PropertySetId);
				_item = CatalogHelper.CreateItemModel(catalogItem, propertySet);
			}

			if (parentCatalogItem != null)
			{
				var propertySet = CatalogHelper.CatalogClient.GetPropertySet(parentCatalogItem.PropertySetId);
				_parentItem = CatalogHelper.CreateItemModel(parentCatalogItem, propertySet);
				_item.ParentItemId = parentCatalogItem.ItemId;
			}
		}

		/// <summary>
		/// Gets the line item.
		/// </summary>
		/// <value>The line item.</value>
		public LineItem LineItem
		{
			get { return _lineItem; }
		}

		/// <summary>
		/// Gets the catalog item.
		/// </summary>
		/// <value>The catalog item.</value>
		public ItemModel CatalogItem
		{
			get { return _item; }
		}

		/// <summary>
		/// Gets the parent catalog item model.
		/// </summary>
		/// <value>The parent catalog item model.</value>
		public ItemModel ParentCatalogItemModel
		{
			get { return _parentItem; }
		}

		/// <summary>
		/// Gets the parent catalog item.
		/// </summary>
		/// <value>The parent catalog item.</value>
		public Item ParentCatalogItem
		{
			get { return _parentItem !=null ? _parentItem.Item : null; }
		}

		/// <summary>
		/// Gets the formatted placed price.
		/// </summary>
		/// <value>The formatted placed price.</value>
		public string FormattedPlacedPrice
		{
			get { return StoreHelper.FormatCurrency(LineItem.PlacedPrice, UserHelper.CustomerSession.Currency); }
		}

		/// <summary>
		/// Gets the formatted extended price.
		/// </summary>
		/// <value>The formatted extended price.</value>
		public string FormattedExtendedPrice
		{
			get { return StoreHelper.FormatCurrency(LineItem.ExtendedPrice, UserHelper.CustomerSession.Currency); }
		}

		/// <summary>
		/// Gets the display name.
		/// </summary>
		/// <value>The display name.</value>
		public string DisplayName
		{
			get
			{
				var retValue = LineItem.DisplayName;
				var title = CatalogHelper.CatalogClient.GetPropertyValueByName(CatalogItem.Item, "Title");
				if (title != null)
				{
					retValue = title.ToString();
				}
				return retValue;
			}
		}

		/// <summary>
		/// Gets the line item identifier.
		/// </summary>
		/// <value>The line item identifier.</value>
		public string LineItemId
		{
			get { return LineItem.LineItemId; }
		}

		/// <summary>
		/// Gets the quantity.
		/// </summary>
		/// <value>The quantity.</value>
		public decimal Quantity
		{
			get { return LineItem.Quantity; }
		}

		/// <summary>
		/// Gets the comment.
		/// </summary>
		/// <value>The comment.</value>
		public string Comment
		{
			get { return LineItem.Comment; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is promotion.
		/// </summary>
		/// <value><c>true</c> if this instance is promotion; otherwise, <c>false</c>.</value>
		public bool IsPromotion
		{
			get
			{
				string promtotioId;
				var isPromotion = PromotionHelper.IsPromotion(CatalogItem.ItemId, out promtotioId);
				return isPromotion && LineItem.Discounts.Any(x => x.PromotionId == promtotioId);
			}
		}
	}
}