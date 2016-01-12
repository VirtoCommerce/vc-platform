using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ShippingPriceError : ValidationError
    {
        public ShippingPriceError() : base(typeof(ShippingPriceError))
        {
        }

        public Money NewPrice { get; set; }
    }
}