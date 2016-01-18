using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ShippingPriceError : ValidationError
    {
        public ShippingPriceError(Money newPrice) 
        {
            NewPrice = newPrice;
        }

        public Money NewPrice { get; private set; }
    }
}