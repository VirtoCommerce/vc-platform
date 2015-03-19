using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Pages.Data.Models
{
	public class Page : Entity, IAuditable
	{
		public Page()
		{
			CreatedDate = DateTime.UtcNow;
		}

		[Required]
		public string Name { get; set; }
		[Required]
		public string Content { get; set; }
		[Required]
		public string Path { get; set; }
		[Required]
		public string Language { get; set; }

		#region IAuditable Methods

		[Required]
		public DateTime CreatedDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? ModifiedDate { get; set; }

		public string ModifiedBy { get; set; }

		#endregion
	}
}
