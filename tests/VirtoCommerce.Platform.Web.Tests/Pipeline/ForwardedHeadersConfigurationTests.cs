using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Web.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Pipeline;

/// <summary>
/// The half of the client-address property that has nothing to do with where a hook sits: whether the
/// resolved address is the nearest proxy's attestation or the caller's own claim. ForwardLimit truncates
/// before the set is built and the set is filled from the end, so the last entry wins - as long as some
/// layer appends one. The configuration under test is the platform's own AddForwardedHeaders, so the
/// test reddens if it stops clearing the trust lists or starts setting ForwardLimit. This is the
/// criterion to point a reviewer at when they ask why the trust lists being cleared is not the end of
/// the analysis.
/// </summary>
[Collection(PlatformPipelineCollection.Name)]
public class ForwardedHeadersConfigurationTests
{
    [Fact]
    public async Task ForwardedHeaders_WithThePlatformsConfiguration_ResolvesTheLastEntryNotTheCallerSupplied()
    {
        using var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder => webBuilder
                .UseTestServer()
                .ConfigureServices(services => services.AddForwardedHeaders())
                .Configure(app =>
                {
                    app.UseForwardedHeaders();
                    app.Run(context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Task.CompletedTask;
                    });
                }))
            .StartAsync(TestContext.Current.CancellationToken);

        var context = await host.GetTestServer().SendAsync(request =>
        {
            request.Request.Method = HttpMethods.Get;
            request.Request.Path = "/";
            request.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.1");

            // First entry is the caller's own claim, second was appended by the layer in front.
            request.Request.Headers["X-Forwarded-For"] = "198.51.100.9, 203.0.113.7";
        }, TestContext.Current.CancellationToken);

        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        context.Connection.RemoteIpAddress.Should().Be(IPAddress.Parse("203.0.113.7"));
        context.Connection.RemoteIpAddress.Should().NotBe(IPAddress.Parse("198.51.100.9"));

        await host.StopAsync(TestContext.Current.CancellationToken);
    }
}
