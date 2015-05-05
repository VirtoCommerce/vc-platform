using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Commerce.Model
{
	public class SeoUrlKeyword : AuditableEntity
	{
		public string Language { get; set; }
		public string Keyword { get; set; }
		public string KeywordValue { get; set; }
		public bool IsActive { get; set; }
		public int KeywordType { get; set; }
		public string Title { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeywords { get; set; }
		public string ImageAltDescription { get; set; }

	}
}
