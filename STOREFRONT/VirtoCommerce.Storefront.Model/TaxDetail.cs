using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class TaxDetail : ValueObject<TaxDetail>
    {
        public decimal Rate { get; set; }

        public Money Amount { get; set; }

        public string Name { get; set; }
    }
}