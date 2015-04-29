
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Catalog.Model
{
	public class EditorialReview : AuditableEntity, ILanguageSupport
	{
	
		public string Content { get; set; }
		public string ReviewType { get; set; }

		#region ILanguageSupport Members
        public string LanguageCode { get; set; }
		#endregion
	}
}
