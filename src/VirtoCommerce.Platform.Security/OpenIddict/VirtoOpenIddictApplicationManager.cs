using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

namespace VirtoCommerce.Platform.Security.OpenIddict;

/// <summary>
/// Also finds the applications of self-registered clients. An application made from a client ID metadata document is
/// stored only when a user consents to it (<see cref="SelfRegisteredClientExtensions.EnsureCreatedAsync"/>).
/// </summary>
public class VirtoOpenIddictApplicationManager : OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>
{
    private readonly IClientIdMetadataDocumentResolver _documentResolver;
    private readonly ISelfRegisteredClientFactory _clientFactory;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IOptionsMonitor<AuthorizationOptions> _authorizationOptions;

    // OpenIddict looks a client up many times per request, and an unsaved application must keep its id until the consent stores it
    private readonly Dictionary<string, VirtoOpenIddictEntityFrameworkCoreApplication> _selfRegistered = new(StringComparer.Ordinal);

    public VirtoOpenIddictApplicationManager(
        IOpenIddictApplicationCache<VirtoOpenIddictEntityFrameworkCoreApplication> cache,
        ILogger<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictApplicationStore<VirtoOpenIddictEntityFrameworkCoreApplication> store,
        IClientIdMetadataDocumentResolver documentResolver,
        ISelfRegisteredClientFactory clientFactory,
        IOpenIddictAuthorizationManager authorizationManager,
        IOpenIddictTokenManager tokenManager,
        IOptionsMonitor<AuthorizationOptions> authorizationOptions)
        : base(cache, logger, options, store)
    {
        _documentResolver = documentResolver;
        _clientFactory = clientFactory;
        _authorizationManager = authorizationManager;
        _tokenManager = tokenManager;
        _authorizationOptions = authorizationOptions;
    }

    public override async ValueTask<VirtoOpenIddictEntityFrameworkCoreApplication> FindByClientIdAsync(string identifier, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(identifier);

        if (_selfRegistered.TryGetValue(identifier, out var resolved))
        {
            return resolved;
        }

        var application = await base.FindByClientIdAsync(identifier, cancellationToken);
        var registrationType = await this.GetRegistrationTypeAsync(application, cancellationToken);

        if (application != null && registrationType == null)
        {
            return application;
        }

        if (application == null && !_documentResolver.IsClientIdUrl(identifier))
        {
            return null;
        }

        application = registrationType == SelfRegisteredClientConstants.DynamicRegistration
            ? await RefreshDynamicallyRegisteredAsync(application, cancellationToken)
            : await ResolveClientIdMetadataDocumentAsync(identifier, application, cancellationToken);

        if (application != null)
        {
            _selfRegistered[identifier] = application;
        }

        return application;
    }

    /// <summary>
    /// Without resolving the metadata document of a self-registered client or updating its application.
    /// </summary>
    public virtual ValueTask<VirtoOpenIddictEntityFrameworkCoreApplication> FindStoredByClientIdAsync(string identifier, CancellationToken cancellationToken = default)
    {
        return base.FindByClientIdAsync(identifier, cancellationToken);
    }

    protected virtual async Task<VirtoOpenIddictEntityFrameworkCoreApplication> ResolveClientIdMetadataDocumentAsync(string clientId,
        VirtoOpenIddictEntityFrameworkCoreApplication stored, CancellationToken cancellationToken)
    {
        if (!_documentResolver.IsClientIdUrl(clientId))
        {
            // Turning the documents off stops the clients that registered with one
            return null;
        }

        var result = await _documentResolver.ResolveAsync(clientId, cancellationToken);
        if (result.Document == null)
        {
            // A document that can't be fetched for a while doesn't sign out the users of a stored client
            return result.IsTransient ? stored : null;
        }

        var document = result.Document;
        var descriptor = await _clientFactory.CreateDescriptorAsync(SelfRegisteredClientConstants.ClientIdMetadataDocument, clientId,
            document.ClientName ?? new Uri(clientId).Host, document.RedirectUris, cancellationToken);

        if (stored == null)
        {
            var application = new VirtoOpenIddictEntityFrameworkCoreApplication();
            await PopulateAsync(application, descriptor, cancellationToken);
            return application;
        }

        return await UpdateIfChangedAsync(stored, descriptor, cancellationToken);
    }

    protected virtual async Task<VirtoOpenIddictEntityFrameworkCoreApplication> RefreshDynamicallyRegisteredAsync(
        VirtoOpenIddictEntityFrameworkCoreApplication stored, CancellationToken cancellationToken)
    {
        if (!_authorizationOptions.CurrentValue.ClientRegistration.DynamicRegistration.Enabled)
        {
            return null;
        }

        var current = new OpenIddictApplicationDescriptor();
        await PopulateAsync(current, stored, cancellationToken);

        var descriptor = await _clientFactory.CreateDescriptorAsync(SelfRegisteredClientConstants.DynamicRegistration, current.ClientId,
            current.DisplayName, current.RedirectUris.Select(x => x.OriginalString), cancellationToken);

        return await UpdateIfChangedAsync(stored, descriptor, cancellationToken);
    }

    protected virtual async Task<VirtoOpenIddictEntityFrameworkCoreApplication> UpdateIfChangedAsync(
        VirtoOpenIddictEntityFrameworkCoreApplication stored, OpenIddictApplicationDescriptor descriptor, CancellationToken cancellationToken)
    {
        var current = new OpenIddictApplicationDescriptor();
        await PopulateAsync(current, stored, cancellationToken);

        if (IsUpToDate(current, descriptor))
        {
            return stored;
        }

        try
        {
            await UpdateAsync(stored, descriptor, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            Logger.LogWarning(ex, "The self-registered client {ClientId} could not be updated", current.ClientId);
            return stored;
        }

        // The users consented to the codes going to the old redirect URIs
        if (!current.RedirectUris.SetEquals(descriptor.RedirectUris))
        {
            await RevokeConsentsAsync(stored, cancellationToken);
        }

        return stored;
    }

    protected virtual bool IsUpToDate(OpenIddictApplicationDescriptor current, OpenIddictApplicationDescriptor descriptor)
    {
        return current.DisplayName == descriptor.DisplayName &&
            current.ApplicationType == descriptor.ApplicationType &&
            current.RedirectUris.SetEquals(descriptor.RedirectUris) &&
            current.Permissions.SetEquals(descriptor.Permissions) &&
            current.Requirements.SetEquals(descriptor.Requirements);
    }

    protected virtual async Task RevokeConsentsAsync(VirtoOpenIddictEntityFrameworkCoreApplication application, CancellationToken cancellationToken)
    {
        var id = await GetIdAsync(application, cancellationToken);

        var authorizations = await _authorizationManager.RevokeByApplicationIdAsync(id, cancellationToken);
        var tokens = await _tokenManager.RevokeByApplicationIdAsync(id, cancellationToken);

        Logger.LogInformation("The redirect URIs of the self-registered client {ClientId} changed: {Authorizations} consents and {Tokens} tokens are revoked",
            application.ClientId, authorizations, tokens);
    }
}
