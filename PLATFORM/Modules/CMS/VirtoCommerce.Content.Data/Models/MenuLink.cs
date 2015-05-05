using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Models
{
	public class MenuLink : AuditableEntity
	{
		public MenuLink()
		{
			CreatedDate = DateTime.UtcNow;
		}

		[Required]
		public string Title { get; set; }
		[Required]
		public string Url { get; set; }

		public string Type { get; set; }
		[Required]
		public bool IsActive { get; set; }
		[Required]
		public int Priority { get; set; }

		public virtual MenuLinkList MenuLinkList { get; set; }
		public string MenuLinkListId { get; set; }
	}
}
