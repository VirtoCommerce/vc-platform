namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// OAuth clients that register themselves, without an administrator. Section Authorization:ClientRegistration.
/// </summary>
public class OAuthClientRegistrationOptions
{
    public OAuthClientIdMetadataDocumentOptions ClientIdMetadataDocuments { get; set; } = new();

    public OAuthDynamicClientRegistrationOptions DynamicRegistration { get; set; } = new();
}
