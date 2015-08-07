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
		/// <summary>
		/// Theme name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Theme path
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Theme modified date
		/// </summary>
		public DateTime Modified { get; set; }
	}
}