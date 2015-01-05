using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Inventory.Model
{
	public class InventoryInfo : ValueObject<InventoryInfo>
	{
		public string ProductId { get; set; }

		public long Stock { get; set; }
		public long Reserved { get; set; }
		public long InTransit { get; set; }
	}
}
