using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class SeoInfo : ILanguageSupport
	{
		public string Id { get; set; }
	
		public string SemanticUrl { get; set; }
		public string PageTitle { get; set; }
		public string MetaDescription { get; set; }
		public string ImageAltDescription { get; set; }
        public string MetaKeywords { get; set; }

		#region ISupportLanguage Members

		public string LanguageCode { get; set; }

		#endregion
	}
}
