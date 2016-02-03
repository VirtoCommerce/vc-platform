using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Converters
{
    public static class QuoteRequestTotalsConverter
    {
        public static QuoteRequestTotals ToWebModel(this VirtoCommerceQuoteModuleWebModelQuoteRequestTotals serviceModel, Currency currency)
        {
            var webModel = new QuoteRequestTotals(currency);

            webModel.AdjustmentQuoteExlTax = new Money(serviceModel.AdjustmentQuoteExlTax ?? 0, currency);
            webModel.DiscountTotal = new Money(serviceModel.DiscountTotal ?? 0, currency);
            webModel.GrandTotalExlTax = new Money(serviceModel.GrandTotalExlTax ?? 0, currency);
            webModel.GrandTotalInclTax = new Money(serviceModel.GrandTotalInclTax ?? 0, currency);
            webModel.OriginalSubTotalExlTax = new Money(serviceModel.OriginalSubTotalExlTax ?? 0, currency);
            webModel.ShippingTotal = new Money(serviceModel.ShippingTotal ?? 0, currency);
            webModel.SubTotalExlTax = new Money(serviceModel.SubTotalExlTax ?? 0, currency);
            webModel.TaxTotal = new Money(serviceModel.TaxTotal ?? 0, currency);

            return webModel;
        }

        public static VirtoCommerceQuoteModuleWebModelQuoteRequestTotals ToServiceModel(this QuoteRequestTotals webModel)
        {
            var serviceModel = new VirtoCommerceQuoteModuleWebModelQuoteRequestTotals();

            serviceModel.AdjustmentQuoteExlTax = (double)webModel.AdjustmentQuoteExlTax.Amount;
            serviceModel.DiscountTotal = (double)webModel.DiscountTotal.Amount;
            serviceModel.GrandTotalExlTax = (double)webModel.GrandTotalExlTax.Amount;
            serviceModel.GrandTotalInclTax = (double)webModel.GrandTotalInclTax.Amount;
            serviceModel.OriginalSubTotalExlTax = (double)webModel.OriginalSubTotalExlTax.Amount;
            serviceModel.ShippingTotal = (double)webModel.ShippingTotal.Amount;
            serviceModel.SubTotalExlTax = (double)webModel.SubTotalExlTax.Amount;
            serviceModel.TaxTotal = (double)webModel.TaxTotal.Amount;

            return serviceModel;
        }
    }
}
