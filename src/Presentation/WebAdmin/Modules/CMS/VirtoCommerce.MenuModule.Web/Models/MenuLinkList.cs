using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VirtoCommerce.MenuModule.Web.Models
{
	public class MenuLinkList
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string StoreId { get; set; }

		public IEnumerable<MenuLink> MenuLinks { get; set; }
	}
}