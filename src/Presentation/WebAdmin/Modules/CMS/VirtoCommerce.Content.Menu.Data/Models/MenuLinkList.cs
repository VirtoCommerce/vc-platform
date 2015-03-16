using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Menu.Data.Models
{
	public class MenuLinkList : Entity, IAuditable
	{
		public MenuLinkList()
		{
			CreatedDate = DateTime.UtcNow;
			MenuLinks = new NullCollection<MenuLink>();
		}

		[Required]
		public string Name { get; set; }
		[Required]
		public string StoreId { get; set; }

		[Required]
		public string Language { get; set; }

		[Required]
		public DateTime CreatedDate { get; set; }

		[Required]
		public string CreatedBy { get; set; }

		public DateTime? ModifiedDate { get; set; }

		public string ModifiedBy { get; set; }

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
