using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Theme
	/// </summary>
	public class Theme
	{
		public string Name { get; set; }

		/// <summary>
		/// Theme path, contains store id
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Last modified date of any element in theme
		/// </summary>
		public DateTime Modified { get; set; }

        public string[] SecurityScopes { get; set; }
    }
}