namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public class ProductQuantityError : ValidationError
    {
        public ProductQuantityError(long availableQuantity) : base(typeof(ProductQuantityError))
        {
            AvailableQuantity = availableQuantity;
        }

        public long AvailableQuantity { get; private set; }
    }
}