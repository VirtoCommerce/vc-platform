using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ProductPriceError : ValidationError
    {
        public ProductPriceError() : base(typeof(ProductPriceError))
        {
        }

        public Money NewPrice { get; set; }
    }
}