using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class TierPrice : ValueObject<TierPrice>
    {
        public Money Price { get; set; }

        public int Quantity { get; set; }
    }
}