using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Content.Data.Models
{
	public class GetThemeAssetsCriteria
	{
		public bool LoadContent { get; set; }

		public DateTime? LastUpdateDate { get; set; }
	}
}
