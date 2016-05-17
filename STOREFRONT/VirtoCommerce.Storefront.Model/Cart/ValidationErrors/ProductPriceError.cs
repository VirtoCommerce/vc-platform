using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ProductPriceError : ValidationError
    {
        public ProductPriceError(Money oldPrice, Money oldPriceWithTax)
        {
            OldPrice = oldPrice;
            OldPriceWithTax = oldPriceWithTax;
        }

        public Money OldPrice { get; private set; }
        public Money OldPriceWithTax { get; private set; }
    }
}