using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Menu.Data.Models
{
	public class MenuLink : Entity, IAuditable
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Link { get; set; }

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
