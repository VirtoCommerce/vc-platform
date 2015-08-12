using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class GetThemeAssetsCriteria
	{
		/// <summary>
		/// If true - returns array of theme assets including binary or text content, if false - returns array of theme assets without content
		/// </summary>
		public bool LoadContent { get; set; }

		/// <summary>
        /// Max value of last updated date, if it's null returns all pages for store
		/// </summary>
		public DateTime? LastUpdateDate { get; set; }
	}
}