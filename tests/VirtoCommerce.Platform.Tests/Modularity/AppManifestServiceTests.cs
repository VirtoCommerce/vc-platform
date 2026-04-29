using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

// AppManifestServiceTests mutates the static PlatformVersion.CurrentVersion as
// part of its fixture; xUnit.v3 runs tests in a single class sequentially by
// default, so we don't need [Collection] to serialize them, but we must
// snapshot/restore around each fixture instance to avoid leaking state into
// other test classes (e.g. AppsControllerTests).

namespace VirtoCommerce.Platform.Tests.Modularity;

public class AppManifestServiceTests : IDisposable
{
    private readonly string _root;
    private readonly SemanticVersion _originalPlatformVersion;

    public AppManifestServiceTests()
    {
        _root = Path.Combine(Path.GetTempPath(), nameof(AppManifestServiceTests) + Path.GetRandomFileName());
        Directory.CreateDirectory(_root);

        // Snapshot the static PlatformVersion so we can deterministically assert
        // it surfaces on the descriptor for the "platform" appId. Restored in Dispose.
        _originalPlatformVersion = PlatformVersion.CurrentVersion;
        PlatformVersion.CurrentVersion = SemanticVersion.Parse("3.999.0");
    }

    public void Dispose()
    {
        PlatformVersion.CurrentVersion = _originalPlatformVersion;
        try { Directory.Delete(_root, true); } catch { /* best-effort cleanup */ }
        GC.SuppressFinalize(this);
    }

    // ---------- Legacy AngularJS path (appId == "platform") ----------

    [Fact]
    public void GetManifest_PlatformApp_FindsModulesWithDistAppJs()
    {
        // Arrange — two installed modules in dependency order: catalog, then pricing.
        var catalog = NewModule("VirtoCommerce.Catalog", version: "3.1011.0");
        WriteFile(catalog.FullPhysicalPath, "dist/app.js", "// catalog");
        WriteFile(catalog.FullPhysicalPath, "dist/style.css", "/* catalog */");

        var pricing = NewModule("VirtoCommerce.Pricing", version: "3.1000.0");
        WriteFile(pricing.FullPhysicalPath, "dist/app.js", "// pricing");
        // No style.css for pricing — exercises the "JS only" branch.

        var service = NewService(catalog, pricing);

        // Act
        var result = service.GetManifest("platform");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("platform", result.AppId);
        Assert.Equal(2, result.Plugins.Count);

        // First plugin: catalog with both script and style files.
        var catalogPlugin = result.Plugins[0];
        Assert.Equal("VirtoCommerce.Catalog", catalogPlugin.Id);
        Assert.Equal("3.1011.0", catalogPlugin.Version);
        Assert.NotNull(catalogPlugin.Entry);
        Assert.Equal(ContentFileTypes.Script, catalogPlugin.Entry.Type);
        Assert.Equal("/modules/$(VirtoCommerce.Catalog)/dist/app.js", catalogPlugin.Entry.Path);
        Assert.False(string.IsNullOrEmpty(catalogPlugin.Entry.Hash), "Entry hash must be populated for cache-busting.");

        var catalogStyle = Assert.Single(catalogPlugin.ContentFiles);
        Assert.Equal(ContentFileTypes.Style, catalogStyle.Type);
        Assert.Equal("/modules/$(VirtoCommerce.Catalog)/dist/style.css", catalogStyle.Path);
        Assert.False(string.IsNullOrEmpty(catalogStyle.Hash));
        Assert.Null(catalogPlugin.Remote);

        // Second plugin: pricing with no style.css — ContentFiles must be empty.
        Assert.Equal("VirtoCommerce.Pricing", result.Plugins[1].Id);
        Assert.Equal("/modules/$(VirtoCommerce.Pricing)/dist/app.js", result.Plugins[1].Entry.Path);
        Assert.Empty(result.Plugins[1].ContentFiles);
    }

    [Fact]
    public void GetManifest_PlatformApp_SkipsModulesWithoutDistAppJs()
    {
        var pure = NewModule("VirtoCommerce.PureBackend", version: "3.0.0");
        // No dist/app.js — should be skipped silently.
        var service = NewService(pure);

        var result = service.GetManifest("platform");

        Assert.NotNull(result);
        Assert.Empty(result.Plugins);
    }

    [Fact]
    public void GetManifest_PlatformApp_HashChangesWhenFileMutates()
    {
        var catalog = NewModule("VirtoCommerce.Catalog");
        var jsPath = WriteFile(catalog.FullPhysicalPath, "dist/app.js", "// v1");
        var service = NewService(catalog);

        var hash1 = service.GetManifest("platform").Plugins[0].Entry.Hash;

        // Mutate the file to a different last-write time.
        File.SetLastWriteTimeUtc(jsPath, DateTime.UtcNow.AddSeconds(10));
        var hash2 = service.GetManifest("platform").Plugins[0].Entry.Hash;

        Assert.NotEqual(hash1, hash2);
    }

