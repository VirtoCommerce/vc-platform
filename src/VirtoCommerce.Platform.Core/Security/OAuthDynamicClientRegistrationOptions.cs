namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Dynamic client registration (RFC 7591) at /connect/register.
/// </summary>
public class OAuthDynamicClientRegistrationOptions
{
    /// <summary>
    /// Anyone may register a client, so it is off by default.
    /// </summary>
    public bool Enabled { get; set; }
}
