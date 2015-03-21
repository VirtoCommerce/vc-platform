using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("OrderGroups")]
	[DataServiceKey("OrderGroupId")]
	public abstract class OrderGroup : StorageEntity
	{
		public OrderGroup()
		{
			OrderGroupId = GenerateNewKey();
		}
		private string _OrderGroupId;
		[Key]
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
		[StringLength(128)]
		[Required]
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

		private string _OrganizationId;
		/// <summary>
		/// The immediate parent organization id for the customer.
		/// </summary>
		/// <value>
		/// The organization id.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string OrganizationId
		{
			get
			{
				return _OrganizationId;
			}
			set
			{
				SetValue(ref _OrganizationId, () => this.OrganizationId, value);
			}
		}

		private string _CustomerId;
		/// <summary>
		/// The customer that placed the order.
		/// </summary>
		/// <value>
		/// The customer id.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string CustomerId
		{
			get
			{
				return _CustomerId;
			}
			set
			{
				SetValue(ref _CustomerId, () => this.CustomerId, value);
			}
		}

		private string _CustomerName;
		[StringLength(128)]
		[DataMember]
		public string CustomerName
		{
			get
			{
				return _CustomerName;
			}
			set
			{
				SetValue(ref _CustomerName, () => this.CustomerName, value);
			}
		}

		private string _StoreId;
		[StringLength(128)]
		[DataMember]
		public string StoreId
		{
			get
			{
				return _StoreId;
			}
			set
			{
				SetValue(ref _StoreId, () => this.StoreId, value);
			}
		}

		private string _AddressId;
		/// <summary>
		/// Gets or sets the billing address id in class <see cref="OrderAddress"/>.
		/// </summary>
		/// <value>
		/// The billing address id.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string AddressId
		{
			get
			{
				return _AddressId;
			}
			set
			{
				SetValue(ref _AddressId, () => this.AddressId, value);
			}
		}

		private decimal _ShippingTotal;
		/// <summary>
		/// Gets or sets the shipping total (sum of all form.ShippingTotal).
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
		/// Gets or sets the handling total. Can be used to handle additional costs, will need be made editable in the admin soon.
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
		/// Gets or sets the sum of all form.TaxTotal.
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
		/// Gets or sets the sum of all form.Subtotal.
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

		private decimal _Total;
		/// <summary>
		/// Gets or sets the sum of all form.Total.
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

        private decimal _DiscountTotal;
        /// <summary>
        /// Gets or sets the cart subtotal discount Total.
        /// </summary>
        /// <value>
        /// The discount Total.
        /// </value>
        [DataMember]
        public decimal DiscountTotal
        {
            get
            {
                return _DiscountTotal;
            }
            set
            {
                SetValue(ref _DiscountTotal, () => this.DiscountTotal, value);
            }
        }

        private decimal _FormDiscountTotal;
        /// <summary>
        /// Gets or sets the form discount total.
        /// </summary>
        /// <value>
        /// The form discount total.
        /// </value>
        [DataMember]
        public decimal FormDiscountTotal
        {
            get
            {
                return _FormDiscountTotal;
            }
            set
            {
                SetValue(ref _FormDiscountTotal, () => this.FormDiscountTotal, value);
            }
        }

        private decimal _LineItemDiscountTotal;
        /// <summary>
        /// Gets or sets the line item discount Total.
        /// </summary>
        /// <value>
        /// The line item discount Total.
        /// </value>
        [DataMember]
        public decimal LineItemDiscountTotal
        {
            get
            {
                return _LineItemDiscountTotal;
            }
            set
            {
                SetValue(ref _LineItemDiscountTotal, () => this.LineItemDiscountTotal, value);
            }
        }

        private decimal _ShipmentDiscountTotal;
        /// <summary>
        /// Gets or sets the shipment discount Total.
        /// </summary>
        /// <value>
        /// The shipment discount Total.
        /// </value>
        [DataMember]
        public decimal ShipmentDiscountTotal
        {
            get
            {
                return _ShipmentDiscountTotal;
            }
            set
            {
                SetValue(ref _ShipmentDiscountTotal, () => this.ShipmentDiscountTotal, value);
            }
        }

		private string _BillingCurrency;
		[DataMember]
		[StringLength(32)]
		public string BillingCurrency
		{
			get
			{
				return _BillingCurrency;
			}
			set
			{
				SetValue(ref _BillingCurrency, () => this.BillingCurrency, value);
			}
		}

		private string _Status;
		[DataMember]
		[StringLength(32)]
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

		#region Navigation Properties

		private ObservableCollection<OrderForm> _OrderForms = null;
		[DataMember]
		public ObservableCollection<OrderForm> OrderForms
		{
			get
			{
				if (_OrderForms == null)
					_OrderForms = new ObservableCollection<OrderForm>();

				return _OrderForms;
			}
		}

		private ObservableCollection<OrderAddress> _OrderAddress = null;
		[DataMember]
		public ObservableCollection<OrderAddress> OrderAddresses
		{
			get
			{
				if (_OrderAddress == null)
					_OrderAddress = new ObservableCollection<OrderAddress>();

				return _OrderAddress;
			}
		}

		#endregion

	}
}
