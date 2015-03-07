using VirtoCommerce.Domain.Catalog.Services;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Extensions
{
    public static class ItemServiceExtensions
    {
        public static module.CatalogProduct GetByIdsOptimized(this IItemService service, string[] itemIds, module.ItemResponseGroup respGroup)
        {
            return null;
        }
    }
}
