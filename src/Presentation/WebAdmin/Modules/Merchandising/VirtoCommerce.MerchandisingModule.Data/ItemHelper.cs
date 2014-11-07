using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.MerchandisingModule.Model;

namespace VirtoCommerce.MerchandisingModule.Data
{
    internal class ItemHelper
    {
        #region Item Methods

        public static T[] GetItems<T>(ICatalogRepository repository, string[] ids,
            ItemResponseGroups responseGroup = ItemResponseGroups.ItemSmall) where T:Item
        {
            if (ids == null || !ids.Any())
                return null;

            var query = repository.Items.Where(x => ids.Contains(x.ItemId)).OfType<T>();
            query = IncludeGroups(query, responseGroup);

            return (query).ToArray();
        }

        public static T[] GetItemsByCode<T>(ICatalogRepository repository, string[] codes,
    ItemResponseGroups responseGroup = ItemResponseGroups.ItemSmall) where T : Item
        {
            if (codes == null || !codes.Any())
                return null;

            var query = repository.Items.Where(x => codes.Contains(x.Code)).OfType<T>();
            query = IncludeGroups(query, responseGroup);

            return (query).ToArray();
        }

        private static IQueryable<T> IncludeGroups<T>(IQueryable<T> query,
            ItemResponseGroups responseGroups) where T:Item
        {
            if (responseGroups.HasFlag(ItemResponseGroups.ItemAssets))
            {
                query = query.Expand(p => p.ItemAssets);
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemAssociations))
            {
                query = query.Expand(p => p.AssociationGroups.Select(a => a.Associations.Select(s => s.CatalogItem)));
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemCategories))
            {
                query = query.Expand(p => p.CategoryItemRelations);
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemEditorialReviews))
            {
                query = query.Expand(p => p.EditorialReviews);
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemProperties))
            {
                query = query.Expand(p => p.ItemPropertyValues);
            }

            return query;
        }
        #endregion
    }
}
