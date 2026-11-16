using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// Dynamic client registration (RFC 7591).
/// </summary>
public interface IDynamicClientRegistrationService
{
    Task<DynamicClientRegistrationResult> RegisterAsync(JsonElement metadata, CancellationToken cancellationToken = default);
}
