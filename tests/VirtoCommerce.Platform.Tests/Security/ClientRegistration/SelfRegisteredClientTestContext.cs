using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

/// <summary>
/// The real application manager and EF Core store logic over a dictionary of stored applications.
/// </summary>
public class SelfRegisteredClientTestContext
{
    public const string McpResource = "https://platform.example.com/admin/mcp";

    public Dictionary<string, VirtoOpenIddictEntityFrameworkCoreApplication> Stored { get; } = new(StringComparer.Ordinal);

    public Dictionary<string, ClientIdMetadataDocumentResult> Documents { get; } = new(StringComparer.Ordinal);

    public List<OAuthResource> Resources { get; } =
    [
        new() { Uri = McpResource, Kind = "mcp", AllowSelfRegisteredClients = true },
        new() { Uri = "https://platform.example.com/ucp/mcp", Kind = "mcp" },
        new() { Uri = "https://platform.example.com/api", Kind = "api" },
    ];

    public AuthorizationOptions Options { get; } = new();

    public int Updates { get; private set; }

    public bool FailUpdates { get; set; }

    public Mock<IClientIdMetadataDocumentResolver> Resolver { get; } = new();

    public Mock<IOpenIddictAuthorizationManager> AuthorizationManager { get; } = new();

    public Mock<IOpenIddictTokenManager> TokenManager { get; } = new();

    public SelfRegisteredClientFactory Factory { get; }

    public SelfRegisteredClientTestContext()
    {
        var provider = new Mock<IOAuthResourceProvider>();
        provider
            .Setup(x => x.GetResourcesAsync(It.IsAny<OAuthResourceContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => Resources.ToList());

        Factory = new SelfRegisteredClientFactory([provider.Object], new HttpContextAccessor(), NullLogger<SelfRegisteredClientFactory>.Instance);

        Resolver
            .Setup(x => x.IsClientIdUrl(It.IsAny<string>()))
            .Returns((string clientId) => Options.ClientRegistration.ClientIdMetadataDocuments.Enabled && new ClientMetadataValidator(new OAuthAppRules()).IsClientIdUrl(clientId));
        Resolver
            .Setup(x => x.ResolveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string clientId, CancellationToken _) => Documents.GetValueOrDefault(clientId) ?? ClientIdMetadataDocumentResult.Fail("Not found"));
    }

    public VirtoOpenIddictApplicationManager CreateManager()
    {
        var store = new Mock<OpenIddictEntityFrameworkCoreApplicationStore<VirtoOpenIddictEntityFrameworkCoreApplication, VirtoOpenIddictEntityFrameworkCoreAuthorization, VirtoOpenIddictEntityFrameworkCoreToken, string>>(
            new MemoryCache(new MemoryCacheOptions()),
            Mock.Of<IOpenIddictEntityFrameworkCoreContext>(),
            Mock.Of<IOptionsMonitor<OpenIddictEntityFrameworkCoreOptions>>(x => x.CurrentValue == new OpenIddictEntityFrameworkCoreOptions()))
        {
            CallBase = true,
        };

        store
            .Setup(x => x.FindByClientIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns((string clientId, CancellationToken _) => ValueTask.FromResult(Stored.GetValueOrDefault(clientId)));
        store
            .Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns((string id, CancellationToken _) => ValueTask.FromResult(Stored.Values.FirstOrDefault(x => x.Id == id)));
        store
            .Setup(x => x.CreateAsync(It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()))
            .Returns((VirtoOpenIddictEntityFrameworkCoreApplication app, CancellationToken _) =>
            {
                // The client id is unique, as in the database
                if (!Stored.TryAdd(app.ClientId!, app))
                {
                    throw new InvalidOperationException($"The client {app.ClientId} is already stored.");
                }

                return ValueTask.CompletedTask;
            });
        store
            .Setup(x => x.UpdateAsync(It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                if (FailUpdates)
                {
                    throw new InvalidOperationException("The database is not available.");
                }

                Updates++;
                return ValueTask.CompletedTask;
            });

        return new VirtoOpenIddictApplicationManager(
            Mock.Of<IOpenIddictApplicationCache<VirtoOpenIddictEntityFrameworkCoreApplication>>(),
            NullLogger<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>>.Instance,
            Mock.Of<IOptionsMonitor<OpenIddictCoreOptions>>(x => x.CurrentValue == new OpenIddictCoreOptions { DisableEntityCaching = true }),
            store.Object,
            Resolver.Object,
            Factory,
            AuthorizationManager.Object,
            TokenManager.Object,
            Mock.Of<IOptionsMonitor<AuthorizationOptions>>(x => x.CurrentValue == Options));
    }

    public void SetDocument(string clientId, string clientName, params string[] redirectUris)
    {
        Documents[clientId] = ClientIdMetadataDocumentResult.Success(new ClientIdMetadataDocument
        {
            ClientId = clientId,
            ClientName = clientName,
            RedirectUris = redirectUris,
        });
    }
}
