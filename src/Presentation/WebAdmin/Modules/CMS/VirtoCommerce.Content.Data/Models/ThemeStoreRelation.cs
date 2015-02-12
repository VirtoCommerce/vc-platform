using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Data.Models
{
	public class ThemeStoreRelation : Entity
	{
		public string StoreId { get; set; }

		public string ThemeName { get; set; }
	}
}
