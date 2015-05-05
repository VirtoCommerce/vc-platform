using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.InventoryModule.Data.Model
{
	public class Inventory : AuditableEntity
	{
		[StringLength(128)]
		[Required]
		public string FulfillmentCenterId { get; set; }

		[Required]
		public decimal InStockQuantity { get; set; }


		[Required]
		public decimal ReservedQuantity { get; set; }

		[Required]
		public decimal ReorderMinQuantity { get; set; }

		public decimal PreorderQuantity { get; set; }

		public decimal BackorderQuantity { get; set; }

		public bool AllowBackorder { get; set; }

		public bool AllowPreorder { get; set; }


		[Required]
		public int Status { get; set; }

		/// <summary>
		/// The date from when the preorder is allowed. 
		/// If not set AllowPreorder has no effect and not available
		/// </summary>
		public DateTime? PreorderAvailabilityDate { get; set; }

		/// <summary>
		/// The date from when the backorder is allowed. 
		/// If not set AllowBackorder has no effect and not available
		/// </summary>
		public DateTime? BackorderAvailabilityDate { get; set; }

		[Required]
		[StringLength(128)]
		public string Sku { get; set; }
	}
}
