using Microsoft.AspNetCore.Authentication;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security.ExternalSignIn;

public static class AuthenticationPropertiesExtensions
{
    public const string StoreIdPropertyName = "store_id";
    public const string OidcUrlPropertyName = "oidc_url";
    public const string UserTypePropertyName = "user_type";

    public static void SetStoreId(this AuthenticationProperties properties, string value)
    {
        properties.Items[StoreIdPropertyName] = value;
    }

    public static string GetStoreId(this AuthenticationProperties properties)
    {
        return properties.Items.GetValueSafe(StoreIdPropertyName);
    }

    public static void SetOidcUrl(this AuthenticationProperties properties, string value)
    {
        properties.Items[OidcUrlPropertyName] = value;
    }

    public static string GetOidcUrl(this AuthenticationProperties properties)
    {
        return properties.Items.GetValueSafe(OidcUrlPropertyName);
    }

    public static void SetNewUserType(this AuthenticationProperties properties, string value)
    {
        properties.Items[UserTypePropertyName] = value;
    }

    public static string GetNewUserType(this AuthenticationProperties properties)
    {
        return properties.Items.GetValueSafe(UserTypePropertyName);
    }
}
