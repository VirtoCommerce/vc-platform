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
	[EntitySet("OrderForms")]
	[DataServiceKey("OrderFormId")]
	public class OrderForm : StorageEntity
	{
		public OrderForm()
		{
			OrderFormId = GenerateNewKey();
		}

		private string _OrderFormId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

		private string _OrderGroupId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string OrderGroupId
		{
			get
			{
				return _OrderGroupId;
			}
			set
			{
				SetValue(ref _OrderGroupId, () => this.OrderGroupId, value);
			}
		}

		private string _Name;
		/// <summary>
		/// Gets or sets the name. Should be set to "Default" by default. Can be used to create multiple OrderForms with different meaning, for example "Quote", which is the initial quote price. Not used at the moment.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[Required]
		[StringLength(128)]
		[DataMember]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
		}

		private string _BillingAddressId;
		[StringLength(128)]
		[DataMember]
		public string BillingAddressId
		{
			get
			{
				return _BillingAddressId;
			}
			set
			{
				SetValue(ref _BillingAddressId, () => this.BillingAddressId, value);
			}
		}

		private string _Status;
		/// <summary>
		/// Gets or sets the status. Can be used in some cases for lets say "Approved" and so on.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[DataMember]
		[StringLength(64)]
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

		private decimal _ShippingTotal;
		/// <summary>
		/// Gets or sets the shipments total. the sum of all shipment
		/// </summary>
		/// <value>
		/// The shipping total.
		/// </value>
		[DataMember]
		public decimal ShippingTotal
		{
			get
			{
				return _ShippingTotal;
			}
			set
			{
				SetValue(ref _ShippingTotal, () => this.ShippingTotal, value);
			}
		}

		private decimal _HandlingTotal;
		/// <summary>
		/// Gets or sets the total handling fee.
		/// </summary>
		/// <value>
		/// The handling total.
		/// </value>
		[DataMember]
		public decimal HandlingTotal
		{
			get
			{
				return _HandlingTotal;
			}
			set
			{
				SetValue(ref _HandlingTotal, () => this.HandlingTotal, value);
			}
		}

		private decimal _TaxTotal;
		/// <summary>
		/// Gets or sets the sum of all applicable taxes.
		/// </summary>
		/// <value>
		/// The tax total.
		/// </value>
		[DataMember]
		public decimal TaxTotal
		{
			get
			{
				return _TaxTotal;
			}
			set
			{
				SetValue(ref _TaxTotal, () => this.TaxTotal, value);
			}
		}

		private decimal _Subtotal;
		/// <summary>
		/// Gets or sets the sub total. Sum of all LineItems' (ListPrice * Quantity). Doesn't include any discounts, shipment costs, taxes or handling fees.
		/// </summary>
		/// <value>
		/// The sub total.
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

		private decimal _Total;
		/// <summary>
		/// Gets or sets the total for the order form including all the discounts. Subtotal + shippingSubTotal + TaxTotal - discountTotal.
		/// </summary>
		/// <value>
		/// The total.
		/// </value>
		[DataMember]
		public decimal Total
		{
			get
			{
				return _Total;
			}
			set
			{
				SetValue(ref _Total, () => this.Total, value);
			}
		}

		private decimal _DiscountAmount;
        /// <summary>
        /// Gets or sets the cart subtotal discount amount.
        /// </summary>
        /// <value>
        /// The discount amount.
        /// </value>
		[DataMember]
		public decimal DiscountAmount
		{
			get
			{
				return _DiscountAmount;
			}
			set
			{
				SetValue(ref _DiscountAmount, () => this.DiscountAmount, value);
			}
		}

        private decimal _LineItemDiscountAmount;
        /// <summary>
        /// Gets or sets the line item discount amount.
        /// </summary>
        /// <value>
        /// The line item discount amount.
        /// </value>
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

        private decimal _ShipmentDiscountAmount;
        /// <summary>
        /// Gets or sets the shipment discount amount.
        /// </summary>
        /// <value>
        /// The shipment discount amount.
        /// </value>
        [DataMember]
        public decimal ShipmentDiscountAmount
        {
            get
            {
                return _ShipmentDiscountAmount;
            }
            set
            {
                SetValue(ref _ShipmentDiscountAmount, () => this.ShipmentDiscountAmount, value);
            }
        }

		#region Navigation Properties

		[DataMember]
		[ForeignKey("OrderGroupId")]
		[Parent]
		public OrderGroup OrderGroup { get; set; }

		private ObservableCollection<LineItem> _LineItems = null;
		[DataMember]
		public ObservableCollection<LineItem> LineItems
		{
			get
			{
				if (_LineItems == null)
					_LineItems = new ObservableCollection<LineItem>();

				return _LineItems;
			}
		}

		private ObservableCollection<OrderFormDiscount> _Discounts;
		[DataMember]
		public ObservableCollection<OrderFormDiscount> Discounts
		{
			get
			{
				if (_Discounts == null)
					_Discounts = new ObservableCollection<OrderFormDiscount>();

				return _Discounts;
			}
		}

		private ObservableCollection<Payment> _Payments;
		[DataMember]
		public ObservableCollection<Payment> Payments
		{
			get
			{
				if (_Payments == null)
					_Payments = new ObservableCollection<Payment>();

				return _Payments;
			}
		}

		private ObservableCollection<Shipment> _Shipments;
		[DataMember]
		public ObservableCollection<Shipment> Shipments
		{
			get
			{
				if (_Shipments == null)
					_Shipments = new ObservableCollection<Shipment>();

				return _Shipments;
			}
		}

		private ObservableCollection<OrderFormPropertyValue> _OrderFormPropertyValues;
		[DataMember]
		public ObservableCollection<OrderFormPropertyValue> OrderFormPropertyValues
		{
			get { return _OrderFormPropertyValues ?? (_OrderFormPropertyValues = new ObservableCollection<OrderFormPropertyValue>()); }
		}

		#endregion

	}
}
