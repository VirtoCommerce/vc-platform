using VirtoCommerce.Platform.Core.Modularity;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// Locks in the backwards-compatibility rules for the
/// <see cref="AppPlacement"/> property on <see cref="ManifestAppInfo"/>.
///
/// Existing module manifests carry a boolean
/// <c>&lt;supportEmbeddedMode&gt;</c>; new manifests carry an enum
/// <c>&lt;placement&gt;</c>. The runtime model has to honour the new
/// element when present and fall back to the old boolean otherwise so
/// hundreds of unmodified third-party modules keep behaving identically.
/// </summary>
public class ManifestAppInfoTests
{
    [Fact]
    public void Placement_DerivedFromSupportEmbeddedMode_True_MainMenu()
    {
        var manifest = new ManifestApp
        {
            Id = "test-app",
            SupportEmbeddedMode = true,
            // Placement intentionally not set — exercising the legacy
            // path: <supportEmbeddedMode>true</supportEmbeddedMode> alone.
        };

        var info = new ManifestAppInfo(manifest);

        Assert.Equal(AppPlacement.MainMenu, info.Placement);
    }

    [Fact]
    public void Placement_DerivedFromSupportEmbeddedMode_False_AppMenu()
    {
        var manifest = new ManifestApp
        {
            Id = "test-app",
            SupportEmbeddedMode = false,
        };

        var info = new ManifestAppInfo(manifest);

        Assert.Equal(AppPlacement.AppMenu, info.Placement);
    }

    [Fact]
    public void Placement_DefaultsToAppMenu_WhenNothingDeclared()
    {
        var manifest = new ManifestApp
        {
            Id = "test-app",
            // Neither Placement nor SupportEmbeddedMode set — equivalent to
            // a manifest that omits both elements. Default bool is false,
            // so derivation lands on AppMenu.
        };

        var info = new ManifestAppInfo(manifest);

        Assert.Equal(AppPlacement.AppMenu, info.Placement);
    }

    [Fact]
    public void Placement_ExplicitValue_WinsOverLegacyBoolean()
    {
        // Manifest with both elements: the new <placement> takes precedence
        // even when the legacy <supportEmbeddedMode> would have suggested
        // a different placement.
        var manifest = new ManifestApp
        {
            Id = "test-app",
            Placement = AppPlacement.Hidden,
            SupportEmbeddedMode = true,
        };

        var info = new ManifestAppInfo(manifest);

        Assert.Equal(AppPlacement.Hidden, info.Placement);
    }

    [Theory]
    [InlineData(AppPlacement.AppMenu)]
    [InlineData(AppPlacement.MainMenu)]
    [InlineData(AppPlacement.Hidden)]
    public void Placement_ExplicitValue_PassesThroughUnchanged(AppPlacement explicitPlacement)
    {
        var manifest = new ManifestApp
        {
            Id = "test-app",
            Placement = explicitPlacement,
        };

        var info = new ManifestAppInfo(manifest);

        Assert.Equal(explicitPlacement, info.Placement);
    }
}
