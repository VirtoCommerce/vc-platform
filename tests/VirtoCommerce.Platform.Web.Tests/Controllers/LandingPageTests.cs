using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Web.Controllers;
using VirtoCommerce.Platform.Web.Extensions;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers;

/// <summary>
/// The platform ships no admin UI. "/" answers with a landing page until an admin UI module maps its own
/// page there; the endpoints below mirror the ones <see cref="Startup"/> maps.
/// </summary>
public class LandingPageTests
{
    [Fact]
    public async Task Root_NoAdminUIModule_ReturnsLandingPage()
    {
        using var host = await CreateHostAsync(withAdminUIModule: false);
        using var client = host.GetTestClient();

        using var response = await client.GetAsync("/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("No admin UI is installed");
    }

    [Fact]
    public async Task Root_AdminUIModuleMapsRoot_ReturnsModulePage()
    {
        using var host = await CreateHostAsync(withAdminUIModule: true);
        using var client = host.GetTestClient();

        using var response = await client.GetAsync("/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Be(FakeAdminUIController.Body);
    }

    [Fact]
    public async Task UnknownPath_NoAdminUIModule_Returns404()
    {
        using var host = await CreateHostAsync(withAdminUIModule: false);
        using var client = host.GetTestClient();

        using var response = await client.GetAsync("/no-such-page", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static Task<IHost> CreateHostAsync(bool withAdminUIModule)
    {
        return new HostBuilder()
            .ConfigureWebHost(webBuilder => webBuilder
                .UseTestServer()
                .ConfigureServices(services =>
                {
                    var mvcBuilder = services.AddControllersWithViews()
                        .ConfigureApplicationPartManager(manager => manager.ApplicationParts.Clear())
                        .AddApplicationPart(typeof(HomeController).Assembly);

                    if (withAdminUIModule)
                    {
                        // The way the platform registers a module's controllers
                        mvcBuilder.AddApplicationPart(typeof(FakeAdminUIController).Assembly);
                    }
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapPlatformLandingPage();
                    });
                }))
            .StartAsync();
    }
}

/// <summary>
/// Stands in for an admin UI module controller that maps the back office page at the site root.
/// </summary>
public class FakeAdminUIController : Controller
{
    public const string Body = "admin ui";

    [HttpGet("/")]
    public ActionResult Index()
    {
        return Content(Body, "text/plain");
    }
}
