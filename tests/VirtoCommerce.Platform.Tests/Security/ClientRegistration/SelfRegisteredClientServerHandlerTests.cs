using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class SelfRegisteredClientServerHandlerTests
{
    private const string ClientId = "https://claude.ai/oauth/claude-code-client-metadata";

    private readonly SelfRegisteredClientTestContext _context = new();

    public SelfRegisteredClientServerHandlerTests()
    {
        _context.SetDocument(ClientId, "Claude Code", "http://localhost/callback");
    }

    [Fact]
    public async Task AuthorizationRequest_SelfRegisteredClientWithResourceAndS256_Accepted()
    {
        // Act
        var context = await ValidateAsync(ClientId, SelfRegisteredClientTestContext.McpResource, CodeChallengeMethods.Sha256);

        // Assert
        context.IsRejected.Should().BeFalse();
    }

    [Fact]
    public async Task AuthorizationRequest_SelfRegisteredClientWithoutResource_Refused()
    {
        // Act
        var context = await ValidateAsync(ClientId, resource: null, CodeChallengeMethods.Sha256);

        // Assert
        context.IsRejected.Should().BeTrue();
        context.Error.Should().Be(Errors.InvalidTarget);
    }

    [Fact]
    public async Task AuthorizationRequest_SelfRegisteredClientWithPlainChallenge_Refused()
    {
        // Act
        var context = await ValidateAsync(ClientId, SelfRegisteredClientTestContext.McpResource, CodeChallengeMethods.Plain);

        // Assert
        context.IsRejected.Should().BeTrue();
        context.Error.Should().Be(Errors.InvalidRequest);
    }

    [Fact]
    public async Task AuthorizationRequest_ClientOfAdministrator_Unchanged()
    {
        // Arrange
        _context.Stored["agent"] = new() { ClientId = "agent" };

        // Act
        var context = await ValidateAsync("agent", resource: null, codeChallengeMethod: null);

        // Assert
        context.IsRejected.Should().BeFalse();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task ConfigurationRequest_AdvertisesWaysToRegister(bool metadataDocuments, bool dynamicRegistration)
    {
        // Arrange
        _context.Options.ClientRegistration.ClientIdMetadataDocuments.Enabled = metadataDocuments;
        _context.Options.ClientRegistration.DynamicRegistration.Enabled = dynamicRegistration;
        var context = new HandleConfigurationRequestContext(CreateTransaction(new OpenIddictRequest()))
        {
            BaseUri = new Uri("https://platform.example.com/")
        };
        var handler = new AttachClientRegistrationMetadata(Mock.Of<IOptionsMonitor<AuthorizationOptions>>(x => x.CurrentValue == _context.Options));

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.Metadata.ContainsKey("client_id_metadata_document_supported").Should().Be(metadataDocuments);
        if (dynamicRegistration)
        {
            ((string)context.Metadata["registration_endpoint"]).Should().Be("https://platform.example.com/connect/register");
        }
        else
        {
            context.Metadata.Should().NotContainKey("registration_endpoint");
        }
        context.TokenEndpointAuthenticationMethods.Contains(ClientAuthenticationMethods.None).Should().Be(metadataDocuments || dynamicRegistration);
    }

    private async Task<ValidateAuthorizationRequestContext> ValidateAsync(string clientId, string resource, string codeChallengeMethod)
    {
        var request = new OpenIddictRequest { ClientId = clientId, CodeChallengeMethod = codeChallengeMethod };
        if (resource != null)
        {
            request.Resources = [resource];
        }

        var context = new ValidateAuthorizationRequestContext(CreateTransaction(request));

        await new ValidateSelfRegisteredClientAuthorizationRequest(_context.CreateManager()).HandleAsync(context);

        return context;
    }

    private static OpenIddictServerTransaction CreateTransaction(OpenIddictRequest request)
    {
        return new OpenIddictServerTransaction
        {
            Request = request,
            Options = new OpenIddictServerOptions(),
            Logger = NullLogger.Instance,
        };
    }
}
