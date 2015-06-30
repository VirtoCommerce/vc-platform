using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Catalog.Model
{
	public class Image : AuditableEntity, ISeoSupport, ILanguageSupport
	{
		public string Name { get; set; }
		public string Url { get; set; }
	    public string Group { get; set; }
		public int SortOrder { get; set; }

		#region ISeoSupport Members

		public ICollection<SeoInfo> SeoInfos { get; set; }

		#endregion

		#region ILanguageSupport Members

		public string LanguageCode { get; set; }

		#endregion
	}
}
