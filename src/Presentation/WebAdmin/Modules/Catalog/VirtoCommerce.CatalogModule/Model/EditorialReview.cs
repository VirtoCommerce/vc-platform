using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Model
{
	public class EditorialReview : ILanguageSupport
	{
		public string Id { get; set; }
	
		public string Content { get; set; }

		#region ILanguageSupport Members
		public string LanguageCode { get; set;	}
		#endregion
	}
}
