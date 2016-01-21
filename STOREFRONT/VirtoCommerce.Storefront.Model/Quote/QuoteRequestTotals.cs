using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteRequestTotals
    {
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