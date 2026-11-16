using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public class DynamicClientRegistrationService : IDynamicClientRegistrationService
{
    public const string DefaultClientName = "Unnamed client";

    private readonly IClientMetadataValidator _validator;
    private readonly ISelfRegisteredClientFactory _clientFactory;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly ILogger<DynamicClientRegistrationService> _logger;

    public DynamicClientRegistrationService(
        IClientMetadataValidator validator,
        ISelfRegisteredClientFactory clientFactory,
        IOpenIddictApplicationManager applicationManager,
        ILogger<DynamicClientRegistrationService> logger)
    {
        _validator = validator;
        _clientFactory = clientFactory;
        _applicationManager = applicationManager;
        _logger = logger;
    }

    public virtual async Task<DynamicClientRegistrationResult> RegisterAsync(JsonElement metadata, CancellationToken cancellationToken = default)
    {
        var result = _validator.Validate(metadata);
        if (result.Metadata == null)
        {
            return new DynamicClientRegistrationResult { Error = result.Error, ErrorDescription = result.ErrorDescription };
        }

        var clientId = CreateClientId();
        var descriptor = await _clientFactory.CreateDescriptorAsync(SelfRegisteredClientConstants.DynamicRegistration, clientId,
            result.Metadata.ClientName ?? DefaultClientName, result.Metadata.RedirectUris, cancellationToken);
        await _applicationManager.CreateAsync(descriptor, cancellationToken);

        _logger.LogInformation("The OAuth client {ClientId} ({ClientName}) registered itself", clientId, descriptor.DisplayName);

        return new DynamicClientRegistrationResult
        {
            ClientId = clientId,
            IssuedAt = DateTimeOffset.UtcNow,
            ClientName = descriptor.DisplayName,
            RedirectUris = result.Metadata.RedirectUris,
        };
    }

    /// <summary>
    /// Must not start with https://, or it would be taken for a client ID metadata document URL.
    /// </summary>
    protected virtual string CreateClientId()
    {
        return Guid.NewGuid().ToString("N");
    }
}
