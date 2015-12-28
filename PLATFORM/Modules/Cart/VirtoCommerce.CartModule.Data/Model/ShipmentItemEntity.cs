using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Data.Model
{
    public class ShipmentItemEntity : AuditableEntity
	{
		[StringLength(128)]
		public string BarCode { get; set; }

		public int Quantity { get; set; }
	
		public virtual LineItemEntity LineItem { get; set; }
		public string LineItemId { get; set; }

		public virtual ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }
	}
}
