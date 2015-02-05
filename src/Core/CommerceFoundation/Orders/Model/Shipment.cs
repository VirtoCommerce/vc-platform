using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;
using VirtoCommerce.Foundation.Orders.Model.Fulfillment;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("Shipments")]
	[DataServiceKey("ShipmentId")]
	public class Shipment : StorageEntity
	{
		public Shipment()
		{
			ShipmentId = GenerateNewKey();
		}

		#region Properties
		private string _ShipmentId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ShipmentId
		{
			get
			{
				return _ShipmentId;
			}
			set
			{
				SetValue(ref _ShipmentId, () => this.ShipmentId, value);
			}
		}



		private string _ShippingMethodId;
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

		private string _ShippingMethodName;
		/// <summary>
		/// Gets or sets the name of the shipping method.
		/// </summary>
		/// <value>
		/// The ShippingMethodLanguage.DisplayName of the ICustomerSession.Language at the time the Shipment was create in front-end.
		/// </value>
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

		private string _ShipmentTrackingNumber;
		[StringLength(128)]
		[DataMember]
		public string ShipmentTrackingNumber
		{
			get
			{
				return _ShipmentTrackingNumber;
			}
			set
			{
				SetValue(ref _ShipmentTrackingNumber, () => this.ShipmentTrackingNumber, value);
			}
		}

		private decimal _itemSubtotal;
		/// <summary>
		/// Gets or sets items in the shipment subtotal. The sum of all lineItem.ExtendedPrice / lineItem.Quantity * Quantity. Without taxes.
		/// </summary>
		[DataMember]
		public decimal ItemSubtotal
		{
			get
			{
				return _itemSubtotal;
			}
			set
			{
				SetValue(ref _itemSubtotal, () => this.ItemSubtotal, value);
			}
		}

		private decimal _itemTaxTotal;
		/// <summary>
		/// Gets or sets the sum of all items' taxes in the shipment.
		/// </summary>
		[DataMember]
		public decimal ItemTaxTotal
		{
			get
			{
				return _itemTaxTotal;
			}
			set
			{
				SetValue(ref _itemTaxTotal, () => this.ItemTaxTotal, value);
			}
		}

		private decimal _ShippingCost;
		/// <summary>
		/// Shipping cost without taxes.
		/// </summary>
		[DataMember]
		public decimal ShippingCost
		{
			get
			{
				return _ShippingCost;
			}
			set
			{
				SetValue(ref _ShippingCost, () => this.ShippingCost, value);
			}
		}

		private decimal _ShippingTaxTotal;
		/// <summary>
		/// Shipping tax total. taxes applied to the ShippingCost.
		/// </summary>
		[DataMember]
		public decimal ShippingTaxTotal
		{
			get
			{
				return _ShippingTaxTotal;
			}
			set
			{
				SetValue(ref _ShippingTaxTotal, () => this.ShippingTaxTotal, value);
			}
		}

		private decimal _totalBeforeTax;
		/// <summary>
		/// Gets or sets the total before taxes (ItemSubtotal + ShippingCost - ShippingDiscountAmount).
		/// </summary>
		[DataMember]
		public decimal TotalBeforeTax
		{
			get
			{
				return _totalBeforeTax;
			}
			set
			{
				SetValue(ref _totalBeforeTax, () => this.TotalBeforeTax, value);
			}
		}

		private decimal _ShipmentTotal;
		/// <summary>
		/// Gets or sets the shipment total. (Subtotal - ShippingDiscountAmount).
		/// </summary>
		/// <value>
		/// The shipment total.
		/// </value>
		[DataMember]
		public decimal ShipmentTotal
		{
			get
			{
				return _ShipmentTotal;
			}
			set
			{
				SetValue(ref _ShipmentTotal, () => this.ShipmentTotal, value);
			}
		}

		private decimal _Subtotal;
		/// <summary>
		/// Gets or sets the subtotal (ItemSubtotal + ShippingCost + ItemTaxTotal + ShippingTaxTotal). The shipment total without discount.
		/// </summary>
		/// <value>
		/// The subtotal.
		/// </value>
		[DataMember]
		public decimal Subtotal
		{
			get
			{
				return _Subtotal;
			}
			set
			{
				SetValue(ref _Subtotal, () => this.Subtotal, value);
			}
		}

		private decimal _ShippingDiscountAmount;
		/// <summary>
		/// Gets or sets the sum of all Discounts.DiscountAmount
		/// </summary>
		/// <value>
		/// The shipment discounts amount.
		/// </value>
		[DataMember]
		public decimal ShippingDiscountAmount
		{
			get
			{
				return _ShippingDiscountAmount;
			}
			set
			{
				SetValue(ref _ShippingDiscountAmount, () => this.ShippingDiscountAmount, value);
			}
		}

		private string _Status;
		[StringLength(64)]
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


		#endregion

		#region Navigation Properties

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

		[DataMember]
		[ForeignKey("OrderFormId")]
		[Parent]
		public OrderForm OrderForm { get; set; }

		private string _PicklistId;
		[StringLength(128)]
		[DataMember]
		public string PicklistId
		{
			get
			{
				return _PicklistId;
			}
			set
			{
				SetValue(ref _PicklistId, () => this.PicklistId, value);
			}
		}

		[DataMember]
		[ForeignKey("PicklistId")]
		public Picklist Picklist { get; set; }

		private ObservableCollection<ShipmentDiscount> _Discounts;
		[DataMember]
		public ObservableCollection<ShipmentDiscount> Discounts
		{
			get
			{
				if (_Discounts == null)
					_Discounts = new ObservableCollection<ShipmentDiscount>();

				return _Discounts;
			}
		}

		private ObservableCollection<ShipmentItem> _ShipmentItems;
		[DataMember]
		public ObservableCollection<ShipmentItem> ShipmentItems
		{
			get
			{
				if (_ShipmentItems == null)
					_ShipmentItems = new ObservableCollection<ShipmentItem>();

				return _ShipmentItems;
			}
		}

        private ObservableCollection<ShipmentOption> _Options;
        [DataMember]
        public ObservableCollection<ShipmentOption> Options
        {
            get
            {
                if (_Options == null)
                    _Options = new ObservableCollection<ShipmentOption>();

                return _Options;
            }
        }
		#endregion
	}
}
