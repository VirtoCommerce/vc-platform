using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Searching criteria of theme assets
	/// </summary>
	public class GetThemeAssetsCriteria
	{
		/// <summary>
		/// Load content of theme assets
		/// </summary>
		public bool LoadContent { get; set; }

		/// <summary>
		/// Last update date
		/// </summary>
		public DateTime? LastUpdateDate { get; set; }
	}
}