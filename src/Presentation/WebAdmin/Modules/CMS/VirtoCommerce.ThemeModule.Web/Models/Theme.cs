using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.ThemeModule.Web.Models
{
	public class Theme
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public DateTime Modified { get; set; }
	}
}