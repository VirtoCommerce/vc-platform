namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public static class SelfRegisteredClientConstants
{
    /// <summary>
    /// The application property with <see cref="ClientIdMetadataDocument"/> or <see cref="DynamicRegistration"/>.
    /// </summary>
    public const string RegistrationProperty = "vc_registration";

    public const string ClientIdMetadataDocument = "client_id_metadata_document";

    public const string DynamicRegistration = "dynamic_registration";

    /// <summary>
    /// The authorization property with the resources the user consented to: an authorization of a self-registered
    /// client counts only for them.
    /// </summary>
    public const string ResourcesProperty = "vc_resources";
}
