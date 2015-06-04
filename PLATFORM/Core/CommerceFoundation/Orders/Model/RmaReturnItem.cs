using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("RmaReturnItems")]
	[DataServiceKey("RmaReturnItemId")]
	public class RmaReturnItem : StorageEntity
	{
		public RmaReturnItem()
		{
			RmaReturnItemId = GenerateNewKey();
		}

		private string _RmaReturnItemId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string RmaReturnItemId
		{
			get
			{
				return _RmaReturnItemId;
			}
			set
			{
				SetValue(ref _RmaReturnItemId, () => this.RmaReturnItemId, value);
			}
		}


		private string _ReturnReason;
		/// <summary>
		/// Gets or sets the return reason.
		/// </summary>
		/// <value>
		/// The return reason. Values from system setting "ReturnReasons".
		/// </value>
		[StringLength(128)]
		[Required]
		[DataMember]
		public string ReturnReason
		{
			get
			{
				return _ReturnReason;
			}
			set
			{
				SetValue(ref _ReturnReason, () => this.ReturnReason, value);
			}
		}

		private string _ItemState;
		/// <summary>
		/// RMA line item state. <c>RmaLineItemState</c>
		/// </summary>
		/// <value>
		/// RmaLineItemState.ToString()
		/// </value>
		[StringLength(128)]
		[Required]
		[DataMember]
		public string ItemState
		{
			get
			{
				return _ItemState;
			}
			set
			{
				SetValue(ref _ItemState, () => this.ItemState, value);
			}
		}

		private string _returnCondition;
		/// <summary>
		/// Condition of the returned item, entered by fulfillment center person. Available values are stored in the AppConfig setting "ReturnCondition".
		/// </summary>
		[StringLength(128)]
		[DataMember]
		public string ReturnCondition
		{
			get
			{
				return _returnCondition;
			}
			set
			{
				SetValue(ref _returnCondition, () => this.ReturnCondition, value);
			}
		}

		private decimal _ReturnAmount;
		/// <summary>
		/// Gets or sets the return amount (lineItem.ExpendedPrice * RmaLineItem.ReturnQuantity / lineItem.Quantity)
		/// </summary>
		/// <value>
		/// The return amount.
		/// </value>
		[DataMember]
		public decimal ReturnAmount
		{
			get
			{
				return _ReturnAmount;
			}
			set
			{
				SetValue(ref _ReturnAmount, () => this.ReturnAmount, value);
			}
		}

		#region Navigation Properties
		private ObservableCollection<RmaLineItem> _RmaLineItems;
		/// <summary>
		/// RmaLineItem collection with a single item in it. Collection is used for technical reasons only.
		/// </summary>
		[DataMember]
		public ObservableCollection<RmaLineItem> RmaLineItems
		{
			get
			{
				if (_RmaLineItems == null)
					_RmaLineItems = new ObservableCollection<RmaLineItem>();

				return _RmaLineItems;
			}
		}
		/*
		private string _RmaLineItemId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		//[ForeignKey("RmaLineItem")]
		public string RmaLineItemId
		{
			get
			{
				return _RmaLineItemId;
			}
			set
			{
				SetValue(ref _RmaLineItemId, () => this.RmaLineItemId, value);
			}
		}
		 * */

		/*
		[DataMember]
		public RmaLineItem RmaLineItem 
		{
			get;set;
		}
		 * */

		private string _RmaRequestId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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

		[DataMember]
		[ForeignKey("RmaRequestId")]
		public RmaRequest RmaRequest { get; set; }

		#endregion


	}
}
