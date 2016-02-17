namespace VirtoCommerce.Storefront.Common
{
    public static class StorefrontConstants
    {
        public const string StoreCookie = "vcf.store";
        public const string LanguageCookie = "vcf.language";
        public const string CurrencyCookie = "vcf.currency";
        public const string AnonymousCustomerIdCookie = "vcf.anonymous-customer-id";
        public const string PasswordResetTokenCookie = "vcf.password-reset-token";
        public const string CustomerIdCookie = "vcf.customer-id";
        public const string AuthenticationCookie = "vcf.authentication";
        public const string LoginOnBehalfUserIdCookie = "vcf.login-on-behalf-user-id";

        public const string AnonymousUsername = "Anonymous";

        public const string ManagerUserNameClaimType = "http://schemas.virtocommerce.com/ws/2016/02/identity/claims/managername";
        public const string ManagerUserIdClaimType = "http://schemas.virtocommerce.com/ws/2016/02/identity/claims/managernameidentifier";
    }
}
