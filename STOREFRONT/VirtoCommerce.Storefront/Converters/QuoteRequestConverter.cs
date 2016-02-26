using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Quote;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Converters
{
    public static class QuoteRequestConverter
    {

        public static QuoteRequest ToWebModel(this VirtoCommerceQuoteModuleWebModelQuoteRequest serviceModel, IEnumerable<Currency> availCurrencies, Language language)
        {
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(serviceModel.Currency)) ?? new Currency(language, serviceModel.Currency);
            var webModel = new QuoteRequest(currency, language);

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);
           
            webModel.Currency = currency;
            webModel.Language = language;
            webModel.ManualRelDiscountAmount = new Money(serviceModel.ManualRelDiscountAmount ?? 0, currency);
            webModel.ManualShippingTotal = new Money(serviceModel.ManualShippingTotal ?? 0, currency);
            webModel.ManualSubTotal = new Money(serviceModel.ManualSubTotal ?? 0, currency);

            if (serviceModel.Addresses != null)
            {
                webModel.Addresses = serviceModel.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            if (serviceModel.Attachments != null)
            {
                webModel.Attachments = serviceModel.Attachments.Select(a => a.ToWebModel()).ToList();
            }

            if (!string.IsNullOrEmpty(serviceModel.Coupon))
            {
                webModel.Coupon = new Coupon { AppliedSuccessfully = true, Code = serviceModel.Coupon };
            }

            if (serviceModel.DynamicProperties != null)
            {
                webModel.DynamicProperties = serviceModel.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            if (serviceModel.Items != null)
            {
                webModel.Items = serviceModel.Items.Select(i => i.ToWebModel(currency)).ToList();
            }

            // TODO
            if (serviceModel.ShipmentMethod != null)
            {
                
            }

            if (serviceModel.TaxDetails != null)
            {
                webModel.TaxDetails = serviceModel.TaxDetails.Select(td => td.ToWebModel(currency)).ToList();
            }

            if (serviceModel.Totals != null)
            {
                webModel.Totals = serviceModel.Totals.ToWebModel(currency);
            }

            return webModel;
        }

        public static VirtoCommerceQuoteModuleWebModelQuoteRequest ToServiceModel(this QuoteRequest webModel)
        {
            var serviceModel = new VirtoCommerceQuoteModuleWebModelQuoteRequest();

            serviceModel.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            serviceModel.Currency = webModel.Currency.Code;
            serviceModel.Addresses = webModel.Addresses.Select(a => a.ToQuoteServiceModel()).ToList();
            serviceModel.Attachments = webModel.Attachments.Select(a => a.ToQuoteServiceModel()).ToList();
            serviceModel.DynamicProperties = webModel.DynamicProperties.Select(dp => dp.ToServiceModel()).ToList();
            serviceModel.Items = webModel.Items.Select(i => i.ToQuoteServiceModel()).ToList();
            serviceModel.LanguageCode = webModel.Language.CultureName;
            serviceModel.ManualRelDiscountAmount = webModel.ManualRelDiscountAmount != null ? (double?)webModel.ManualRelDiscountAmount.Amount : null;
            serviceModel.ManualShippingTotal = webModel.ManualShippingTotal != null ? (double?)webModel.ManualShippingTotal.Amount : null;
            serviceModel.ManualSubTotal = webModel.ManualSubTotal != null ? (double?)webModel.ManualSubTotal.Amount : null;
            serviceModel.TaxDetails = webModel.TaxDetails.Select(td => td.ToServiceModel()).ToList();

            if (webModel.Coupon != null && webModel.Coupon.AppliedSuccessfully)
            {
                serviceModel.Coupon = webModel.Coupon.Code;
            }

            if (webModel.Totals != null)
            {
                serviceModel.Totals = webModel.Totals.ToServiceModel();
            }

            return serviceModel;
        }
    }
}