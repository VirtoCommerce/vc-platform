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

        public const string AllowedStoresClaimType = "http://schemas.virtocommerce.com/ws/2016/02/identity/claims/allowedstores";
        public const string OperatorUserNameClaimType = "http://schemas.virtocommerce.com/ws/2016/02/identity/claims/operatorname";
        public const string OperatorUserIdClaimType = "http://schemas.virtocommerce.com/ws/2016/02/identity/claims/operatornameidentifier";
    }
}
