using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class ProductPromoEntry
    {
        public string Code { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string ProductId { get; set; }

        public object Owner { get; set; }

        public string Outline { get; set; }

        public ICollection<ProductPromoEntry> Variations { get; set; }

        public IDictionary<string, string> Attributes { get; set; }
    }
}