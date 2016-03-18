using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Tax.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;

namespace VirtoCommerce.StoreModule.Web.Converters
{
    public static class StoreConverter
    {
        public static webModel.Store ToWebModel(this coreModel.Store store)
        {
            var retVal = new webModel.Store();
            retVal.InjectFrom(store);
            retVal.SeoInfos = store.SeoInfos;
            retVal.DefaultCurrency = store.DefaultCurrency;
            retVal.StoreState = store.StoreState;
            retVal.DynamicProperties = store.DynamicProperties;

            if (store.Settings != null)
                retVal.Settings = store.Settings.Select(x => x.ToWebModel()).ToList();

            if (store.ShippingMethods != null)
                retVal.ShippingMethods = store.ShippingMethods.Select(x => x.ToWebModel()).ToList();

            if (store.PaymentMethods != null)
                retVal.PaymentMethods = store.PaymentMethods.Select(x => x.ToWebModel()).ToList();

            if (store.TaxProviders != null)
                retVal.TaxProviders = store.TaxProviders.Select(x => x.ToWebModel()).ToList();

            if (store.Languages != null)
                retVal.Languages = store.Languages;

            if (store.Currencies != null)
                retVal.Currencies = store.Currencies;

            if (store.TrustedGroups != null)
                retVal.TrustedGroups = store.TrustedGroups;

            if (store.ReturnsFulfillmentCenter != null)
                retVal.ReturnsFulfillmentCenter = store.ReturnsFulfillmentCenter.ToWebModel();

            if (store.FulfillmentCenter != null)
                retVal.FulfillmentCenter = store.FulfillmentCenter.ToWebModel();


			return retVal;
        }

        public static coreModel.Store ToCoreModel(this webModel.Store store, ShippingMethod[] shippingMethods, PaymentMethod[] paymentMethods, TaxProvider[] taxProviders)
        {
            var retVal = new coreModel.Store();
            retVal.InjectFrom(store);
            retVal.SeoInfos = store.SeoInfos;
            retVal.StoreState = store.StoreState;
            retVal.DynamicProperties = store.DynamicProperties;

            if (store.Settings != null)
                retVal.Settings = store.Settings.Select(x => x.ToCoreModel()).ToList();

            if (store.ShippingMethods != null)
            {
                retVal.ShippingMethods = new List<ShippingMethod>();
                foreach (var shippingMethod in shippingMethods)
                {
                    var webShippingMethod = store.ShippingMethods.FirstOrDefault(x => x.Code == shippingMethod.Code);
                    if (webShippingMethod != null)
                    {
                        retVal.ShippingMethods.Add(webShippingMethod.ToCoreModel(shippingMethod));
                    }
                }
            }

            if (store.PaymentMethods != null)
            {
                retVal.PaymentMethods = new List<PaymentMethod>();
                foreach (var paymentMethod in paymentMethods)
                {
                    var webPaymentMethod = store.PaymentMethods.FirstOrDefault(x => x.Code == paymentMethod.Code);
                    if (webPaymentMethod != null)
                    {
                        retVal.PaymentMethods.Add(webPaymentMethod.ToCoreModel(paymentMethod));
                    }
                }
            }

            if (store.TaxProviders != null)
            {
                retVal.TaxProviders = new List<TaxProvider>();
                foreach (var taxProvider in taxProviders)
                {
                    var webTaxProvider = store.TaxProviders.FirstOrDefault(x => x.Code == taxProvider.Code);
                    if (webTaxProvider != null)
                    {
                        retVal.TaxProviders.Add(webTaxProvider.ToCoreModel(taxProvider));
                    }
                }
            }

            if (store.Languages != null)
                retVal.Languages = store.Languages;

            if (store.Currencies != null)
                retVal.Currencies = store.Currencies;

            if (store.TrustedGroups != null)
                retVal.TrustedGroups = store.TrustedGroups;

            if (store.ReturnsFulfillmentCenter != null)
                retVal.ReturnsFulfillmentCenter = store.ReturnsFulfillmentCenter.ToCoreModel();

            if (store.FulfillmentCenter != null)
                retVal.FulfillmentCenter = store.FulfillmentCenter.ToCoreModel();

            return retVal;
        }


    }
}
