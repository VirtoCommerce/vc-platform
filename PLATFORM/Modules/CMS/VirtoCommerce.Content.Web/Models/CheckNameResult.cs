using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class CheckNameResult
	{
		/// <summary>
		/// Result of checking (if true - enable to save object, if false - unable to save object)
		/// </summary>
		public bool Result { get; set; }
	}
}