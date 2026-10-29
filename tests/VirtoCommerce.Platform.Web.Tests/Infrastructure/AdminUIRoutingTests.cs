using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Controllers;
using VirtoCommerce.Platform.Web.Extensions;
using VirtoCommerce.Platform.Web.Infrastructure.AdminUI;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Infrastructure;

/// <summary>
/// Which admin UI answers "/" and "/apps/{appId}/". The modules are read from module.manifest XML, and the pipeline
/// mirrors the parts of <see cref="Startup"/> involved: <see cref="AdminUIMiddleware"/> ahead of routing,
/// <see cref="AdminUIMatcherPolicy"/>, the platform page at "/" and a module controller that maps "/".
/// </summary>
public class AdminUIRoutingTests
{
    // The AngularJS admin: the module maps its page at "/" (FakeAngularJsAdminController)
    private const string AngularJsModule = """
        <module><id>VirtoCommerce.AdminAngularJs</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
          <apps><app id="platform"><title>Commerce Manager</title><contentPath>Content/wwwroot</contentPath><placement>Hidden</placement>
            <adminUI priority="100" hosting="Endpoint" /></app></apps>
        </module>
        """;

    // The Vue admin: a static app (the test serves its "index page" with an endpoint at /apps/admin-shell/)
    private const string AdminShellModule = """
        <module><id>VirtoCommerce.AdminShell</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
          <apps><app id="admin-shell"><title>Admin Shell</title><adminUI priority="50" /></app></apps>
        </module>
        """;

    // An app that doesn't declare itself an admin UI
    private const string SystemOperationsModule = """
        <module><id>VirtoCommerce.SystemOperations</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
          <apps><app id="system-operations"><title>System Operations</title></app></apps>
        </module>
        """;

    [Fact]
    public async Task Root_NoAdminUI_ReturnsPlatformPage()
    {
        using var host = await CreateHostAsync([SystemOperationsModule]);

        var body = await GetAsync(host, "/");

        body.Should().Contain("No admin UI is installed");
    }

    [Fact]
    public async Task Root_AngularJsOnly_ReturnsAngularJsAdminAtSiteRoot()
    {
        using var host = await CreateHostAsync([AngularJsModule]);

        (await GetAsync(host, "/")).Should().Be("angularjs mount= base=");
    }

    [Fact]
    public async Task AppPath_AngularJs_ReturnsAngularJsAdminWithItsMountPathAndTheSitePathBase()
    {
        using var host = await CreateHostAsync([AngularJsModule]);

        (await GetAsync(host, "/apps/platform/")).Should().Be("angularjs mount=/apps/platform base=");
    }

    [Fact]
    public async Task AppPath_AngularJs_RelativeUrlsReachThePlatform()
    {
        using var host = await CreateHostAsync([AngularJsModule]);

        (await GetAsync(host, "/apps/platform/api/ping")).Should().Be("pong path=/api/ping base=");
    }

