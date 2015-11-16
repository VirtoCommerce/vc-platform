using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class ForgotPassword : ValueObject<ForgotPassword>
    {
        public string Email { get; set; }
    }
}
