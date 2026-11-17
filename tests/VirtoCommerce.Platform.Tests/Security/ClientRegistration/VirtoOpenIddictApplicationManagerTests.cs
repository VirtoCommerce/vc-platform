using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class VirtoOpenIddictApplicationManagerTests
{
    private const string ClientId = "https://claude.ai/oauth/claude-code-client-metadata";

    private readonly SelfRegisteredClientTestContext _context = new();

    [Fact]
    public async Task FindByClientId_MetadataDocumentUrl_PublicClientWithTheLeastItNeeds()
    {
        // Arrange
        _context.SetDocument(ClientId, "Claude Code", "http://localhost/callback", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        application.Should().NotBeNull();
        var descriptor = await GetDescriptorAsync(manager, application);
        descriptor.ClientId.Should().Be(ClientId);
        descriptor.DisplayName.Should().Be("Claude Code");
        descriptor.ClientType.Should().Be(ClientTypes.Public);
        descriptor.ApplicationType.Should().Be(ApplicationTypes.Native);
        descriptor.ConsentType.Should().Be(ConsentTypes.Explicit);
        descriptor.RedirectUris.Select(x => x.OriginalString).Should().BeEquivalentTo("http://localhost/callback", "https://claude.ai/api/mcp/auth_callback");
        descriptor.Requirements.Should().BeEquivalentTo(Requirements.Features.ProofKeyForCodeExchange);
        descriptor.Permissions.Should().BeEquivalentTo(
            Permissions.Endpoints.Authorization,
            Permissions.Endpoints.Token,
            Permissions.GrantTypes.AuthorizationCode,
            Permissions.GrantTypes.RefreshToken,
            Permissions.ResponseTypes.Code,
            Permissions.Scopes.Profile,
            Permissions.Prefixes.Resource + SelfRegisteredClientTestContext.McpResource);
        (await manager.GetRegistrationTypeAsync(application, TestContext.Current.CancellationToken)).Should().Be(SelfRegisteredClientConstants.ClientIdMetadataDocument);
    }

    [Fact]
    public async Task FindByClientId_MetadataDocumentUrl_NotStoredUntilConsent()
    {
        // Arrange
        _context.SetDocument(ClientId, "Claude Code", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();

        // Act
        var first = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);
        var second = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);
        var storedBeforeConsent = _context.Stored.ToList();
        await manager.EnsureCreatedAsync(first, TestContext.Current.CancellationToken);
        await manager.EnsureCreatedAsync(first, TestContext.Current.CancellationToken);

        // Assert
        storedBeforeConsent.Should().BeEmpty();
        second.Should().BeSameAs(first, "the application keeps its id until the consent stores it");
        _context.Stored.Should().ContainKey(ClientId).WhoseValue.Should().BeSameAs(first);
    }

    [Fact]
    public async Task FindByClientId_NoClientName_NamedByTheHost()
    {
        // Arrange
        _context.SetDocument(ClientId, null, "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        (await manager.GetDisplayNameAsync(application, TestContext.Current.CancellationToken)).Should().Be("claude.ai");
    }

    [Fact]
    public async Task FindByClientId_WebRedirectUrisOnly_WebApplication()
    {
        // Arrange
        _context.SetDocument(ClientId, "Claude", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        (await manager.GetApplicationTypeAsync(application, TestContext.Current.CancellationToken)).Should().Be(ApplicationTypes.Web);
    }

    [Fact]
    public async Task FindByClientId_InvalidOrMissingDocument_NoApplication()
    {
        // Arrange
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeNull();
    }

    [Fact]
    public async Task FindByClientId_UnknownClientIdThatIsNotAUrl_NoApplication()
    {
        // Arrange
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync("claude-code", TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeNull();
        _context.Resolver.Invocations.Where(x => x.Method.Name == "ResolveAsync").Should().BeEmpty();
    }

    [Fact]
    public async Task FindByClientId_ApplicationOfAnAdministrator_Unchanged()
    {
        // Arrange
        var application = new VirtoOpenIddictEntityFrameworkCoreApplication { ClientId = ClientId, DisplayName = "Registered by hand" };
        _context.Stored[ClientId] = application;
        _context.SetDocument(ClientId, "From the document", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();

        // Act
        var result = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeSameAs(application);
        application.DisplayName.Should().Be("Registered by hand");
        _context.Updates.Should().Be(0);
    }

    [Fact]
    public async Task FindByClientId_StoredClientWhoseDocumentChanged_FollowsTheDocument()
    {
        // Arrange
        var stored = await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.SetDocument(ClientId, "Claude (new)", "https://claude.ai/api/mcp/auth_callback/v2");
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeSameAs(stored);
        _context.Updates.Should().Be(1);
        (await manager.GetDisplayNameAsync(application, TestContext.Current.CancellationToken)).Should().Be("Claude (new)");
        (await manager.GetRedirectUrisAsync(application, TestContext.Current.CancellationToken)).Should().BeEquivalentTo("https://claude.ai/api/mcp/auth_callback/v2");
    }

    [Fact]
    public async Task FindByClientId_StoredClientWithNewRedirectUris_RevokesTheConsentsAndTokens()
    {
        // Arrange
        var stored = await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.SetDocument(ClientId, "Claude", "https://evil.example.com/callback");
        var manager = _context.CreateManager();

        // Act
        await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        _context.AuthorizationManager.Verify(x => x.RevokeByApplicationIdAsync(stored.Id, It.IsAny<CancellationToken>()), Times.Once);
        _context.TokenManager.Verify(x => x.RevokeByApplicationIdAsync(stored.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByClientId_StoredClientWithNewName_KeepsTheConsents()
    {
        // Arrange
        await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.SetDocument(ClientId, "Claude (new)", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();

        // Act
        await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        _context.Updates.Should().Be(1);
        _context.AuthorizationManager.Invocations.Should().BeEmpty();
    }

    [Fact]
    public async Task FindByClientId_StoredClientWhoseDocumentIsTheSame_NotUpdated()
    {
        // Arrange
        await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();

        // Act
        await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        _context.Updates.Should().Be(0);
    }

    [Fact]
    public async Task FindByClientId_StoredClientWhenAModuleServesANewResource_GetsIt()
    {
        // Arrange
        await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.Resources.Add(new() { Uri = "https://platform.example.com/vendor/mcp", Kind = "mcp", AllowSelfRegisteredClients = true });
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        (await manager.HasPermissionAsync(application, Permissions.Prefixes.Resource + "https://platform.example.com/vendor/mcp", TestContext.Current.CancellationToken)).Should().BeTrue();
    }

    [Fact]
    public async Task FindByClientId_StoredClientWhoseDocumentCannotBeFetchedForAWhile_StillWorks()
    {
        // Arrange
        var stored = await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.Documents[ClientId] = ClientIdMetadataDocumentResult.Fail("Timeout", isTransient: true);
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeSameAs(stored);
    }

    [Fact]
    public async Task FindByClientId_StoredClientWhoseDocumentBecameInvalid_Refused()
    {
        // Arrange
        await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.Documents[ClientId] = ClientIdMetadataDocumentResult.Fail("The client_id does not match");
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeNull();
    }

    [Fact]
    public async Task FindByClientId_MetadataDocumentsTurnedOff_StoredClientRefused()
    {
        // Arrange
        await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.Options.ClientRegistration.ClientIdMetadataDocuments.Enabled = false;
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeNull();
    }

    [Fact]
    public async Task FindByClientId_DynamicallyRegisteredClient_KeepsItsRedirectUrisAndFollowsTheResources()
    {
        // Arrange
        _context.Options.ClientRegistration.DynamicRegistration.Enabled = true;
        var descriptor = await _context.Factory.CreateDescriptorAsync(SelfRegisteredClientConstants.DynamicRegistration, "dcr-client", "Agent", ["https://agent.example.com/callback"], TestContext.Current.CancellationToken);
        var setup = _context.CreateManager();
        await setup.CreateAsync(descriptor, TestContext.Current.CancellationToken);
        _context.Resources.Add(new() { Uri = "https://platform.example.com/vendor/mcp", Kind = "mcp", AllowSelfRegisteredClients = true });
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync("dcr-client", TestContext.Current.CancellationToken);

        // Assert
        (await manager.GetRedirectUrisAsync(application, TestContext.Current.CancellationToken)).Should().BeEquivalentTo("https://agent.example.com/callback");
        (await manager.HasPermissionAsync(application, Permissions.Prefixes.Resource + "https://platform.example.com/vendor/mcp", TestContext.Current.CancellationToken)).Should().BeTrue();
        _context.Resolver.Invocations.Where(x => x.Method.Name == "ResolveAsync").Should().BeEmpty();
    }

    [Fact]
    public async Task FindByClientId_DynamicRegistrationTurnedOff_DynamicallyRegisteredClientRefused()
    {
        // Arrange
        var descriptor = await _context.Factory.CreateDescriptorAsync(SelfRegisteredClientConstants.DynamicRegistration, "dcr-client", "Agent", ["https://agent.example.com/callback"],
            TestContext.Current.CancellationToken);
        await _context.CreateManager().CreateAsync(descriptor, TestContext.Current.CancellationToken);
        _context.Options.ClientRegistration.DynamicRegistration.Enabled = false;
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync("dcr-client", TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeNull();
    }

    [Fact]
    public async Task FindByClientId_StoredClientWhoseUpdateFails_Refused()
    {
        // Arrange
        var stored = await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.SetDocument(ClientId, "Claude", "https://evil.example.com/callback");
        _context.FailUpdates = true;
        var manager = _context.CreateManager();

        // Act
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        application.Should().BeNull("the application in memory has redirect URIs the users didn't consent to");
        _context.AuthorizationManager.Verify(x => x.RevokeByApplicationIdAsync(stored.Id, It.IsAny<CancellationToken>()), Times.Once,
            "the consents to the old redirect URIs are revoked before the update, so its failure can't keep them");
    }

    [Fact]
    public async Task EnsureCreated_AnotherConsentStoredTheClientMeanwhile_ReturnsTheStoredOne()
    {
        // Arrange
        _context.SetDocument(ClientId, "Claude Code", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);
        var otherManager = _context.CreateManager();
        var other = await otherManager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);
        await otherManager.EnsureCreatedAsync(other, TestContext.Current.CancellationToken);

        // Act
        var stored = await manager.EnsureCreatedAsync(application, TestContext.Current.CancellationToken);

        // Assert
        stored.Should().BeSameAs(other);
        _context.Stored.Should().ContainSingle().Which.Value.Should().BeSameAs(other);
    }

    [Fact]
    public async Task EnsureCreated_CreateFailsAndNothingIsStored_Throws()
    {
        // Arrange
        _context.SetDocument(ClientId, "Claude Code", "https://claude.ai/api/mcp/auth_callback");
        var manager = _context.CreateManager();
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);
        _context.FailCreates = true;

        // Act
        var act = async () => await manager.EnsureCreatedAsync(application, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>("the client found again is only in memory, not stored");
        _context.Stored.Should().BeEmpty();
    }

    [Fact]
    public async Task HasResourcePermissions_ResourceNoLongerAllowed_False()
    {
        // Arrange
        await StoreClientAsync("Claude", "https://claude.ai/api/mcp/auth_callback");
        _context.Resources.Single(x => x.Uri == SelfRegisteredClientTestContext.McpResource).AllowSelfRegisteredClients = false;
        var manager = _context.CreateManager();
        var application = await manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);

        // Act
        var allowed = await manager.HasResourcePermissionsAsync(application, [SelfRegisteredClientTestContext.McpResource], TestContext.Current.CancellationToken);

        // Assert
        allowed.Should().BeFalse("a refresh token must not keep a resource that no longer allows self-registered clients");
    }

    private async Task<VirtoOpenIddictEntityFrameworkCoreApplication> StoreClientAsync(string clientName, params string[] redirectUris)
    {
        _context.SetDocument(ClientId, clientName, redirectUris);
        var manager = _context.CreateManager();
        var application = await manager.FindByClientIdAsync(ClientId);
        await manager.EnsureCreatedAsync(application);
        return application;
    }

    private static async Task<OpenIddictApplicationDescriptor> GetDescriptorAsync(IOpenIddictApplicationManager manager, object application)
    {
        var descriptor = new OpenIddictApplicationDescriptor();
        await manager.PopulateAsync(descriptor, application);
        return descriptor;
    }
}
