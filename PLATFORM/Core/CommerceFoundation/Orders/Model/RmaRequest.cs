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
	[EntitySet("RmaRequests")]
	[DataServiceKey("RmaRequestId")]
	public class RmaRequest : StorageEntity
	{
		public RmaRequest()
		{
			RmaRequestId = GenerateNewKey();
		}

		private string _RmaRequestId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string RmaRequestId
		{
			get
			{
				return _RmaRequestId;
			}
			set
			{
				SetValue(ref _RmaRequestId, () => this.RmaRequestId, value);
			}
		}


		private string _AuthorizationCode;
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

		private string _Notes;
		/// <summary>
		/// Fulfillment center person notes on package received
		/// </summary>
		[DataMember]
		public string Notes
		{
			get
			{
				return _Notes;
			}
			set
			{
				SetValue(ref _Notes, () => this.Notes, value);
			}
		}

		private string _AgentId;
		/// <summary>
		/// Fulfillment center representative who received items of the return
		/// </summary>
		[StringLength(128)]
		[DataMember]
		public string AgentId
		{
			get
			{
				return _AgentId;
			}
			set
			{
				SetValue(ref _AgentId, () => this.AgentId, value);
			}
		}

		private string _ReturnAddressId;
		/// <summary>
		/// Return destination address id (fulfillment center address)
		/// </summary>
		[StringLength(128)]
		[DataMember]
		public string ReturnAddressId
		{
			get
			{
				return _ReturnAddressId;
			}
			set
			{
				SetValue(ref _ReturnAddressId, () => this.ReturnAddressId, value);
			}
		}

		private string _ReturnFromAddressId;
		/// <summary>
		/// Return departure address id
		/// </summary>
		[StringLength(128)]
		[DataMember]
		public string ReturnFromAddressId
		{
			get
			{
				return _ReturnFromAddressId;
			}
			set
			{
				SetValue(ref _ReturnFromAddressId, () => this.ReturnFromAddressId, value);
			}
		}


		private string _Comment;
		/// <summary>
		/// Comment for the return is entered by the customer
		/// </summary>
		[StringLength(528)]
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

		private string _Status;
		/// <summary>
		/// Return request status. Available values stored in <c>RmaRequestStatus</c>
		/// </summary>
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

		private decimal _ItemSubtotal;
		/// <summary>
		/// Gets or sets the sum of all RmaReturnItems.ReturnAmount.
		/// </summary>
		/// <value>
		/// The item subtotal.
		/// </value>
		[DataMember]
		public decimal ItemSubtotal
		{
			get
			{
				return _ItemSubtotal;
			}
			set
			{
				SetValue(ref _ItemSubtotal, () => this.ItemSubtotal, value);
			}
		}

		private decimal _TotalBeforeTax;
		/// <summary>
		/// Gets or sets the total (including shipping) before taxes.
		/// </summary>
		/// <value>
		/// The total before tax.
		/// </value>
		[DataMember]
		public decimal TotalBeforeTax
		{
			get
			{
				return _TotalBeforeTax;
			}
			set
			{
				SetValue(ref _TotalBeforeTax, () => this.TotalBeforeTax, value);
			}
		}

		private decimal _ItemTax;
		/// <summary>
		/// Gets or sets the taxes amount to refund (lineItem.TaxTotal * RmaLineItem.ReturnQuantity / lineItem.Quantity).
		/// </summary>
		/// <value>
		/// The item tax.
		/// </value>
		[DataMember]
		public decimal ItemTax
		{
			get
			{
				return _ItemTax;
			}
			set
			{
				SetValue(ref _ItemTax, () => this.ItemTax, value);
			}
		}

		private decimal _LessReStockingFee;
		/// <summary>
		/// Gets or sets the less restocking fee. Currently 0.
		/// </summary>
		/// <value>
		/// The less re stocking fee.
		/// </value>
		[DataMember]
		public decimal LessReStockingFee
		{
			get
			{
				return _LessReStockingFee;
			}
			set
			{
				SetValue(ref _LessReStockingFee, () => this.LessReStockingFee, value);
			}
		}

		private decimal _ReturnTotal;
		/// <summary>
		/// Gets or sets the total amount of money to refund on RMA request completion
		/// </summary>
		/// <value>
		/// The return total.
		/// </value>
		[DataMember]
		public decimal ReturnTotal
		{
			get
			{
				return _ReturnTotal;
			}
			set
			{
				SetValue(ref _ReturnTotal, () => this.ReturnTotal, value);
			}
		}

		private decimal _RefundAmount;
		/// <summary>
		/// Gets or sets the amount already refunded to customer
		/// </summary>
		/// <value>
		/// The refund amount.
		/// </value>
		[DataMember]
		public decimal RefundAmount
		{
			get
			{
				return _RefundAmount;
			}
			set
			{
				SetValue(ref _RefundAmount, () => this.RefundAmount, value);
			}
		}

		private bool _IsPhysicalReturnRequired;
		/// <summary>
		/// Gets or sets a value indicating whether the items in this RMA request are required to be physically returned.
		/// </summary>
		/// <value>
		/// <c>true</c> if the items are required to be physically returned; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsPhysicalReturnRequired
		{
			get
			{
				return _IsPhysicalReturnRequired;
			}
			set
			{
				SetValue(ref _IsPhysicalReturnRequired, () => this.IsPhysicalReturnRequired, value);
			}
		}

		#region Navigation Properties
		private ObservableCollection<RmaReturnItem> _returnItems;
		[DataMember]
		public ObservableCollection<RmaReturnItem> RmaReturnItems
		{
			get
			{
				if (_returnItems == null)
					_returnItems = new ObservableCollection<RmaReturnItem>();

				return _returnItems;
			}
		}

		private string _OrderId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string OrderId
		{
			get
			{
				return _OrderId;
			}
			set
			{
				SetValue(ref _OrderId, () => this.OrderId, value);
			}
		}

		[DataMember]
		[ForeignKey("OrderId")]
		[Parent]
		public Order Order { get; set; }

		private string _ExchangeOrderId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ExchangeOrderId
		{
			get
			{
				return _ExchangeOrderId;
			}
			set
			{
				SetValue(ref _ExchangeOrderId, () => this.ExchangeOrderId, value);
			}
		}

		[DataMember]
		[ForeignKey("ExchangeOrderId")]
		public Order ExchangeOrder { get; set; }
		#endregion

	}
}
