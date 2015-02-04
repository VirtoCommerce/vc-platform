using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
	public class ShipmentMethod
	{
		public string ShipmentMethodCode { get; set; }
		public string Name { get; set; }
		public string LogoUrl { get; set; }
		public string Currency { get; set; }
		public decimal Price { get; set; }
		public ICollection<Discount> Discounts { get; set; }
	}
}
