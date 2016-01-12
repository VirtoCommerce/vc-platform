namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ProductQuantityError : ValidationError
    {
        public ProductQuantityError() : base(typeof(ProductQuantityError))
        {
        }

        public long AvailableQuantity { get; set; }
    }
}