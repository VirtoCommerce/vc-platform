using System.Collections.Generic;

namespace VirtoCommerce.MenuModule.Web.Models
{
	public class MenuLinkList
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string StoreId { get; set; }
		public string Language { get; set; }

		public IEnumerable<MenuLink> MenuLinks { get; set; }
	}
}