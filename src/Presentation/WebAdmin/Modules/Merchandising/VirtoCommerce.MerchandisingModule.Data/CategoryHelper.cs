using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Data
{
    using VirtoCommerce.Foundation.Catalogs.Model;
    using VirtoCommerce.Foundation.Catalogs.Repositories;

    internal static class CategoryHelper
    {
        public static T[] GetCategories<T>(ICatalogRepository repository, string catalogId, string outline) where T : CategoryBase
        {
            return repository.Categories.OfType<T>().Where(
                c => (string.IsNullOrEmpty(catalogId) || c.CatalogId == catalogId)).ToArray();
        }

    }
}
