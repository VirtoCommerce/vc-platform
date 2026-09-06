using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.Platform.Core.Modularity;
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

    [Fact]
    public async Task Configure_WithAStartupOverridingNeitherNewMember_ServesNormally()
    {
        var withoutLegacy = await SendOpenRequestAsync(extraStartup: null);
        var withLegacy = await SendOpenRequestAsync(extraStartup: new LegacyPlatformStartup());

        // "Unchanged" is a comparison, not a status code: a default implementation that added a header
        // or altered the body would leave a status-only assertion green.
        withLegacy.Status.Should().Be(withoutLegacy.Status);
        withLegacy.Status.Should().Be(StatusCodes.Status200OK);
        withLegacy.Body.Should().Be(withoutLegacy.Body);
        withLegacy.Body.Should().Be(RequestPipelineHarness.OpenPathBody);
        withLegacy.Headers.Should().BeEquivalentTo(withoutLegacy.Headers);

        // The legacy one did not short-circuit the loop.
        withLegacy.AfterRoutingInvocations.Should().Be(1);
        withLegacy.AfterAuthenticationInvocations.Should().Be(1);
    }

    private static async Task<(int Status, string Body, IDictionary<string, string> Headers, int AfterRoutingInvocations, int AfterAuthenticationInvocations)>
        SendOpenRequestAsync(IPlatformStartup extraStartup)
    {
        await using var harness = await RequestPipelineHarness.StartAsync(extraStartup: extraStartup);

        var context = await harness.SendAsync(request =>
        {
            request.Request.Method = HttpMethods.Get;
            request.Request.Path = RequestPipelineHarness.OpenPath;
        });

        var body = await RequestPipelineHarness.ReadBodyAsync(context);

        // Date and Server vary per response and per host; everything else the segment can touch is
        // compared. Content-Length is kept deliberately - it is what a body change would move.
        var headers = context.Response.Headers
            .Where(x => !x.Key.Equals("Date", StringComparison.OrdinalIgnoreCase)
                     && !x.Key.Equals("Server", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Key, x => x.Value.ToString(), StringComparer.OrdinalIgnoreCase);

        return (context.Response.StatusCode, body, headers, harness.AfterRouting.Invocations, harness.AfterAuthentication.Invocations);
    }
}
