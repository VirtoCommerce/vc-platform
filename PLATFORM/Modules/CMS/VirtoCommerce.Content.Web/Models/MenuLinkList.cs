using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Menu link list
	/// </summary>
	public class MenuLinkList
	{
		/// <summary>
		/// Id of menu link list
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// Name of menu link list
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Store id of menu link list
		/// </summary>
		public string StoreId { get; set; }
		/// <summary>
		/// Language of menu link list
		/// </summary>
		public string Language { get; set; }
		/// <summary>
		/// Collection of links
		/// </summary>
		public ICollection<MenuLink> MenuLinks { get; set; }
	}
}