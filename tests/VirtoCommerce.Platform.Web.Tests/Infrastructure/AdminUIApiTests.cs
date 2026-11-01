using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Infrastructure;
using VirtoCommerce.Platform.Web.Infrastructure.AdminUI;
using VirtoCommerce.Platform.Web.Licensing;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Infrastructure;

/// <summary>
/// What the platform tells clients about the admin UIs: the apps list, the UI context and the values behind it.
/// </summary>
public class AdminUIApiTests
{
    private const string AngularJsModule = """
        <module><id>VirtoCommerce.AdminAngularJs</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
          <apps><app id="platform"><title>Commerce Manager</title><contentPath>Content/wwwroot</contentPath>
            <adminUI priority="100" hosting="Endpoint" /></app></apps>
        </module>
        """;

    private const string AdminShellModule = """
        <module><id>VirtoCommerce.AdminShell</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
          <apps><app id="admin-shell"><title>Admin Shell</title><adminUI priority="50" /></app></apps>
        </module>
        """;

    private const string SystemOperationsModule = """
        <module><id>VirtoCommerce.SystemOperations</id><version>3.1000.0</version><platformVersion>3.1000.0</platformVersion>
          <apps><app id="system-operations"><title>System Operations</title></app></apps>
        </module>
        """;

    [Fact]
    public void GetApps_AdminShellAsRootApp_LinksEachAdminUIWhereItOpens()
    {
        var modules = Modules(SystemOperationsModule, AdminShellModule, AngularJsModule);
        var controller = new AppsController(ModuleService(modules), Service(modules, rootApp: "admin-shell"));

        var apps = controller.GetApps().ToList();

        apps.Select(x => (x.Id, x.RelativeUrl)).Should().Equal(
            ("platform", "/apps/platform"),
            ("admin-shell", "/"),
            ("system-operations", "/apps/system-operations"));
    }

    [Fact]
    public void GetApps_AngularJsAdminByDefault_IsFirstAndOpensAtTheSiteRoot()
    {
        var modules = Modules(AdminShellModule, AngularJsModule);
        var controller = new AppsController(ModuleService(modules), Service(modules));

        var apps = controller.GetApps().ToList();

        apps.Select(x => (x.Id, x.RelativeUrl)).Should().Equal(
            ("platform", "/"),
            ("admin-shell", "/apps/admin-shell"));
    }

    [Fact]
    public void GetApps_NoAngularJsAdmin_HasNoCommerceManagerEntry()
    {
        var modules = Modules(AdminShellModule);
        var controller = new AppsController(ModuleService(modules), Service(modules));

        controller.GetApps().Select(x => x.Id).Should().Equal("admin-shell");
    }

    [Fact]
    public async Task GetAdminUIContext_PlatformUIDisabled_ReturnsNotFound()
    {
        var controller = CommonController(new AdminUIContext(), enable: false);

        var result = await controller.GetAdminUIContext();

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAdminUIContext_Enabled_ReturnsTheContext()
    {
        var context = new AdminUIContext { PlatformVersion = "3.1093.0" };
        var controller = CommonController(context, enable: true);

        var result = await controller.GetAdminUIContext();

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeSameAs(context);
    }

    [Fact]
    public async Task GetContextAsync_NoLicense_ReportsOptionsAndAdminUIs()
    {
        var modules = Modules(AngularJsModule, AdminShellModule);
        var provider = ContextProvider(modules, new PlatformOptions { DemoCredentials = "{ userName: 'admin' }" }, refreshProbingFolder: false, forceWebSockets: true);

        var context = await provider.GetContextAsync(TestContext.Current.CancellationToken);

        context.Should().BeEquivalentTo(new
        {
            PlatformVersion = PlatformVersion.CurrentVersion?.ToString(),
            DemoCredentials = "{ userName: 'admin' }",
            DemoResetTime = (DateTime?)null,
            License = (object)null,
            SendDiagnosticData = true,
            AllowInstallModules = false,
            ForceWebSockets = true,
        });
        context.AdminUIs.Select(x => (x.Id, x.Url)).Should().Equal(("platform", "/"), ("admin-shell", "/apps/admin-shell/"));
    }

    [Theory]
    [InlineData("00:00:00")]
    [InlineData("23:59:59")]
    public async Task GetContextAsync_DemoResetTime_IsTheNextOccurrenceInUtc(string resetTime)
    {
        var provider = ContextProvider(Modules(), new PlatformOptions { DemoResetTime = resetTime });
        var before = DateTime.UtcNow;

        var context = await provider.GetContextAsync(TestContext.Current.CancellationToken);

        context.DemoResetTime.Should().NotBeNull();
        context.DemoResetTime!.Value.Kind.Should().Be(DateTimeKind.Utc);
        context.DemoResetTime.Value.TimeOfDay.Should().Be(TimeSpan.Parse(resetTime));
        context.DemoResetTime.Value.Should().BeOnOrAfter(before.AddSeconds(-1)).And.BeBefore(before.AddDays(1));
    }

    [Fact]
    public async Task GetContextAsync_DemoResetTimeNotATime_IsNull()
    {
        var provider = ContextProvider(Modules(), new PlatformOptions { DemoResetTime = "soon" });

        var context = await provider.GetContextAsync(TestContext.Current.CancellationToken);

        context.DemoResetTime.Should().BeNull();
    }

    private static List<ManifestModuleInfo> Modules(params string[] manifests)
    {
        return manifests
            .Select(xml => new ManifestModuleInfo().LoadFromManifest(ManifestReader.Read(new MemoryStream(Encoding.UTF8.GetBytes(xml)))))
            .ToList();
    }

    private static IModuleService ModuleService(List<ManifestModuleInfo> modules)
    {
        var moduleService = new Mock<IModuleService>();
        moduleService.Setup(x => x.GetInstalledModules()).Returns(modules);
        return moduleService.Object;
    }

    private static AdminUIAppService Service(List<ManifestModuleInfo> modules, string rootApp = null)
    {
        var options = new Mock<IOptionsMonitor<PlatformUIOptions>>();
        options.Setup(x => x.CurrentValue).Returns(new PlatformUIOptions { RootApp = rootApp });
        return new AdminUIAppService(ModuleService(modules), options.Object, NullLogger<AdminUIAppService>.Instance);
    }

    private static CommonController CommonController(AdminUIContext context, bool enable)
    {
        var provider = new Mock<IAdminUIContextProvider>();
        provider.Setup(x => x.GetContextAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(context);

        return new CommonController(
            Mock.Of<ICountriesService>(),
            Options.Create(new LoginPageUIOptions()),
            Options.Create(new PlatformUIOptions { Enable = enable }),
            provider.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
    }

    private static AdminUIContextProvider ContextProvider(List<ManifestModuleInfo> modules, PlatformOptions platformOptions, bool refreshProbingFolder = true, bool forceWebSockets = false)
    {
        var repository = new Mock<IPlatformRepository>();
        repository.Setup(x => x.RawLicenses).Returns(new List<RawLicenseEntity>().AsQueryable());

        return new AdminUIContextProvider(
            Options.Create(platformOptions),
            Options.Create(new LocalStorageModuleCatalogOptions { RefreshProbingFolderOnStart = refreshProbingFolder }),
            Options.Create(new PushNotificationOptions { ForceWebSockets = forceWebSockets }),
            Options.Create(new PlatformUIOptions()),
            new LicenseProvider(Options.Create(platformOptions), () => repository.Object),
            Mock.Of<ISettingsManager>(),
            Service(modules));
    }
}
