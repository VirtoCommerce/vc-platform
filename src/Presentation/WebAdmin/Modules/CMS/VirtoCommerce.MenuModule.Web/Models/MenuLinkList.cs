using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VirtoCommerce.MenuModule.Web.Models
{
	public class MenuLinkList
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string StoreId { get; set; }
		public string Language { get; set; }

		private Collection<MenuLink> _menuLinks;
		public Collection<MenuLink> MenuLinks { get { return _menuLinks ?? (_menuLinks = new Collection<MenuLink>()); } }
	}
}