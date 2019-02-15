namespace VirtoCommerce.Platform.Core.Security
{
    public static class VirtoCommerceClaimTypes
    {
        public const string UserName = "http://schemas.virtocommerce.com/ws/2016/02/identity/claims/username";
        /// <summary>
        ///   Permissions that will be granted to the user by cookies when bearer token authentication is enabled.
        ///   This can help to authorize the user for direct(non-AJAX) GET requests to the VC platform API and/or to use
        ///   some 3rd-party web applications for the VC platform(like Hangfire dashboard).
        /// </summary>
        public const string LimitedPermissionsClaimName = "LimitedPermissions";
    }
}
