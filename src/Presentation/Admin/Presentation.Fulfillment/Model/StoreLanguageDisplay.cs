using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.ManagementClient.Stores.Model
{
    public class StoreLanguageDisplay
    {
        public StoreLanguage Language { get; set; }
        public string DisplayName { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as StoreLanguageDisplay;
            return (other != null) && other.Language != null && other.Language.LanguageCode.Equals(Language.LanguageCode, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Language.LanguageCode.GetHashCode();
        }
    }
}
