namespace VirtoCommerce.ApiClient.DataContracts.Quotes
{
    public class QuoteRequestTotals
    {
        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal ShippingTotal { get; set; }

        public decimal DiscountTotal { get; set; }

        public decimal TaxTotal { get; set; }
    }
}