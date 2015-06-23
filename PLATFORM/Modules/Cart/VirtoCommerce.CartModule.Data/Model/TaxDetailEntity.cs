using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Data.Model
{
	public class TaxDetailEntity : Entity
	{
		public decimal Rate { get; set; }
		[Column(TypeName = "Money")]
		public decimal Amount { get; set; }
		[StringLength(1024)]
		public string Name { get; set; }

		public virtual ShoppingCartEntity ShoppingCart { get; set; }
		public string ShoppingCartId { get; set; }

		public virtual ShipmentEntity Shipment { get; set; }
		public string ShipmentId { get; set; }

		public virtual LineItemEntity LineItem { get; set; }
		public string LineItemId { get; set; }
	}
}
