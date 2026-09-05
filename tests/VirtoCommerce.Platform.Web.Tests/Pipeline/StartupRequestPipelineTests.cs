using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Pipeline;

/// <summary>
/// Observes what a middleware registered through each IPlatformStartup hook can see, by driving the
/// extracted pipeline segment. Adjacency itself is not observable — a probe can show that a hook ran
/// before authentication, never that nothing ran in between — so these assert the properties the
/// positions exist for.
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
}
