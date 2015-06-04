using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("Payments")]
	[DataServiceKey("PaymentId")]
	[KnownType(typeof(CreditCardPayment))]
	[KnownType(typeof(CashCardPayment))]
	[KnownType(typeof(InvoicePayment))]
	[KnownType(typeof(OtherPayment))]
	public abstract class Payment : StorageEntity
	{
		public Payment()
		{
			PaymentId = GenerateNewKey();
		}

		private string _PaymentId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string PaymentId
		{
			get
			{
				return _PaymentId;
			}
			set
			{
				SetValue(ref _PaymentId, () => this.PaymentId, value);
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

		private string _ContractId;
		[StringLength(128)]
		[DataMember]
		public string ContractId
		{
			get
			{
				return _ContractId;
			}
			set
			{
				SetValue(ref _ContractId, () => this.ContractId, value);
			}
		}

		private string _PaymentMethodId;
		/// <summary>
		/// Gets or sets the payment method id. It's a reference to <see cref="PaymentMethod"/>.PaymentMethodId property.
		/// </summary>
		/// <value>
		/// The payment method id.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string PaymentMethodId
		{
			get
			{
				return _PaymentMethodId;
			}
			set
			{
				SetValue(ref _PaymentMethodId, () => this.PaymentMethodId, value);
			}
		}

		private string _PaymentMethodName;
		/// <summary>
		/// Gets or sets the name of the payment method in <see cref="PaymentMethod"/>.Name property.
		/// </summary>
		/// <value>
		/// The name of the payment method.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string PaymentMethodName
		{
			get
			{
				return _PaymentMethodName;
			}
			set
			{
				SetValue(ref _PaymentMethodName, () => this.PaymentMethodName, value);
			}
		}

		#region Codes returned by the provider
		private string _ValidationCode;
		/// <summary>
		/// Gets or sets the validation code or approval code. In some payment gateways this is used to capture the transaction.
		/// </summary>
		/// <value>
		/// The validation code.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string ValidationCode
		{
			get
			{
				return _ValidationCode;
			}
			set
			{
				SetValue(ref _ValidationCode, () => this.ValidationCode, value);
			}
		}

		private string _AuthorizationCode;
		/// <summary>
		/// Gets or sets the authorization code. Used when previously authorized transaction needs to be captured or voided.
		/// </summary>
		/// <value>
		/// The authorization code.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string AuthorizationCode
		{
			get
			{
				return _AuthorizationCode;
			}
			set
			{
				SetValue(ref _AuthorizationCode, () => this.AuthorizationCode, value);
			}
		}

		private string _StatusCode;
		/// <summary>
		/// Indicates the status of the authorization request. This field contains the actual response code as returned by the Gateway.
		/// </summary>
		/// <value>
		/// The status code.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string StatusCode
		{
			get
			{
				return _StatusCode;
			}
			set
			{
				SetValue(ref _StatusCode, () => this.StatusCode, value);
			}
		}

		private string _StatusDesc;
		/// <summary>
		/// Text information that describes each response code. This field contains a response or display text message. This message can be used by the 
		/// terminal to display the authorization result. The display text must not be used to determine the nature of a response message. 
		/// A Gateway may translate the response according to the language indicated in the merchant account setup (if applicable). 
		/// </summary>
		/// <value>
		/// The status code.
		/// </value>
		[StringLength(128)]
		[DataMember]
		public string StatusDesc
		{
			get
			{
				return _StatusDesc;
			}
			set
			{
				SetValue(ref _StatusDesc, () => this.StatusDesc, value);
			}
		}

		#endregion

		private int _PaymentTypeId;
		[Required]
		[DataMember]
		public int PaymentTypeId
		{
			get
			{
				return _PaymentTypeId;
			}
			set
			{
				SetValue(ref _PaymentTypeId, () => this.PaymentTypeId, value);
			}
		}

		private int _PaymentType;
		[DataMember]
		public int PaymentType
		{
			get
			{
				return _PaymentType;
			}
			set
			{
				SetValue(ref _PaymentType, () => this.PaymentType, value);
			}
		}

		private decimal _Amount;
		[DataMember]
		public decimal Amount
		{
			get
			{
				return _Amount;
			}
			set
			{
				SetValue(ref _Amount, () => this.Amount, value);
			}
		}

		private string _Status;
		/// <summary>
		/// Gets or sets the status of the payment. Status can be Pending, Canceled, Completed, Denied, Failed, Processing, OnHold and are defined in PaymentStatus enum.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
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

		private string _TransactionType;
		/// <summary>
		/// Gets or sets the type of the transaction. The types can be Authorization, Capture, Sale, Credit, Void and are described in TransactionType enum.
		/// </summary>
		/// <value>
		/// The type of the transaction.
		/// </value>
		[StringLength(64)]
		[DataMember]
		public string TransactionType
		{
			get
			{
				return _TransactionType;
			}
			set
			{
				SetValue(ref _TransactionType, () => this.TransactionType, value);
			}
		}

		[DataMember]
		[ForeignKey("OrderFormId")]
		[Parent]
		public OrderForm OrderForm { get; set; }

	}
}
