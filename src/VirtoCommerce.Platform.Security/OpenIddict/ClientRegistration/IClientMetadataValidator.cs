using System.Text.Json;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// The rules of the metadata that self-registered clients give. Replace the registration, or derive from
/// <see cref="ClientMetadataValidator"/>, to accept other clients.
/// </summary>
public interface IClientMetadataValidator
{
    bool IsClientIdUrl(string clientId);

    /// <summary>
    /// The metadata of a dynamic registration request, or the RFC 7591 error.
    /// </summary>
    ClientMetadataResult Validate(JsonElement metadata);

    ClientIdMetadataDocumentResult ParseDocument(string clientId, string json);
}
