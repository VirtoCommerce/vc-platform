using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class ResetPassword : ValueObject<ForgotPassword>
    {
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
