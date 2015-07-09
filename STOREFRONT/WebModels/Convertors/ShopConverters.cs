#region
using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.ApiClient.DataContracts.Stores;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;
using Data = VirtoCommerce.ApiClient.DataContracts;
using System.Collections.Generic;

#endregion

namespace VirtoCommerce.Web.Convertors
{
    public static class ShopConverters
    {
        #region Public Methods and Operators
        public static Shop AsWebModel(this Store store)
        {
            string[] paymentTypeIds = null;

            ICollection<PaymentMethod> paymentMethodModels = null;
            if (store.PaymentMethods != null)
            {
                paymentMethodModels = store.PaymentMethods.OrderBy(pm => pm.Priority).Select(paymentMethod => paymentMethod.AsWebModel()).ToList();
            }

            if (paymentMethodModels != null)
            {
                paymentTypeIds = GetPaymentMethodLogoIds(paymentMethodModels);
            }

            var shop = new Shop
                       {
                           StoreId = store.Id,
                           Name = store.Name,
                           Email = "support@no-email.com",
                           Description = store.Description,
                           Currency = store.DefaultCurrency,
                           Url = store.Url ?? String.Format("~/{0}/{1}", store.DefaultLanguage, store.Id).ToAbsoluteUrl(),
                           SimplifiedUrl = "~/".ToAbsoluteUrl().Trim('/'),
                           SecureUrl = store.SecureUrl,
                           DefaultUrl = store.SecureUrl ?? store.Url,
                           CustomerAccountsEnabled = true,
                           Domain = "localhost",
                           CustomerAccountsOptional = true,
                           EnabledPaymentTypes = paymentTypeIds,
                           DefaultLanguage = store.DefaultLanguage,
                           State = store.StoreState,
                           Catalog = store.Catalog,
                           PaymentMethods = paymentMethodModels,
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

        private static string[] GetPaymentMethodLogoIds(IEnumerable<PaymentMethod> paymentMethods)
        {
            var logoIds = new List<string>();

            if (paymentMethods.Any(pm =>
                !String.IsNullOrEmpty(pm.Group) && pm.Group.Equals("BankCard", StringComparison.OrdinalIgnoreCase)))
            {
                logoIds.Add(PaymentTypes.american_express.ToString());
                logoIds.Add(PaymentTypes.maestro.ToString());
                logoIds.Add(PaymentTypes.master.ToString());
                logoIds.Add(PaymentTypes.visa.ToString());
            }

            if (paymentMethods.Any(pm =>
                !String.IsNullOrEmpty(pm.Code) && pm.Code.IndexOf("PayPal", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                logoIds.Add(PaymentTypes.paypal.ToString());
            }

            return logoIds.ToArray();
        }

        private enum PaymentTypes
        {
            american_express,
            bitcoin,
            discover,
            google_wallet,
            maestro,
            master,
            paypal,
            visa
        }
        #endregion
    }
}