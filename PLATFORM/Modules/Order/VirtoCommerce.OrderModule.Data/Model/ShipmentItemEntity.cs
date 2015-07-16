using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
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

		public virtual ShipmentPackageEntity ShipmentPackage { get; set; }
		public string ShipmentPackageId { get; set; }

	
	}
}
