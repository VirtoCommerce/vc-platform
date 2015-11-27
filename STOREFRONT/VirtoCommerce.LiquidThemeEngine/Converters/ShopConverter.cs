using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ShopConverter
    {
        public static Shop ToShopifyModel(this storefrontModel.Store store, storefrontModel.WorkContext workContext)
        {
            Shop result = new Shop();
            result.InjectFrom<NullableAndEnumValueInjection>(store);
            result.CustomerAccountsEnabled = true;
            result.CustomerAccountsOptional = true;
            result.Currency = workContext.CurrentCurrency.Code;
            result.CollectionsCount = "0";
            result.Description = store.Description;
            result.Domain = store.Url;
            result.Email = store.Email;
            result.MoneyFormat = "";
            result.MoneyWithCurrencyFormat = "";
            result.Url = store.Url ?? "~/";
            result.SimplifiedUrl = store.Url;
            result.Currencies = store.Currencies.Select(x => x.Code).ToArray();
            result.Languages = store.Languages.Select(x => x.ToShopifyModel()).ToArray();
            result.Catalog = store.Catalog;
            result.Metafields = new MetaFieldNamespacesCollection(new[] { new MetafieldsCollection("global", store.DynamicProperties) });

            return result;
        }
    }
}
