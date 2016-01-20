using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Quote.Model
{
	public class ShipmentMethod : ValueObject<ShipmentMethod>
	{
		public string ShipmentMethodCode { get; set; }
		public string OptionName { get; set; }
		public string Name { get; set; }
		public string LogoUrl { get; set; }
		public string Currency { get; set; }
		public decimal Price { get; set; }
	}
}
