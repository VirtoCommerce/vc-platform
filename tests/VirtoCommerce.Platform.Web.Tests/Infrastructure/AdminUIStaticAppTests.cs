using System;
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
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Extensions;
using VirtoCommerce.Platform.Web.Infrastructure.AdminUI;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Infrastructure;

/// <summary>
/// An <see cref="AdminUIHosting.App"/> admin UI at the site root, through the static-file middlewares the platform
/// registers for an app (the UseDefaultFiles + UseStaticFiles pair of UseModulesAndAppsFiles) rather than stand-in endpoints.
/// </summary>
public sealed class AdminUIStaticAppTests : IDisposable
{
    private const string AdminShellModule = """
        <module><id>VirtoCommerce.AdminShell</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
          <apps><app id="admin-shell"><title>Admin Shell</title><adminUI priority="50" /></app></apps>
        </module>
        """;

    private const string IndexHtml = "<html><script type=\"module\" src=\"/apps/admin-shell/index123.js\"></script></html>";

    private readonly string _appFolder;

    public AdminUIStaticAppTests()
    {
        _appFolder = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "admin-shell-app-" + Guid.NewGuid().ToString("N"))).FullName;
        File.WriteAllText(Path.Combine(_appFolder, "index.html"), IndexHtml);
        File.WriteAllText(Path.Combine(_appFolder, "index123.js"), "console.log('shell')");
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/?returnUrl=%2F")]
    [InlineData("/apps/admin-shell/")]
    public async Task Get_AppAdminUIAtTheRoot_ServesItsIndexPage(string url)
    {
        using var host = await CreateHostAsync();
        using var client = host.GetTestClient();

        using var response = await client.GetAsync(url, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
        (await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken)).Should().Be(IndexHtml);
    }

    [Fact]
    public async Task Head_AppAdminUIAtTheRoot_IsAnswered()
    {
        using var host = await CreateHostAsync();
        using var client = host.GetTestClient();

        using var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, "/"), TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_AppFilesByTheirAbsoluteUrls_AreServed()
    {
        using var host = await CreateHostAsync();
        using var client = host.GetTestClient();

        using var response = await client.GetAsync("/apps/admin-shell/index123.js", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_RootWithAdminUIOff_ServesThePlatformPageNotTheApp()
    {
        using var host = await CreateHostAsync(enable: false);

        var html = await host.GetTestClient().GetStringAsync("/", TestContext.Current.CancellationToken);

        html.Should().Contain("The admin UI is turned off");
    }

    public void Dispose()
    {
        Directory.Delete(_appFolder, recursive: true);
    }

    private Task<IHost> CreateHostAsync(bool enable = true)
    {
        var modules = new List<ManifestModuleInfo>
        {
            new ManifestModuleInfo().LoadFromManifest(ManifestReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(AdminShellModule)))),
        };
        var moduleService = new Mock<IModuleService>();
        moduleService.Setup(x => x.GetInstalledModules()).Returns(modules);
        var options = new Mock<IOptionsMonitor<PlatformUIOptions>>();
        options.Setup(x => x.CurrentValue).Returns(new PlatformUIOptions { Enable = enable });
        var service = new AdminUIAppService(moduleService.Object, options.Object, NullLogger<AdminUIAppService>.Instance);

        return new HostBuilder()
            .ConfigureWebHost(webBuilder => webBuilder
                .UseTestServer()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IAdminUIAppService>(service);
                    services.AddSingleton<MatcherPolicy, AdminUIMatcherPolicy>();
                    services.Configure<PlatformUIOptions>(x => x.Enable = enable);
                    services.AddRouting();
                })
                .Configure(app =>
                {
                    // The order of Startup.ConfigureRequestPipeline: admin UI routing, routing, then the apps' static files
                    app.UseMiddleware<AdminUIMiddleware>();
                    app.UseRouting();

                    var appPath = new PathString("/apps/admin-shell");
                    app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(_appFolder), RequestPath = appPath });
                    app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(_appFolder), RequestPath = appPath });

                    app.UseEndpoints(endpoints => endpoints.MapPlatformLandingPage());
                }))
            .StartAsync();
    }
}
