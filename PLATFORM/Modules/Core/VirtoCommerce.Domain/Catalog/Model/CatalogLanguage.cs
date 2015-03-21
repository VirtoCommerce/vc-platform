
namespace VirtoCommerce.Domain.Catalog.Model
{
    public class CatalogLanguage
    {
		public string Id { get; set; }
	    public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }

		public bool IsDefault { get; set; }
        public string LanguageCode { get; set; }
    }
}
