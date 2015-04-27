using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Data.Models
{
	public class MenuLink : Entity, IAuditable
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

		[Required]
		public DateTime CreatedDate { get; set; }

		[Required]
		public string CreatedBy { get; set; }

		public DateTime? ModifiedDate { get; set; }

		public string ModifiedBy { get; set; }

		public virtual MenuLinkList MenuLinkList { get; set; }
		public string MenuLinkListId { get; set; }
	}
}
