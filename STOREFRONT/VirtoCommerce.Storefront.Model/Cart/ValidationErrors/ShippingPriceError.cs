using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ShippingPriceError : ValidationError
    {
        public ShippingPriceError(Money oldPrice) 
        {
            OldPrice = oldPrice;
        }

        public Money OldPrice { get; private set; }
    }
}