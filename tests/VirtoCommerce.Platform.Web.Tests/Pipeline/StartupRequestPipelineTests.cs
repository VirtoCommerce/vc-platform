using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Pipeline;

/// <summary>
/// Observes what a middleware registered through each IPlatformStartup hook can see, by driving the
/// extracted pipeline segment.
/// </summary>
[Collection(PlatformPipelineCollection.Name)]
public class StartupRequestPipelineTests
{
    [Fact]
    public async Task ConfigureAfterRouting_OnARoutedRequest_SeesTheMatchedEndpoint()
    {
        await using var harness = await RequestPipelineHarness.StartAsync();

        await harness.SendAsync(context =>
        {
            context.Request.Method = HttpMethods.Get;
            context.Request.Path = RequestPipelineHarness.OpenPath;
        });

        harness.AfterRouting.Invocations.Should().Be(1);
        harness.AfterRouting.EndpointMatched.Should().BeTrue();
    }

    [Fact]
    public async Task Configure_OnACredentialedRequest_TheTwoHooksStraddleAuthentication()
    {
        await using var harness = await RequestPipelineHarness.StartAsync();

        await harness.SendAsync(context =>
        {
            context.Request.Method = HttpMethods.Get;
            context.Request.Path = RequestPipelineHarness.OpenPath;
            context.Request.Headers[RequestPipelineHarness.CredentialHeader] = "harness-caller";
        });

        // Both hooks ran: without this, the first assertion below is satisfied by a hook that is not in
        // the pipeline at all, which is the very state it is supposed to distinguish from.
        harness.AfterRouting.Invocations.Should().Be(1);
        harness.AfterAuthentication.Invocations.Should().Be(1);

        // One observation of one request: asserting either half alone would pass against a hook on the
        // wrong side of UseAuthentication.
        harness.AfterRouting.AuthenticateResultFeaturePresent.Should().BeFalse();
        harness.AfterAuthentication.AuthenticateResultFeaturePresent.Should().BeTrue();
        harness.AfterAuthentication.AuthenticateSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task ConfigureAfterAuthentication_OnARequestAuthorizationRejects_IsStillInvokedAndTheResponseIs403()
    {
        await using var harness = await RequestPipelineHarness.StartAsync();

        // Authenticated, but without the claim the endpoint's policy requires.
        var context = await harness.SendAsync(request =>
        {
            request.Request.Method = HttpMethods.Get;
            request.Request.Path = RequestPipelineHarness.ProtectedPath;
            request.Request.Headers[RequestPipelineHarness.CredentialHeader] = "harness-caller";
        });

        // This is the criterion the hook position exists for: authorization short-circuits before the
        // endpoint, so a hook placed after UseAuthorization would never see a rejected request.
        harness.AfterAuthentication.Invocations.Should().Be(1);
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        context.Response.Headers.Location.ToString().Should().BeEmpty();
    }

    [Fact]
    public async Task ConfigureAfterRouting_OnAForwardedRequest_SeesTheResolvedClientAddress()
    {
        await using var harness = await RequestPipelineHarness.StartAsync();

        await harness.SendAsync(context =>
        {
            context.Request.Method = HttpMethods.Get;
            context.Request.Path = RequestPipelineHarness.OpenPath;
            context.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.1");
            context.Request.Headers["X-Forwarded-For"] = "203.0.113.7";
        });

        // With forwarding enabled the platform clears both trust lists, so there is no specially
        // trusted peer to send a request from.
        harness.AfterRouting.RemoteIpAddress.Should().Be(IPAddress.Parse("203.0.113.7"));
        harness.AfterRouting.RemoteIpAddress.Should().NotBe(IPAddress.Parse("10.0.0.1"));
    }
}
