using FluentAssertions;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class ClientMetadataValidatorTests
{
    private const string ClientId = "https://agent.example.com/oauth/client.json";

    private readonly ClientMetadataValidator _validator = new(new OAuthAppRules());

    [Theory]
    [InlineData("https://agent.example.com/oauth/client.json")]
    [InlineData("https://agent.example.com:8443/client")]
    [InlineData("https://agent.example.com/client?version=2")]
    public void IsClientIdUrl_HttpsUrlWithPath_True(string clientId)
    {
        // Act
        var result = _validator.IsClientIdUrl(clientId);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("claude-code")]
    [InlineData("http://agent.example.com/client.json")]
    [InlineData("https://agent.example.com")]
    [InlineData("https://agent.example.com/")]
    [InlineData("https://agent.example.com/client.json#fragment")]
    [InlineData("https://user:secret@agent.example.com/client.json")]
    [InlineData("https://agent.example.com/oauth/../client.json")]
    [InlineData("https://agent.example.com/oauth/./client.json")]
    [InlineData("https://agent.example.com/oauth/%2e%2e/client.json")]
    public void IsClientIdUrl_NotAMetadataDocumentUrl_False(string clientId)
    {
        // Act
        var result = _validator.IsClientIdUrl(clientId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Parse_ValidDocument_ReturnsTheClient()
    {
        // Arrange
        var json = $$"""
            {
              "client_id": "{{ClientId}}",
              "client_name": "  Agent  ",
              "redirect_uris": ["https://agent.example.com/callback", "http://localhost/callback", "https://agent.example.com/callback"],
              "grant_types": ["authorization_code", "refresh_token", "client_credentials"],
              "response_types": ["code"],
              "token_endpoint_auth_method": "none"
            }
            """;

        // Act
        var (document, error) = Parse(json);

        // Assert
        error.Should().BeNull();
        document.ClientId.Should().Be(ClientId);
        document.ClientName.Should().Be("Agent");
        document.RedirectUris.Should().Equal("https://agent.example.com/callback", "http://localhost/callback");
    }

    [Fact]
    public void Parse_NoClientName_NameIsNull()
    {
        // Act
        var (document, _) = Parse(Document());

        // Assert
        document.ClientName.Should().BeNull();
    }

    [Fact]
    public void Parse_LongClientName_IsCut()
    {
        // Act
        var (document, _) = Parse(Document(extra: $"\"client_name\": \"{new string('a', 300)}\","));

        // Assert
        document.ClientName.Should().HaveLength(ClientMetadata.MaxClientNameLength);
    }

    [Theory]
    [InlineData("not json")]
    [InlineData("[]")]
    public void Parse_NotAJsonObject_Refused(string json)
    {
        // Act
        var result = Parse(json);

        // Assert
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void Parse_ClientIdOfAnotherUrl_Refused()
    {
        // Act
        var (document, error) = Parse(Document(clientId: "https://evil.example.com/client.json"));

        // Assert
        document.Should().BeNull();
        error.Should().Contain("client_id");
    }

    [Theory]
    [InlineData("\"token_endpoint_auth_method\": \"client_secret_basic\",")]
    [InlineData("\"client_secret\": \"secret\",")]
    [InlineData("\"grant_types\": [\"client_credentials\"],")]
    [InlineData("\"grant_types\": \"authorization_code\",")]
    [InlineData("\"response_types\": [\"token\"],")]
    public void Parse_NotAPublicCodeClient_Refused(string extra)
    {
        // Arrange
        var json = Document(extra: extra, authMethod: extra.Contains("token_endpoint_auth_method") ? null : "none");

        // Act
        var result = Parse(json);

        // Assert
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void Parse_NoTokenEndpointAuthMethod_Refused()
    {
        // Act
        var result = Parse(Document(authMethod: null));

        // Assert
        result.Error.Should().Contain("token_endpoint_auth_method");
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("\"https://agent.example.com/callback\"")]
    [InlineData("[\"http://agent.example.com/callback\"]")]
    [InlineData("[\"javascript:alert(1)\"]")]
    [InlineData("[\"https://agent.example.com/callback#fragment\"]")]
    public void Parse_InvalidRedirectUris_Refused(string redirectUris)
    {
        // Act
        var result = Parse(Document(redirectUris: redirectUris));

        // Assert
        result.Error.Should().NotBeNull();
    }

    private (ClientIdMetadataDocument Document, string Error) Parse(string json)
    {
        var result = _validator.ParseDocument(ClientId, json);
        return (result.Document, result.Error);
    }

    private static string Document(string clientId = ClientId, string redirectUris = "[\"https://agent.example.com/callback\"]",
        string authMethod = "none", string extra = "")
    {
        var method = authMethod == null ? "" : $"\"token_endpoint_auth_method\": \"{authMethod}\",";

        return $$"""{ {{extra}} {{method}} "client_id": "{{clientId}}", "redirect_uris": {{redirectUris}} }""";
    }
}
