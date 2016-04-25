using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class TierPrice : ValueObject<TierPrice>
    {
        public Money ListPrice { get; set; }

        public Money SalePrice { get; set; }

        public long Quantity { get; set; }
    }
}