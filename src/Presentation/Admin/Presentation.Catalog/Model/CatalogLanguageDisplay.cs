using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Model
{
    public class CatalogLanguageDisplay
    {
        public CatalogLanguage Language { get; set; }
        public string DisplayName { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as CatalogLanguageDisplay;
            return (other != null) && other.Language != null && other.Language.Language.Equals(Language.Language, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Language.Language.GetHashCode();
        }
    }
}
