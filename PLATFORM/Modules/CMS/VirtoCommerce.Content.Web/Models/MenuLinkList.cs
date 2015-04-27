using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VirtoCommerce.Content.Web.Models
{
	public class MenuLinkList
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string StoreId { get; set; }
		public string Language { get; set; }

		public ICollection<MenuLink> MenuLinks { get; set; }
	}
}