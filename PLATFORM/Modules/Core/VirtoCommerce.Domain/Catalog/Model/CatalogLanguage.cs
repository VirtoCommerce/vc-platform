
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Catalog.Model
{
    public class CatalogLanguage : Entity
    {
	    public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }

		public bool IsDefault { get; set; }
        public string LanguageCode { get; set; }
    }
}
