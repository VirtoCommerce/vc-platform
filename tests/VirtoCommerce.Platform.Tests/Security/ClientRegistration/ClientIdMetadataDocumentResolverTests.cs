using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class ClientIdMetadataDocumentResolverTests
{
    private const string ClientId = "https://agent.example.com/oauth/client.json";

    private readonly AuthorizationOptions _options = new();
    private readonly FakeHandler _handler = new();
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());

    [Fact]
    public async Task Resolve_ValidDocument_ReturnsItAndCachesIt()
    {
        // Arrange
        _handler.Respond(ClientId, Json(Document()));
        var resolver = CreateResolver();

        // Act
        var first = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);
        var second = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        first.Document.Should().NotBeNull();
        first.Document.RedirectUris.Should().Equal("https://agent.example.com/callback");
        second.Should().BeSameAs(first);
        _handler.Requests.Should().HaveCount(1);
        _handler.Requests[0].Headers.Accept.ToString().Should().Contain("application/json");
    }

    [Fact]
    public async Task Resolve_InvalidDocument_FailsAndIsNotCached()
    {
        // Arrange
        _handler.Respond(ClientId, Json(Document(clientId: "https://other.example.com/client.json")));
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);
        _handler.Respond(ClientId, Json(Document()));
        var fixedResult = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        result.IsTransient.Should().BeFalse();
        fixedResult.Document.Should().NotBeNull("an invalid document is fetched again, not cached");
        _handler.Requests.Should().HaveCount(2);
    }

    [Fact]
    public async Task Resolve_NotAClientIdUrl_NotFetched()
    {
        // Arrange
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync("http://agent.example.com/client.json", TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        _handler.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task Resolve_Disabled_NotFetched()
    {
        // Arrange
        _options.ClientRegistration.ClientIdMetadataDocuments.Enabled = false;
        var resolver = CreateResolver();

        // Act
        var isClientIdUrl = resolver.IsClientIdUrl(ClientId);
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        isClientIdUrl.Should().BeFalse();
        result.Document.Should().BeNull();
        _handler.Requests.Should().BeEmpty();
    }

    [Theory]
    [InlineData("claude.ai", false)]
    [InlineData("agent.example.com", true)]
    [InlineData("*.example.com", true)]
    [InlineData("*.agent.example.com", false)]
    public async Task Resolve_AllowedHosts_FetchesOnlyFromThem(string allowedHost, bool allowed)
    {
        // Arrange
        _options.ClientRegistration.ClientIdMetadataDocuments.AllowedHosts = [allowedHost];
        _handler.Respond(ClientId, Json(Document()));
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        (result.Document != null).Should().Be(allowed);
        _handler.Requests.Should().HaveCount(allowed ? 1 : 0);
    }

    [Fact]
    public async Task Resolve_LargerThanTheLimit_Refused()
    {
        // Arrange
        _options.ClientRegistration.ClientIdMetadataDocuments.MaxDocumentSize = 100;
        _handler.Respond(ClientId, Json(Document(extra: $"\"client_uri\": \"https://agent.example.com/{new string('a', 200)}\",")));
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        result.Error.Should().Contain("larger");
    }

    [Fact]
    public async Task Resolve_LargerThanTheLimitWithoutContentLength_Refused()
    {
        // Arrange
        _options.ClientRegistration.ClientIdMetadataDocuments.MaxDocumentSize = 100;
        _handler.Respond(ClientId, () => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StreamContent(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(new string(' ', 1000) + Document()))),
        });
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Error.Should().Contain("larger");
    }

    [Theory]
    [InlineData(HttpStatusCode.MovedPermanently)]
    [InlineData(HttpStatusCode.Found)]
    [InlineData(HttpStatusCode.TemporaryRedirect)]
    [InlineData(HttpStatusCode.PermanentRedirect)]
    public async Task Resolve_Redirect_NotFollowed(HttpStatusCode statusCode)
    {
        // Arrange
        _handler.Respond(ClientId, Redirect("https://cdn.example.com/client.json", statusCode));
        _handler.Respond("https://cdn.example.com/client.json", Json(Document()));
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        result.Error.Should().Contain("redirect");
        _handler.Requests.Should().ContainSingle();
    }

    [Fact]
    public async Task Resolve_SuccessOtherThan200_Refused()
    {
        // Arrange
        _handler.Respond(ClientId, () => new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation) { Content = new StringContent(Document()) });
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.ServiceUnavailable, true)]
    public async Task Resolve_ErrorStatus_Fails(HttpStatusCode statusCode, bool transient)
    {
        // Arrange
        _handler.Respond(ClientId, () => new HttpResponseMessage(statusCode));
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        result.IsTransient.Should().Be(transient);
    }

    [Fact]
    public async Task Resolve_Timeout_FailsAsTransient()
    {
        // Arrange
        _options.ClientRegistration.ClientIdMetadataDocuments.Timeout = TimeSpan.FromMilliseconds(50);
        _handler.Respond(ClientId, async cancellationToken =>
        {
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            return Json(Document())();
        });
        var resolver = CreateResolver();

        // Act
        var result = await resolver.ResolveAsync(ClientId, TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        result.IsTransient.Should().BeTrue();
    }

    [Theory]
    [InlineData("https://localhost/client.json")]
    [InlineData("https://127.0.0.1/client.json")]
    [InlineData("https://169.254.169.254/latest/meta-data")]
    [InlineData("https://[::1]/client.json")]
    [InlineData("https://10.0.0.1:8443/client.json")]
    public async Task Resolve_WithThePlatformHandler_PrivateAddressIsNotConnectedTo(string clientId)
    {
        // Arrange
        using var resolver = CreatePlatformResolver(Environments.Production);

        // Act
        var result = await resolver.ResolveAsync(clientId, TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        result.IsTransient.Should().BeFalse();
        result.Error.Should().Contain("public address");
    }

    [Theory]
    [InlineData("Development", true, true)]
    [InlineData("Development", false, false)]
    [InlineData("Production", true, false)]
    public async Task Resolve_WithThePlatformHandler_LoopbackOnlyWhenAllowedInDevelopment(string environment, bool allowLoopback, bool connects)
    {
        // Arrange
        _options.ClientRegistration.ClientIdMetadataDocuments.AllowLoopbackInDevelopment = allowLoopback;
        _options.ClientRegistration.ClientIdMetadataDocuments.Timeout = TimeSpan.FromSeconds(2);
        using var resolver = CreatePlatformResolver(environment);

        // Act
        // Nothing listens on the port: an allowed address fails to connect, a refused one is not tried
        var result = await resolver.ResolveAsync("https://127.0.0.1:1/client.json", TestContext.Current.CancellationToken);

        // Assert
        result.Document.Should().BeNull();
        result.Error.Contains("public address").Should().Be(!connects);
    }

    [Fact]
    public async Task Resolve_WithThePlatformHandlerAllowingLoopback_PrivateNetworkStillRefused()
    {
        // Arrange
        _options.ClientRegistration.ClientIdMetadataDocuments.AllowLoopbackInDevelopment = true;
        using var resolver = CreatePlatformResolver(Environments.Development);

        // Act
        var result = await resolver.ResolveAsync("https://10.0.0.1/client.json", TestContext.Current.CancellationToken);

        // Assert
        result.Error.Should().Contain("public address");
    }

    [Theory]
    [InlineData(null, 3600)]
    [InlineData("max-age=600", 600)]
    [InlineData("max-age=10", 300)]
    [InlineData("max-age=86400", 3600)]
    [InlineData("no-store", 300)]
    public void GetLifetime_FollowsCacheControlWithinTheLimits(string cacheControl, int expectedSeconds)
    {
        // Arrange
        var header = cacheControl == null ? null : CacheControlHeaderValue.Parse(cacheControl);

        // Act
        var lifetime = TestResolver.Lifetime(header, _options.ClientRegistration.ClientIdMetadataDocuments);

        // Assert
        lifetime.Should().Be(TimeSpan.FromSeconds(expectedSeconds));
    }

    private ClientIdMetadataDocumentResolver CreatePlatformResolver(string environment)
    {
        return new ClientIdMetadataDocumentResolver(
            Mock.Of<IPlatformMemoryCache>(x => x.CreateEntry(It.IsAny<object>()) == Mock.Of<ICacheEntry>()),
            Mock.Of<IOptionsMonitor<AuthorizationOptions>>(x => x.CurrentValue == _options),
            new ClientMetadataValidator(new OAuthAppRules()),
            Mock.Of<IHostEnvironment>(x => x.EnvironmentName == environment),
            NullLogger<ClientIdMetadataDocumentResolver>.Instance);
    }

    private TestResolver CreateResolver()
    {
        return new TestResolver(_handler, _cache, Mock.Of<IOptionsMonitor<AuthorizationOptions>>(x => x.CurrentValue == _options));
    }

    private static string Document(string clientId = ClientId, string extra = "")
    {
        return $$"""{ {{extra}} "client_id": "{{clientId}}", "token_endpoint_auth_method": "none", "redirect_uris": ["https://agent.example.com/callback"] }""";
    }

    private static Func<HttpResponseMessage> Json(string json)
    {
        return () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
    }

    private static Func<HttpResponseMessage> Redirect(string location, HttpStatusCode statusCode = HttpStatusCode.Found)
    {
        return () =>
        {
            var response = new HttpResponseMessage(statusCode);
            response.Headers.Location = new Uri(location);
            return response;
        };
    }

    private sealed class TestResolver(HttpMessageHandler handler, IMemoryCache cache, IOptionsMonitor<AuthorizationOptions> options)
        : ClientIdMetadataDocumentResolver(handler, cache, options, new ClientMetadataValidator(new OAuthAppRules()), NullLogger<ClientIdMetadataDocumentResolver>.Instance)
    {
        public static TimeSpan Lifetime(CacheControlHeaderValue cacheControl, OAuthClientIdMetadataDocumentOptions options) => GetLifetime(cacheControl, options);
    }

    private sealed class FakeHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, Func<CancellationToken, Task<HttpResponseMessage>>> _responses = new(StringComparer.Ordinal);

        public List<HttpRequestMessage> Requests { get; } = [];

        public void Respond(string uri, Func<HttpResponseMessage> response) => _responses[uri] = _ => Task.FromResult(response());

        public void Respond(string uri, Func<CancellationToken, Task<HttpResponseMessage>> response) => _responses[uri] = response;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);

            return _responses.TryGetValue(request.RequestUri!.AbsoluteUri, out var response)
                ? response(cancellationToken)
                : Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
