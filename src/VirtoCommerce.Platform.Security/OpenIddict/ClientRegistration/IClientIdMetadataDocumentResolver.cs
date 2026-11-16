using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// Resolves a client id that is the HTTPS URL of the client's metadata document.
/// </summary>
public interface IClientIdMetadataDocumentResolver
{
    /// <summary>
    /// False for any client id when the documents are turned off.
    /// </summary>
    bool IsClientIdUrl(string clientId);

    Task<ClientIdMetadataDocumentResult> ResolveAsync(string clientId, CancellationToken cancellationToken = default);
}
