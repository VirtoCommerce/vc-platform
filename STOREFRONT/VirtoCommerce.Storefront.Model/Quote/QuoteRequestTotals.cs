using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteRequestTotals
    {
        public QuoteRequestTotals()
        {
        }

        public QuoteRequestTotals(Currency currency)
        {
            OriginalSubTotalExlTax = new Money(currency);
            SubTotalExlTax = new Money(currency);
            ShippingTotal = new Money(currency);
            DiscountTotal = new Money(currency);
            TaxTotal = new Money(currency);
            AdjustmentQuoteExlTax = new Money(currency);
            GrandTotalExlTax = new Money(currency);
            GrandTotalInclTax = new Money(currency);
        }

        public Money OriginalSubTotalExlTax { get; set; }

        public Money SubTotalExlTax { get; set; }

        public Money ShippingTotal { get; set; }

        public Money DiscountTotal { get; set; }

        public Money TaxTotal { get; set; }

        public Money AdjustmentQuoteExlTax { get; set; }

        public Money GrandTotalExlTax { get; set; }

        public Money GrandTotalInclTax { get; set; }
    }
}