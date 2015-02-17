using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Data.Models
{
	public class Theme : Entity, IAuditable
	{
		public string Name { get; set; }

		public string ThemePath { get; set; }

		//[Required]
		//[StringLength(64)]
		public string CreatedBy { get; set; }

		//[Required]
		public DateTime CreatedDate { get; set; }

		//[StringLength(64)]
		public string ModifiedBy { get; set; }

		public DateTime? ModifiedDate { get; set; }
	}
}
