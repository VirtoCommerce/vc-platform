using System;
using VirtoCommerce.Domain.Inventory.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.InventoryModule.Web.Model
{
	public class InventoryInfo : ValueObject<InventoryInfo>, IAuditable
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion
		public FulfillmentCenter FulfillmentCenter { get; set; }
		public string FulfillmentCenterId { get; set; }
	
		public string ProductId { get; set; }

		public long InStockQuantity { get; set; }
		public long ReservedQuantity { get; set; }
		public long ReorderMinQuantity { get; set; }
		public long PreorderQuantity { get; set; }
		public long BackorderQuantity { get; set; }
		public bool AllowBackorder { get; set; }
		public bool AllowPreorder { get; set; }
		public long InTransit { get; set; }
		public DateTime? PreorderAvailabilityDate { get; set; }
		public DateTime? BackorderAvailabilityDate { get; set; }
		public InventoryStatus Status { get; set; }
	}
}
