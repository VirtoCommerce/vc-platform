using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class GetPagesCriteria
	{
		/// <summary>
		/// Max value of last updated date, if it's null returns all pages for store
		/// </summary>
		public DateTime? LastUpdateDate { get; set; }
	}
}