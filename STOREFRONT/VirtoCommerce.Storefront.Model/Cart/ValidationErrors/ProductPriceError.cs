using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ProductPriceError : ValidationError
    {
        public ProductPriceError(Money newPrice) : base(typeof(ProductPriceError))
        {
            NewPrice = newPrice;
        }

        public Money NewPrice { get; private set; }
    }
}