    [Fact]
    public async Task AppPath_AngularJsWithoutTrailingSlash_RedirectsToTheSlash()
    {
        using var host = await CreateHostAsync([AngularJsModule]);
        using var client = host.GetTestClient();

        using var response = await client.GetAsync("/apps/platform?EmbeddedMode=true", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.OriginalString.Should().Be("platform/?EmbeddedMode=true");
    }

    [Fact]
    public async Task Root_AdminShellOnly_ReturnsAdminShellAtSiteRoot()
    {
        using var host = await CreateHostAsync([AdminShellModule]);

        (await GetAsync(host, "/")).Should().Be("admin-shell index");
    }

    [Fact]
    public async Task Root_BothByDefault_AngularJsOwnsTheRootAndAdminShellIsAnApp()
    {
        using var host = await CreateHostAsync([AngularJsModule, AdminShellModule]);

        (await GetAsync(host, "/")).Should().Be("angularjs mount= base=");
        (await GetAsync(host, "/apps/admin-shell/")).Should().Be("admin-shell index");
    }

    [Fact]
    public async Task Root_BothWithAdminShellAsRootApp_AdminShellOwnsTheRootAndAngularJsIsUnderItsAppPath()
    {
        using var host = await CreateHostAsync([AngularJsModule, AdminShellModule], rootApp: "admin-shell");

        (await GetAsync(host, "/")).Should().Be("admin-shell index");
        (await GetAsync(host, "/apps/platform/")).Should().Be("angularjs mount=/apps/platform base=");
    }

    [Fact]
    public async Task Root_RootAppWithoutAdminUIDeclaration_ServesThatApp()
    {
        using var host = await CreateHostAsync([AngularJsModule, SystemOperationsModule], rootApp: "system-operations");

        (await GetAsync(host, "/")).Should().Be("system-operations index");
    }

    [Fact]
    public async Task Root_RootAppNone_ReturnsPlatformPageEvenWithAdminUIs()
    {
        using var host = await CreateHostAsync([AngularJsModule, AdminShellModule], rootApp: "none");

        (await GetAsync(host, "/")).Should().Contain("No admin UI is served at the site root");
        (await GetAsync(host, "/apps/platform/")).Should().Be("angularjs mount=/apps/platform base=");
    }

    [Fact]
    public async Task Root_UnknownRootApp_FallsBackToHighestPriority()
    {
        using var host = await CreateHostAsync([AdminShellModule, AngularJsModule], rootApp: "no-such-app");

        (await GetAsync(host, "/")).Should().Be("angularjs mount= base=");
    }

    [Fact]
    public async Task UnknownPath_Returns404()
    {
        using var host = await CreateHostAsync([AngularJsModule]);
        using var client = host.GetTestClient();

        using var response = await client.GetAsync("/no-such-page", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public void GetAdminUIs_AdminShellAsRootApp_ReportsWhereEachUIOpens()
    {
        var service = CreateService([AngularJsModule, AdminShellModule], rootApp: "admin-shell");

        var adminUIs = service.GetAdminUIs().ToDictionary(x => x.Id);

        adminUIs["admin-shell"].Should().BeEquivalentTo(new { Url = "/", Path = "/apps/admin-shell", IsRoot = true, Hosting = AdminUIHosting.App, Priority = 50, ModuleId = "VirtoCommerce.AdminShell" });
        adminUIs["platform"].Should().BeEquivalentTo(new { Url = "/apps/platform/", Path = "/apps/platform", IsRoot = false, Hosting = AdminUIHosting.Endpoint, Priority = 100, Title = "Commerce Manager" });
    }

    [Fact]
    public async Task Root_PlatformUIDisabled_ReturnsPlatformPageSayingTheUIIsOff()
    {
        using var host = await CreateHostAsync([AngularJsModule, AdminShellModule], enable: false);

        (await GetAsync(host, "/")).Should().Contain("The admin UI is turned off");
    }

    [Fact]
    public async Task Root_RootAppNoneWithAdminUIsInstalled_SaysNoAdminUIIsServedAtTheRoot()
    {
        using var host = await CreateHostAsync([AngularJsModule], rootApp: "none");

        (await GetAsync(host, "/")).Should().Contain("No admin UI is served at the site root");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Root_Head_IsAnsweredForLoadBalancerProbes(bool angularJsInstalled)
    {
        using var host = await CreateHostAsync([angularJsInstalled ? AngularJsModule : SystemOperationsModule]);
        using var client = host.GetTestClient();

        using var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, "/"), TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Root_AnotherModuleEndpointAtRootWithoutAdminUI_WinsOverThePlatformPage()
    {
        using var host = await CreateHostAsync([SystemOperationsModule], otherRootEndpoint: true);

        (await GetAsync(host, "/")).Should().Be("other root page");
    }

    [Theory]
    [InlineData("endpoint", AdminUIHosting.Endpoint)]
    [InlineData("Endpoint", AdminUIHosting.Endpoint)]
    [InlineData("App", AdminUIHosting.App)]
    [InlineData("Remote", AdminUIHosting.App)]
    public void ManifestReader_AdminUIHosting_IsReadLenientlySoTheManifestStillLoads(string hosting, AdminUIHosting expected)
    {
        var xml = $"""
            <module><id>X</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
              <apps><app id="x"><adminUI priority="7" hosting="{hosting}" /></app></apps>
            </module>
            """;

        var manifest = ManifestReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(xml)));

        manifest.Apps[0].AdminUI.Should().BeEquivalentTo(new { Priority = 7, Hosting = expected });
    }

    private static async Task<string> GetAsync(IHost host, string url)
    {
        using var client = host.GetTestClient();
        using var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK, url);
        return await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
    }

    private static AdminUIAppService CreateService(IEnumerable<string> manifests, string rootApp = null, bool enable = true)
    {
        var modules = manifests
            .Select(xml => new ManifestModuleInfo().LoadFromManifest(ManifestReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(xml)))))
            .ToList();

        var moduleService = new Mock<IModuleService>();
        moduleService.Setup(x => x.GetInstalledModules()).Returns(modules);

        var options = new Mock<IOptionsMonitor<PlatformUIOptions>>();
        options.Setup(x => x.CurrentValue).Returns(new PlatformUIOptions { RootApp = rootApp, Enable = enable });

        return new AdminUIAppService(moduleService.Object, options.Object, NullLogger<AdminUIAppService>.Instance);
    }

    private static Task<IHost> CreateHostAsync(IEnumerable<string> manifests, string rootApp = null, bool enable = true, bool otherRootEndpoint = false)
    {
        var service = CreateService(manifests, rootApp, enable);

        return new HostBuilder()
            .ConfigureWebHost(webBuilder => webBuilder
                .UseTestServer()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IAdminUIAppService>(service);
                    services.Configure<PlatformUIOptions>(options => options.Enable = enable);
                    services.AddSingleton<MatcherPolicy, AdminUIMatcherPolicy>();
                    services.AddControllersWithViews()
                        .ConfigureApplicationPartManager(manager => manager.ApplicationParts.Clear())
                        .AddApplicationPart(typeof(HomeController).Assembly)
                        .AddApplicationPart(typeof(FakeAngularJsAdminController).Assembly);
                })
                .Configure(app =>
                {
                    app.UseMiddleware<AdminUIMiddleware>();
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapPlatformLandingPage();
                        // Stand-ins for the static index pages the platform serves for apps
                        endpoints.MapGet("/apps/admin-shell/", () => "admin-shell index");
                        endpoints.MapGet("/apps/system-operations/", () => "system-operations index");
                        if (otherRootEndpoint)
                        {
                            // A module's own page at "/" that isn't an admin UI page
                            endpoints.MapGet("/", () => "other root page");
                        }
                        endpoints.MapGet("/api/ping", (HttpContext context) => $"pong path={context.Request.Path} base={context.Request.PathBase}");
                    });
                }))
            .StartAsync();
    }
}

/// <summary>
/// Stands in for the AngularJS admin module's controller.
/// </summary>
public class FakeAngularJsAdminController : Controller
{
    [AcceptVerbs("GET", "HEAD", Route = "/")]
    [AdminUIPage("platform")]
    public ActionResult Index()
    {
        var mount = HttpContext.Features.Get<AdminUIRequestFeature>()?.MountPath;
        return Content($"angularjs mount={mount} base={Request.PathBase}", "text/plain");
    }
}
