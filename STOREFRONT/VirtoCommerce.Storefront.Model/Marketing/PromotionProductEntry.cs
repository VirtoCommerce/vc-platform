using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Marketing
{
    public class PromotionProductEntry
    {
        public IDictionary<string, string> Attributes { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Code { get; set; }

        public Money Discount { get; set; }

        public string Outline { get; set; }

        public object Owner { get; set; }

        public Money Price { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public ICollection<PromotionProductEntry> Variations { get; set; }
    }
}