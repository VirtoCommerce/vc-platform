using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("LineItems")]
	[DataServiceKey("LineItemId")]
	public class LineItem : StorageEntity
	{
		public LineItem()
		{
			LineItemId = GenerateNewKey();
		}

		private string _LineItemId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string LineItemId
		{
			get
			{
				return _LineItemId;
			}
			set
			{
				SetValue(ref _LineItemId, () => this.LineItemId, value);
			}
		}

		private string _OrderFormId;
		[StringLength(128)]
		[DataMember]
		public string OrderFormId
		{
			get
			{
				return _OrderFormId;
			}
			set
			{
				SetValue(ref _OrderFormId, () => this.OrderFormId, value);
			}
		}

		private string _Catalog;
		/// <summary>
		/// Gets or sets the catalog item was added from.
		/// </summary>
		/// <value>
		/// The catalog.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string Catalog
		{
			get
			{
				return _Catalog;
			}
			set
			{
				SetValue(ref _Catalog, () => this.Catalog, value);
			}
		}

		private string _CatalogCategory;
		/// <summary>
		/// Gets or sets the catalog category item was added from
		/// </summary>
		/// <value>
		/// The catalog category.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string CatalogCategory
		{
			get
			{
				return _CatalogCategory;
			}
			set
			{
				SetValue(ref _CatalogCategory, () => this.CatalogCategory, value);
			}
		}

		private string _ShippingMethodId;
		/// <summary>
		/// Gets or sets the shipping method id, the shipment method that will be used to send current line item.
		/// </summary>
		/// <value>
		/// The shipping method id.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string ShippingMethodId
		{
			get
			{
				return _ShippingMethodId;
			}
			set
			{
				SetValue(ref _ShippingMethodId, () => this.ShippingMethodId, value);
			}
		}

		private string _CatalogOutline;
		/// <summary>
		/// Gets or sets the catalog outline. Complete path to where the was added from. For example ElectronicsCatalog/TVs
		/// </summary>
		/// <value>
		/// The catalog outline.
		/// </value>
		[DataMember]
		public string CatalogOutline
		{
			get
			{
				return _CatalogOutline;
			}
			set
			{
				SetValue(ref _CatalogOutline, () => this.CatalogOutline, value);
			}
		}

		private string _CatalogItemId;
		[StringLength(128)]
		[Required]
		[DataMember]
		public string CatalogItemId
		{
			get
			{
				return _CatalogItemId;
			}
			set
			{
				SetValue(ref _CatalogItemId, () => this.CatalogItemId, value);
			}
		}

		private string _ParentCatalogItemId;
		[StringLength(128)]
		[DataMember]
		public string ParentCatalogItemId
		{
			get
			{
				return _ParentCatalogItemId;
			}
			set
			{
				SetValue(ref _ParentCatalogItemId, () => this.ParentCatalogItemId, value);
			}
		}

		private string _CatalogItemCode;
		[StringLength(128)]
		[Required]
		[DataMember]
		public string CatalogItemCode
		{
			get
			{
				return _CatalogItemCode;
			}
			set
			{
				SetValue(ref _CatalogItemCode, () => this.CatalogItemCode, value);
			}
		}

		private decimal _Quantity;
		/// <summary>
		/// Gets or sets the quantity of item added to the cart.
		/// </summary>
		/// <value>
		/// The quantity.
		/// </value>
		[Required]
		[DataMember]
		public decimal Quantity
		{
			get
			{
				return _Quantity;
			}
			set
			{
				SetValue(ref _Quantity, () => this.Quantity, value);
			}
		}

		private decimal _MinQuantity;
		/// <summary>
		/// Gets or sets the min quantity. The minimum quantity that can be ordered.
		/// </summary>
		/// <value>
		/// The min quantity.
		/// </value>
		[DataMember]
		public decimal MinQuantity
		{
			get
			{
				return _MinQuantity;
			}
			set
			{
				SetValue(ref _MinQuantity, () => this.MinQuantity, value);
			}
		}

		private decimal _MaxQuantity;
		/// <summary>
		/// Gets or sets the max quantity. The maximum quantity that can be ordered.
		/// </summary>
		/// <value>
		/// The max quantity.
		/// </value>
		[DataMember]
		public decimal MaxQuantity
		{
			get
			{
				return _MaxQuantity;
			}
			set
			{
				SetValue(ref _MaxQuantity, () => this.MaxQuantity, value);
			}
		}

		private decimal _PlacedPrice;
		[Required]
		[DataMember]
		public decimal PlacedPrice
		{
			get
			{
				return _PlacedPrice;
			}
			set
			{
				SetValue(ref _PlacedPrice, () => this.PlacedPrice, value);
			}
		}

		private decimal _taxTotal;
		/// <summary>
		/// Gets or sets the total taxes amount for this item multiplied by Quantity.
		/// </summary>
		/// <value>
		/// The tax total.
		/// </value>
		[Required]
		[DataMember]
		public decimal TaxTotal
		{
			get
			{
				return _taxTotal;
			}
			set
			{
				SetValue(ref _taxTotal, () => this.TaxTotal, value);
			}
		}

		private decimal _ListPrice;
		/// <summary>
		/// Gets or sets the price from a price list.
		/// </summary>
		/// <value>
		/// The price list price.
		/// </value>
		[DataMember]
		public decimal ListPrice
		{
			get
			{
				return _ListPrice;
			}
			set
			{
				SetValue(ref _ListPrice, () => this.ListPrice, value);
			}
		}

		private decimal _LineItemDiscountAmount;
		[DataMember]
		public decimal LineItemDiscountAmount
		{
			get
			{
				return _LineItemDiscountAmount;
			}
			set
			{
				SetValue(ref _LineItemDiscountAmount, () => this.LineItemDiscountAmount, value);
			}
		}

		private decimal _InStockQuantity;
		/// <summary>
		/// Gets or sets the in stock quantity. The available quantity in warehouses.
		/// </summary>
		/// <value>
		/// The in stock quantity.
		/// </value>
		[DataMember]
		public decimal InStockQuantity
		{
			get
			{
				return _InStockQuantity;
			}
			set
			{
				SetValue(ref _InStockQuantity, () => this.InStockQuantity, value);
			}
		}

		private decimal _PreorderQuantity;
		[DataMember]
		public decimal PreorderQuantity
		{
			get
			{
				return _PreorderQuantity;
			}
			set
			{
				SetValue(ref _PreorderQuantity, () => this.PreorderQuantity, value);
			}
		}

		private decimal _BackorderQuantity;
		[DataMember]
		public decimal BackorderQuantity
		{
			get
			{
				return _BackorderQuantity;
			}
			set
			{
				SetValue(ref _BackorderQuantity, () => this.BackorderQuantity, value);
			}
		}

		private string _ShippingAddressId;
		[StringLength(128)]
		[DataMember]
		public string ShippingAddressId
		{
			get
			{
				return _ShippingAddressId;
			}
			set
			{
				SetValue(ref _ShippingAddressId, () => this.ShippingAddressId, value);
			}
		}

		private string _ShippingMethodName;
		[StringLength(128)]
		[DataMember]
		public string ShippingMethodName
		{
			get
			{
				return _ShippingMethodName;
			}
			set
			{
				SetValue(ref _ShippingMethodName, () => this.ShippingMethodName, value);
			}
		}

		private decimal _ExtendedPrice;
		/// <summary>
		/// Gets or sets the price the customer pays for the whole selected lineItem amount.
		/// </summary>
		/// <value>
		/// The extended price.
		/// </value>
		[DataMember]
		public decimal ExtendedPrice
		{
			get
			{
				return _ExtendedPrice;
			}
			set
			{
				SetValue(ref _ExtendedPrice, () => this.ExtendedPrice, value);
			}
		}

		private string _Description;
		[StringLength(512)]
		[DataMember]
		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				SetValue(ref _Description, () => this.Description, value);
			}
		}

		private string _Comment;
		[StringLength(512)]
		[DataMember]
		public string Comment
		{
			get
			{
				return _Comment;
			}
			set
			{
				SetValue(ref _Comment, () => this.Comment, value);
			}
		}

		private string _DisplayName;
		[StringLength(1024)]
		[DataMember]
		public string DisplayName
		{
			get
			{
				return _DisplayName;
			}
			set
			{
				SetValue(ref _DisplayName, () => this.DisplayName, value);
			}
		}

		private bool _AllowBackorders;
		[DataMember]
		public bool AllowBackorders
		{
			get
			{
				return _AllowBackorders;
			}
			set
			{
				SetValue(ref _AllowBackorders, () => AllowBackorders, value);
			}
		}

		private bool _AllowPreorders;
		[DataMember]
		public bool AllowPreorders
		{
			get
			{
				return _AllowPreorders;
			}
			set
			{
				SetValue(ref _AllowPreorders, () => AllowPreorders, value);
			}
		}

		private string _InventoryStatus;
		[StringLength(128)]
		[DataMember]
		public string InventoryStatus
		{
			get
			{
				return _InventoryStatus;
			}
			set
			{
				SetValue(ref _InventoryStatus, () => this.InventoryStatus, value);
			}
		}

		private string _Status;
		[StringLength(128)]
		[DataMember]
		public string Status
		{
			get
			{
				return _Status;
			}
			set
			{
				SetValue(ref _Status, () => this.Status, value);
			}
		}

		private string _FulfillmentCenterId;
		[StringLength(128)]
		[DataMember]
		public string FulfillmentCenterId
		{
			get
			{
				return _FulfillmentCenterId;
			}
			set
			{
				SetValue(ref _FulfillmentCenterId, () => this.FulfillmentCenterId, value);
			}
		}

        private decimal _Weight;
        [DataMember]
        public decimal Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                SetValue(ref _Weight, () => this.Weight, value);
            }
        }

		#region Navigation Properties

		[DataMember]
		[ForeignKey("OrderFormId")]
		[Parent]
		public OrderForm OrderForm { get; set; }

		private ObservableCollection<LineItemDiscount> _Discounts;
		[DataMember]
		public ObservableCollection<LineItemDiscount> Discounts
		{
			get
			{
				if (_Discounts == null)
					_Discounts = new ObservableCollection<LineItemDiscount>();

				return _Discounts;
			}
		}

		private ObservableCollection<LineItemOption> _Options;
		[DataMember]
		public ObservableCollection<LineItemOption> Options
		{
			get
			{
				if (_Options == null)
					_Options = new ObservableCollection<LineItemOption>();

				return _Options;
			}
		}

        private string _ParentLineItemId;
        /// <summary>
        /// Gets or sets the parent line item identifier.
        /// Can be used for related items in bundles, packages, etc
        /// </summary>
        /// <value>
        /// The parent line item identifier.
        /// </value>
        [DataMember]
        [StringLength(128)]
        [ForeignKey("ParentLineItem")]
        public string ParentLineItemId
        {
            get
            {
                return _ParentLineItemId;
            }
            set
            {
                SetValue(ref _ParentLineItemId, () => this.ParentLineItemId, value);
            }
        }

        [DataMember]
        public virtual LineItem ParentLineItem { get; set; }

		#endregion
	}
}
