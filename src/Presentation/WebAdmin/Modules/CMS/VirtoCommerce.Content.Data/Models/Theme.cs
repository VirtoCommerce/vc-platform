using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Data.Models
{
	public class Theme : Entity
	{
		public string Name { get; set; }

		public string ThemePath { get; set; }
	}
}
