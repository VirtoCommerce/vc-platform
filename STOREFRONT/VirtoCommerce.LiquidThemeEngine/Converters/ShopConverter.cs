using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ShopConverter
    {
        public static Shop ToShopifyModel(this StorefrontModel.Store store, StorefrontModel.WorkContext workContext)
        {
            Shop result = new Shop();
            result.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(store);
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
            result.Currencies = store.Currencies.Select(x => x.Code).ToArray();
            result.Languages = store.Languages.Select(x => x.ToShopifyModel()).ToArray();
            result.Catalog = store.Catalog;
            result.Status = store.StoreState.ToString();
            result.Metafields = new MetaFieldNamespacesCollection(new[] { new MetafieldsCollection("dynamic_properties", workContext.CurrentLanguage, store.DynamicProperties), new MetafieldsCollection("settings", store.Settings) });

            return result;
        }
    }
}
