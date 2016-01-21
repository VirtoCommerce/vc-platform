using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Converters
{
    public static class QuoteRequestConverter
    {
        public static QuoteRequest ToWebModel(this VirtoCommerceQuoteModuleWebModelQuoteRequest serviceModel)
        {
            var webModel = new QuoteRequest();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            var language = new Language(serviceModel.LanguageCode);
            var currency = new Currency(language, serviceModel.Currency);

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
                webModel.TaxDetails = serviceModel.TaxDetails.Select(td => td.ToWebModel()).ToList();
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

            return serviceModel;
        }
    }
}