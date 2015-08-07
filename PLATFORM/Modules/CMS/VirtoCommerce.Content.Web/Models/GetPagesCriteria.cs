using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Searching criteria of pages
	/// </summary>
	public class GetPagesCriteria
	{
		/// <summary>
		/// Last update date
		/// </summary>
		public DateTime? LastUpdateDate { get; set; }
	}
}