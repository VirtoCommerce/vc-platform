using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Common
{
    public static class LocalizationExtension
    {
        public static T SelectForLanguage<T>(this IEnumerable<T> items, Language language) where T : IHasLanguage
        {
            return items.FirstOrDefault(i => i.Language.Equals(language));
        }
    }
}