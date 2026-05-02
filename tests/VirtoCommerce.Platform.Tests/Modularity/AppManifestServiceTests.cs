using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
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
    public void GetManifest_PlatformApp_HashChangesWhenFileMutates_AfterCacheInvalidation()
    {
        // The service caches the descriptor for the process lifetime, so a
        // file mutation alone won't change the returned hash — by design,
        // since modules + files don't change mid-process. The cache is
        // explicitly invalidated via AppManifestCacheRegion.ExpireRegion()
        // (e.g. by a future hot-reload feature, or by a test that needs a
        // fresh probe).
        var catalog = NewModule("VirtoCommerce.Catalog");
        var jsPath = WriteFile(catalog.FullPhysicalPath, "dist/app.js", "// v1");
        var service = NewService(catalog);

        var hash1 = service.GetManifest("platform").Plugins[0].Entry.Hash;

        // Mutate the file to a different last-write time AND invalidate
        // the cache region — both are required for the next probe to see
        // the new mtime.
        File.SetLastWriteTimeUtc(jsPath, DateTime.UtcNow.AddSeconds(10));
        AppManifestCacheRegion.ExpireRegion();

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

        var result = service.GetManifest("vc-shell-marketplace");

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
    public void GetManifest_ModernApp_PluginPermission_FlowsToDescriptorAsMetadata()
    {
        // The service does not filter by permission — it returns the full
        // plugin list with each plugin's `permission` field intact, and the
        // consuming SPA decides whether to load each plugin based on the
        // current user's claims. This keeps the controller cacheable
        // (one ETag per appId, not per user) and lets the service share a
        // single cached descriptor across all callers.
        var host = NewModule("VirtoCommerce.MarketplaceVendor");
        host.Apps.Add(new ManifestAppInfo { Id = "vc-shell-marketplace" });

        var plugin = NewModule("VirtoCommerce.MarketplaceReviews");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/remoteEntry.js", "// MF");
        WriteFile(plugin.FullPhysicalPath, "plugins/vc-shell-marketplace/plugin.json", """
        { "permission": "marketplace:reviews:advanced" }
        """);

        var service = NewService(host, plugin);

        var result = service.GetManifest("vc-shell-marketplace");

        var p = Assert.Single(result.Plugins);
        Assert.Equal("marketplace:reviews:advanced", p.Permission);
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

        Assert.Equal(s_expectedTopologicalOrder, result.Plugins.Select(p => p.Id).ToArray());
    }

    // CA1861 — keep the expected-order array out of the call site so xUnit's
    // assertion equality doesn't allocate a new array on every test invocation.
    private static readonly string[] s_expectedTopologicalOrder = ["Module.A", "Module.B", "Module.C"];

    // ---------- Cache + Hash behaviour ----------

    [Fact]
    public void GetManifest_RepeatedCalls_HitCacheNotFilesystem()
    {
        // The probe is identical across users for a given appId, and modules
        // + files don't change mid-process, so the descriptor is cached.
        // Verified by counting IModuleService.GetInstalledModules invocations.
        var catalog = NewModule("VirtoCommerce.Catalog");
        WriteFile(catalog.FullPhysicalPath, "dist/app.js", "// catalog");

        var harness = NewServiceWithMock(catalog);

        harness.Service.GetManifest("platform");
        harness.Service.GetManifest("platform");
        harness.Service.GetManifest("platform");

        harness.ModuleServiceMock.Verify(s => s.GetInstalledModules(), Times.Once);
    }

    [Fact]
    public void GetManifest_DescriptorHash_IsPopulatedAndDeterministic()
    {
        // Hash flows through the cache, so two calls return the same value
        // (same cached object, in fact). Strong ETag input for the controller.
        var catalog = NewModule("VirtoCommerce.Catalog");
        WriteFile(catalog.FullPhysicalPath, "dist/app.js", "// catalog");
        var service = NewService(catalog);

        var first = service.GetManifest("platform");
        var second = service.GetManifest("platform");

        Assert.False(string.IsNullOrEmpty(first.Hash));
        Assert.Equal(first.Hash, second.Hash);
    }

    [Fact]
    public void GetManifest_AfterRegionExpire_RebuildsFromFilesystem()
    {
        // AppManifestCacheRegion.ExpireRegion() is the documented escape
        // hatch for any future feature that needs to invalidate the cache
        // without a process restart. Calling it forces the next request
        // to re-walk the modules.
        var catalog = NewModule("VirtoCommerce.Catalog");
        WriteFile(catalog.FullPhysicalPath, "dist/app.js", "// catalog");

        var harness = NewServiceWithMock(catalog);

        harness.Service.GetManifest("platform");
        AppManifestCacheRegion.ExpireRegion();
        harness.Service.GetManifest("platform");

        harness.ModuleServiceMock.Verify(s => s.GetInstalledModules(), Times.Exactly(2));
    }

    [Fact]
    public void GetManifest_UnknownAppId_CachesNullResult()
    {
        // Repeated probes for a non-existent appId must not re-walk the
        // module list — null is a valid cached value.
        var harness = NewServiceWithMock(NewModule("VirtoCommerce.Foo"));

        Assert.Null(harness.Service.GetManifest("does-not-exist"));
        Assert.Null(harness.Service.GetManifest("does-not-exist"));

        harness.ModuleServiceMock.Verify(s => s.GetInstalledModules(), Times.Once);
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

    private AppManifestService NewService(params ManifestModuleInfo[] modules) =>
        NewServiceWithMock(modules).Service;

    private AppManifestServiceTestHarness NewServiceWithMock(params ManifestModuleInfo[] modules)
    {
        var moduleService = new Mock<IModuleService>();
        moduleService.Setup(s => s.GetInstalledModules()).Returns(modules.ToList());
        var service = new AppManifestService(
            moduleService.Object,
            NullLogger<AppManifestService>.Instance,
            new TestPlatformMemoryCache());

        // Each test instance gets its own cache (via TestPlatformMemoryCache),
        // but the static AppManifestCacheRegion is shared across instances. Ensure
        // any tokens raised by a previous test don't pre-expire this test's
        // entries — and clean up at the end via Dispose.
        AppManifestCacheRegion.ExpireRegion();

        return new AppManifestServiceTestHarness(service, moduleService);
    }

    private sealed record AppManifestServiceTestHarness(
        AppManifestService Service,
        Mock<IModuleService> ModuleServiceMock);

    /// <summary>
    /// Minimal <see cref="IPlatformMemoryCache"/> over an in-memory backing
    /// cache. Returns a default <see cref="MemoryCacheEntryOptions"/> so the
    /// tests don't depend on the platform's CachingOptions configuration —
    /// the only behaviour we exercise is "store, retrieve, expire on token".
    /// </summary>
    private sealed class TestPlatformMemoryCache : MemoryCache, IPlatformMemoryCache
    {
        public TestPlatformMemoryCache()
            : base(Options.Create(new MemoryCacheOptions()))
        {
        }

        public MemoryCacheEntryOptions GetDefaultCacheEntryOptions() => new();
    }
}
