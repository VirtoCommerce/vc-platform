using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class DynamicClientRegistrationServiceTests
{
    private readonly SelfRegisteredClientTestContext _context = new();

    [Fact]
    public async Task Register_ValidatorOfAProject_ItsRulesApply()
    {
        // Arrange
        var service = CreateService(new NamedClientsOnlyValidator());

        // Act
        var unnamed = await service.RegisterAsync(Json("""{ "redirect_uris": ["https://agent.example.com/callback"] }"""), TestContext.Current.CancellationToken);
        var named = await service.RegisterAsync(Json("""{ "client_name": "Agent", "redirect_uris": ["https://agent.example.com/callback"] }"""),
            TestContext.Current.CancellationToken);

        // Assert
        unnamed.Error.Should().Be(ClientMetadata.Errors.InvalidClientMetadata);
        named.Error.Should().BeNull();
        _context.Stored.Should().ContainSingle().Which.Key.Should().Be(named.ClientId);
    }

    [Fact]
    public async Task Register_NoClientName_DefaultName()
    {
        // Arrange
        var service = CreateService(new ClientMetadataValidator(new OAuthAppRules()));

        // Act
        var result = await service.RegisterAsync(Json("""{ "redirect_uris": ["https://agent.example.com/callback"] }"""), TestContext.Current.CancellationToken);

        // Assert
        result.ClientName.Should().Be(DynamicClientRegistrationService.DefaultClientName);
        result.ClientId.Should().NotStartWith("https://");
    }

    [Fact]
    public async Task Register_ValidMetadata_ListsTheGrantAndResponseTypesOfTheClient()
    {
        // Arrange
        var service = CreateService(new ClientMetadataValidator(new OAuthAppRules()));

        // Act
        var result = await service.RegisterAsync(Json("""{ "redirect_uris": ["https://agent.example.com/callback"] }"""), TestContext.Current.CancellationToken);

        // Assert
        result.GrantTypes.Should().BeEquivalentTo(GrantTypes.AuthorizationCode, GrantTypes.RefreshToken);
        result.ResponseTypes.Should().BeEquivalentTo(ResponseTypes.Code);
    }

    private DynamicClientRegistrationService CreateService(IClientMetadataValidator validator)
    {
        return new DynamicClientRegistrationService(validator, _context.Factory, _context.CreateManager(), NullLogger<DynamicClientRegistrationService>.Instance);
    }

    private static JsonElement Json(string json)
    {
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    // A project that registers only clients that give their name
    private sealed class NamedClientsOnlyValidator() : ClientMetadataValidator(new OAuthAppRules())
    {
        public override ClientMetadataResult Validate(JsonElement metadata)
        {
            return GetString(metadata, ClientMetadata.Names.ClientName) == null
                ? ClientMetadataResult.Fail(ClientMetadata.Errors.InvalidClientMetadata, "The client_name is required.")
                : base.Validate(metadata);
        }
    }
}
