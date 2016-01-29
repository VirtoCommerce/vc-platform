namespace VirtoCommerce.Storefront.Model.Cart.ValidationErrors
{
    public abstract class ValidationError
    {
        public ValidationError()
        {
            ErrorCode = GetType().Name;
        }

        public string ErrorCode { get; private set; }
    }
}