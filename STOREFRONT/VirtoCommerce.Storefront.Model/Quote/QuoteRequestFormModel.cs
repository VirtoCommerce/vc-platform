using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteRequestFormModel
    {
        public QuoteRequestFormModel()
        {
            Items = new List<QuoteItemFormModel>();
        }

        public string Id { get; set; }

        public string Tag { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; }

        public Address BillingAddress { get; set; }

        public Address ShippingAddress { get; set; }

        public ICollection<QuoteItemFormModel> Items { get; set; }
    }
}