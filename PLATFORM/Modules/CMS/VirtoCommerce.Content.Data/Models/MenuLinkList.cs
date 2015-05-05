using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Models
{
	public class MenuLinkList : AuditableEntity
	{
		public MenuLinkList()
		{
			MenuLinks = new NullCollection<MenuLink>();
		}

		[Required]
		public string Name { get; set; }
		[Required]
		public string StoreId { get; set; }

		[Required]
		public string Language { get; set; }

		public virtual ICollection<MenuLink> MenuLinks { get; set; }

		public void Attach(MenuLinkList list)
		{
			this.Name = list.Name;
			this.Language = list.Language;

			foreach (var link in list.MenuLinks)
			{
				if (!this.MenuLinks.Any(l => l.Id == link.Id))
				{
					this.MenuLinks.Add(link);
				}
				else
				{
					var existingLink = this.MenuLinks.First(l => l.Id == link.Id);
					existingLink.Url = link.Url;
					existingLink.Title = link.Title;
					existingLink.IsActive = link.IsActive;
					existingLink.Priority = link.Priority;
					existingLink.Type = link.Type;
				}
			}
		}
	}
}
