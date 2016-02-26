using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class TaxDetail 
    {
        public TaxDetail(Currency currency)
        {
            Rate = new Money(currency);
            Amount = new Money(currency);
        }
        public Money Rate { get; set; }

        public Money Amount { get; set; }

        public string Name { get; set; }
    }
}