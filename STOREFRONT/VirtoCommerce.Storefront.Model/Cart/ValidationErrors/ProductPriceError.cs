using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ProductPriceError : ValidationError
    {
        public ProductPriceError(Money oldPrice)
        {
            OldPrice = oldPrice;
        }

        public Money OldPrice { get; private set; }
    }
}