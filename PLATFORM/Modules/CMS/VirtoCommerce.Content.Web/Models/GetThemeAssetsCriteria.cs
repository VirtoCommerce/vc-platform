using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class GetThemeAssetsCriteria
	{
		public bool LoadContent { get; set; }

		public DateTime? LastUpdateDate { get; set; }
	}
}