    // ---------- Modern MF path (appId != "platform") ----------

    [Fact]
    public void GetManifest_UnknownAppId_ReturnsNull()
    {
        var service = NewService(NewModule("VirtoCommerce.Foo"));

        var result = service.GetManifest("does-not-exist");

        Assert.Null(result);
    }

    [Fact]
    public void GetManifest_ModernApp_NoManifest_SynthesizesFromConvention()
    {
        // Host module declares <app id="vc-shell-marketplace">.
        var host = NewModule("VirtoCommerce.MarketplaceVendor", version: "3.1000.0");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace", Title = "Marketplace" });

        // Plugin module just drops a remoteEntry.js — no plugin.json.
        var reviews = NewModule("VirtoCommerce.MarketplaceReviews", version: "3.1001.0");
        WriteFile(reviews.FullPhysicalPath, "plugins/vc-shell-marketplace/remoteEntry.js", "// MF");

        var service = NewService(host, reviews);

        var result = service.GetManifest("vc-shell-marketplace");

        Assert.NotNull(result);
        Assert.Equal("Marketplace", result.Title);
        var plugin = Assert.Single(result.Plugins);
        Assert.Equal("VirtoCommerce.MarketplaceReviews", plugin.Id);
        Assert.Equal("3.1001.0", plugin.Version);
        Assert.Equal(ContentFileTypes.Script, plugin.Entry.Type);
        Assert.Equal("/modules/$(VirtoCommerce.MarketplaceReviews)/plugins/vc-shell-marketplace/remoteEntry.js", plugin.Entry.Path);
        Assert.False(string.IsNullOrEmpty(plugin.Entry.Hash));
        Assert.Empty(plugin.ContentFiles);
        Assert.NotNull(plugin.Remote);
        Assert.Equal("VirtoCommerce.MarketplaceReviews", plugin.Remote.Name);
        Assert.Equal("./Module", plugin.Remote.Exposed);
    }

