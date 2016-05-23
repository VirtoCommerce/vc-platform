using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using PagedList;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ShopConverter
    {
        public static Shop ToShopifyModel(this StorefrontModel.Store store, StorefrontModel.WorkContext workContext)
        {
            Shop result = new Shop();
            if (workContext.Categories != null)
            {
                result.Collections = new MutablePagedList<Collection>((pageNumber, pageSize) =>
                {
                    workContext.Categories.Slice(pageNumber, pageSize);
                    return new StaticPagedList<Collection>(workContext.Categories.Select(x => x.ToShopifyModel(workContext)), workContext.Categories);
                });
            }
            result.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(store);
            result.CustomerAccountsEnabled = true;
            result.CustomerAccountsOptional = true;
            result.Currency = workContext.CurrentCurrency.Code;
            result.Description = store.Description;
            result.Domain = store.Url;
            result.Email = store.Email;
            result.MoneyFormat = "";
            result.MoneyWithCurrencyFormat = "";
            result.Url = store.Url ?? "~/";
            result.Currencies = store.Currencies.Select(x => x.Code).ToArray();
            result.Languages = store.Languages.Select(x => x.ToShopifyModel()).ToArray();
            result.Catalog = store.Catalog;
            result.Status = store.StoreState.ToString();
            result.Metafields = new MetaFieldNamespacesCollection(new[] { new MetafieldsCollection("dynamic_properties", workContext.CurrentLanguage, store.DynamicProperties), new MetafieldsCollection("settings", store.Settings) });

            return result;
        }
    }
}
