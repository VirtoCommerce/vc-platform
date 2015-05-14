#region
using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.ApiClient.DataContracts.Stores;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;
using Data = VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.Web.Convertors
{
    public static class ShopConverters
    {
        #region Public Methods and Operators
        public static Shop AsWebModel(this Store store)
        {
            var shop = new Shop
                       {
                           StoreId = store.Id,
                           Name = store.Name,
                           Email = "support@no-email.com",
                           Description = store.Description,
                           Currency = store.Currencies.FirstOrDefault(),
                           Url = store.Url ?? String.Format("~/{0}/{1}", store.DefaultLanguage, store.Id).ToAbsoluteUrl(),
                           SecureUrl = store.SecureUrl,
                           CustomerAccountsEnabled = true,
                           Domain = "localhost",
                           CustomerAccountsOptional = true,
                           EnabledPaymentTypes = new[] { "amex", "paypal", "google", "bitcoin" },
                           DefaultLanguage = store.DefaultLanguage,
                           State = store.StoreState,
                           Catalog = store.Catalog,
                           Languages = store.Languages,
                           Currencies = store.Currencies
                       };

            if (store.Seo != null)
            {
                shop.Keywords = store.Seo.Select(k => k.AsWebModel());
            }

            if (store.Settings != null && store.Settings.Any())
            {
                var fieldsCollection = new MetafieldsCollection("global", store.Settings);
                shop.Metafields = new MetaFieldNamespacesCollection(new[] { fieldsCollection });
            }

            return shop;
        }

        public static SeoKeyword AsWebModel(this Data.SeoKeyword keyword)
        {
            var ret = new SeoKeyword();
            ret.InjectFrom(keyword);
            return ret;
        }
        #endregion
    }
}