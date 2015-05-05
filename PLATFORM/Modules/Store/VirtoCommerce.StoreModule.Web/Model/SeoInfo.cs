using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.StoreModule.Web.Model
{
	public class SeoInfo
	{
		public string Id { get; set; }

		public string SemanticUrl { get; set; }
		public string PageTitle { get; set; }
		public string MetaDescription { get; set; }
		public string ImageAltDescription { get; set; }
		public string LanguageCode { get; set; }

	}
}