    [Fact]
    public void GetManifest_ModernApp_WithManifest_OverridesDefaultsAndInfersTypes()
    {
        var host = NewModule("VirtoCommerce.MarketplaceVendor");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace" });

        var plugin = NewModule("VirtoCommerce.MarketplaceReviews", version: "3.1001.0");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/module.mjs", "// MF");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/theme.css", "/* css */");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/extras.js", "// helpers");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/plugin.json", """
        {
          "id": "reviews-custom",
          "version": "1.2.0",
          "entry": "module.mjs",
          "contentFiles": ["theme.css", "extras.js"],
          "permission": "marketplace:reviews:advanced",
          "remote": { "name": "ReviewsRemote", "exposed": "./Reviews" }
        }
        """);

        var service = NewService(host, plugin);

        // Caller has the required permission.
        var result = service.GetManifest("vc-shell-marketplace", PrincipalWith("marketplace:reviews:advanced"));

        var p = Assert.Single(result.Plugins);
        Assert.Equal("reviews-custom", p.Id);
        Assert.Equal("1.2.0", p.Version);
        Assert.Equal(ContentFileTypes.Script, p.Entry.Type);
        Assert.Equal("/modules/$(VirtoCommerce.MarketplaceReviews)/plugins/vc-shell-marketplace/module.mjs", p.Entry.Path);
        Assert.Equal("ReviewsRemote", p.Remote.Name);
        Assert.Equal("./Reviews", p.Remote.Exposed);

        // Content files: extension-based type inference (.css → style, .js → script).
        Assert.Equal(2, p.ContentFiles.Count);
        var css = p.ContentFiles.Single(f => f.Path.EndsWith(".css"));
        Assert.Equal(ContentFileTypes.Style, css.Type);
        var js = p.ContentFiles.Single(f => f.Path.EndsWith(".js"));
        Assert.Equal(ContentFileTypes.Script, js.Type);
    }

    [Fact]
    public void GetManifest_ModernApp_PluginPermission_FiltersOutForUnauthorizedUsers()
    {
        var host = NewModule("VirtoCommerce.MarketplaceVendor");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace" });

        var plugin = NewModule("VirtoCommerce.MarketplaceReviews");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/remoteEntry.js", "// MF");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/plugin.json", """
        { "permission": "marketplace:reviews:advanced" }
        """);

        var service = NewService(host, plugin);

        var withoutPerm = service.GetManifest("vc-shell-marketplace", PrincipalWith()); // no permissions
        var withPerm = service.GetManifest("vc-shell-marketplace", PrincipalWith("marketplace:reviews:advanced"));

        Assert.Empty(withoutPerm.Plugins);
        Assert.Single(withPerm.Plugins);
    }

    [Fact]
    public void GetManifest_ModernApp_MalformedPluginJson_FallsBackToConvention()
    {
        var host = NewModule("VirtoCommerce.MarketplaceVendor");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace" });

        var plugin = NewModule("VirtoCommerce.MarketplaceReviews");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/remoteEntry.js", "// MF");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/plugin.json", "{ this is { not json");

        var service = NewService(host, plugin);

        var result = service.GetManifest("vc-shell-marketplace");

        // Falls back to defaults; plugin still loads.
        var p = Assert.Single(result.Plugins);
        Assert.Equal("VirtoCommerce.MarketplaceReviews", p.Id);
        Assert.EndsWith("plugins/vc-shell-marketplace/remoteEntry.js", p.Entry.Path);
    }

    [Fact]
    public void GetManifest_ModernApp_ManifestEntryMissingFile_SkipsPlugin()
    {
        var host = NewModule("VirtoCommerce.MarketplaceVendor");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace" });

        var plugin = NewModule("VirtoCommerce.MarketplaceReviews");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/plugin.json", """
        { "entry": "ghost.js" }
        """);
        // ghost.js is not written — plugin should be skipped.

        var service = NewService(host, plugin);

        var result = service.GetManifest("vc-shell-marketplace");

        Assert.Empty(result.Plugins);
    }

    [Fact]
    public void GetManifest_ModernApp_RespectsCustomDiscoveryFolder()
    {
        var host = NewModule("VirtoCommerce.MarketplaceVendor");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace", PluginsDiscoveryFolder = "extensions" });

        var plugin = NewModule("VirtoCommerce.MarketplaceReviews");
        // Plugin must live under "extensions/" instead of the default "plugins/".
        WriteFile(plugin.FullPhysicalPath, "extensions/vc-shell-marketplace/remoteEntry.js", "// MF");

        var service = NewService(host, plugin);

        var result = service.GetManifest("vc-shell-marketplace");

        var p = Assert.Single(result.Plugins);
        Assert.Equal("/modules/$(VirtoCommerce.MarketplaceReviews)/extensions/vc-shell-marketplace/remoteEntry.js", p.Entry.Path);
    }

    [Fact]
    public void GetManifest_ModernApp_PreservesTopologicalOrder()
    {
        // GetInstalledModules() returns in dep order: A, B, C — service must preserve.
        var host = NewModule("VirtoCommerce.Host");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace" });

        var a = NewModule("Module.A");
        var b = NewModule("Module.B");
        var c = NewModule("Module.C");
        foreach (var m in new[] { a, b, c })
        {
            WriteFile(m.FullPhysicalPath, "plugins/vc-shell-marketplace/remoteEntry.js", "// MF");
        }

        var service = NewService(host, a, b, c);

        var result = service.GetManifest("vc-shell-marketplace");

        Assert.Equal(new[] { "Module.A", "Module.B", "Module.C" }, result.Plugins.Select(p => p.Id).ToArray());
    }

    // ---------- Helpers ----------

    private ManifestModuleInfo NewModule(string id, string version = "1.0.0")
    {
        var path = Path.Combine(_root, id);
        Directory.CreateDirectory(path);
        var module = new ManifestModuleInfo
        {
            ModuleName = id,
            FullPhysicalPath = path,
            IsInstalled = true,
        };
        // Use reflection-equivalent — these properties have private setters via XmlSerializer / LoadFromManifest;
        // for tests we drive them through LoadFromManifest with a minimal ModuleManifest stub.
        module.LoadFromManifest(new ModuleManifest
        {
            Id = id,
            Version = version,
            PlatformVersion = "3.0.0",
        });
        return module;
    }

    private static string WriteFile(string moduleRoot, string relativePath, string content)
    {
        var fullPath = Path.Combine(moduleRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        File.WriteAllText(fullPath, content);
        return fullPath;
    }

    private static AppManifestService NewService(params ManifestModuleInfo[] modules)
    {
        var moduleService = new Mock<IModuleService>();
        moduleService.Setup(s => s.GetInstalledModules()).Returns(modules.ToList());
        return new AppManifestService(moduleService.Object, NullLogger<AppManifestService>.Instance);
    }

    private static ClaimsPrincipal PrincipalWith(params string[] permissions)
    {
        var claims = new List<Claim> { new(ClaimTypes.Name, "tester") };
        claims.AddRange(permissions.Select(p =>
            new Claim(PlatformConstants.Security.Claims.PermissionClaimType, p)));
        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }
}
