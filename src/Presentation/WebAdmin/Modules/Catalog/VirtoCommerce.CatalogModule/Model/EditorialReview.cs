
namespace VirtoCommerce.CatalogModule.Model
{
	public class EditorialReview : ILanguageSupport
	{
		public string Id { get; set; }
	
		public string Content { get; set; }
		public string ReviewType { get; set; }

		#region ILanguageSupport Members
        public string LanguageCode { get; set; }
		#endregion
	}
}
