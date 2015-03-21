using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Inventories.Model
{
	[DataContract]
	[EntitySet("Inventories")]
	[DataServiceKey("InventoryId")]
	public class Inventory : StorageEntity
	{
		public Inventory()
		{
			_InventoryId = GenerateNewKey();
		}

		private string _InventoryId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string InventoryId
		{
			get
			{
				return _InventoryId;
			}
			set
			{
				SetValue(ref _InventoryId, () => this.InventoryId, value);
			}
		}

		private string _fulfillmentCenterId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[Required]
		public string FulfillmentCenterId
		{
			get
			{
				return _fulfillmentCenterId;
			}
			set
			{
				SetValue(ref _fulfillmentCenterId, () => this.FulfillmentCenterId, value);
			}
		}

		private decimal _InStockQuantity;
		[DataMember]
		[Required]
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

		private decimal _ReservedQuantity;
		[DataMember]
		[Required]
		public decimal ReservedQuantity
		{
			get
			{
				return _ReservedQuantity;
			}
			set
			{
				SetValue(ref _ReservedQuantity, () => this.ReservedQuantity, value);
			}
		}

		private decimal _ReorderMinQuantity;
		[DataMember]
		[Required]
		public decimal ReorderMinQuantity
		{
			get
			{
				return _ReorderMinQuantity;
			}
			set
			{
				SetValue(ref _ReorderMinQuantity, () => this.ReorderMinQuantity, value);
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

		private bool _AllowBackorder;
		[DataMember]
		public bool AllowBackorder
		{
			get
			{
				return _AllowBackorder;
			}
			set
			{
				SetValue(ref _AllowBackorder, () => this.AllowBackorder, value);
			}
		}

		private bool _AllowPreorder;
		[DataMember]
		public bool AllowPreorder
		{
			get
			{
				return _AllowPreorder;
			}
			set
			{
				SetValue(ref _AllowPreorder, () => this.AllowPreorder, value);
			}
		}

		/// <summary>
		/// Inventory status look at InventoryStatus enumeration
		/// </summary>
		private int _Status;
		[DataMember]
		[Required]
		public int Status
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

		/// <summary>
		/// The date from when the preorder is allowed. 
		/// If not set AllowPreorder has no effect and not available
		/// </summary>
		private DateTime? _PreorderAvailabilityDate;
		[DataMember]
		public DateTime? PreorderAvailabilityDate
		{
			get
			{
				return _PreorderAvailabilityDate;
			}
			set
			{
				SetValue(ref _PreorderAvailabilityDate, () => this.PreorderAvailabilityDate, value);
			}
		}

		/// <summary>
		/// The date from when the backorder is allowed. 
		/// If not set AllowBackorder has no effect and not available
		/// </summary>
		private DateTime? _BackorderAvailabilityDate;
		[DataMember]
		public DateTime? BackorderAvailabilityDate
		{
			get
			{
				return _BackorderAvailabilityDate;
			}
			set
			{
				SetValue(ref _BackorderAvailabilityDate, () => this.BackorderAvailabilityDate, value);
			}
		}

		private string _Sku;
		[Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string Sku
		{
			get
			{
				return _Sku;
			}
			set
			{
				SetValue(ref _Sku, () => this.Sku, value);
			}
		}        
	}
}
