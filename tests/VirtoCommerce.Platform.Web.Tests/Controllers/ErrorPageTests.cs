using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Web.Controllers;
using VirtoCommerce.Platform.Web.Licensing;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers;

/// <summary>
/// Covers the production error page: the path handed to UseExceptionHandler must resolve to a real
/// endpoint, and that endpoint must be able to render. The pipeline below mirrors the parts of
/// <see cref="Startup"/> the error page depends on - the exception handler, the default controller
/// route, and a middleware that only throws for one path, the way the authentication middleware does.
/// </summary>
public class ErrorPageTests
{
    private const string ThrowingPath = "/signin-oidc";

    [Fact]
    public async Task ExceptionHandler_UnhandledException_RendersErrorPageWithStatus500()
    {
        using var host = await CreateHostAsync();
        using var client = host.GetTestClient();

        using var response = await client.PostAsync(ThrowingPath, new StringContent(string.Empty), TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("An error occurred while processing your request.");
    }

    [Fact]
    public async Task ExceptionHandlingPath_RequestedDirectly_RendersErrorPage()
    {
        using var host = await CreateHostAsync();
        using var client = host.GetTestClient();

        using var response = await client.GetAsync(Startup.ExceptionHandlingPath, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("An error occurred while processing your request.");
    }

    private static Task<IHost> CreateHostAsync()
    {
        return new HostBuilder()
            .ConfigureWebHost(webBuilder => webBuilder
                .UseTestServer()
                .ConfigureServices(ConfigureServices)
                .Configure(Configure))
            .StartAsync();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews()
            .AddApplicationPart(typeof(HomeController).Assembly);

        // HomeController is activated through DI, so every constructor dependency has to resolve even
        // though the Error action touches none of them.
        services.AddOptions();
        services.AddSingleton<Func<IPlatformRepository>>(() => throw new NotSupportedException());
        services.AddSingleton<LicenseProvider>();
        services.AddSingleton(new Mock<ISettingsManager>().Object);
    }

    private static void Configure(IApplicationBuilder app)
    {
        app.UseExceptionHandler(Startup.ExceptionHandlingPath);

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments(ThrowingPath))
            {
                throw new InvalidOperationException("Simulated unhandled exception.");
            }

            await next();
        });

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"));
    }
}
