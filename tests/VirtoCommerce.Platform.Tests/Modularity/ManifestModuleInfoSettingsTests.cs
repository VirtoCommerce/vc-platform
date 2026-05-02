using System.Linq;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;
using static VirtoCommerce.Platform.Core.PlatformConstants.Settings;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// End-to-end coverage for the manifest-declared settings flow:
/// <c>ModuleManifest.Settings</c> XML POCO → <c>ManifestModuleInfo.Settings</c>
/// runtime <see cref="SettingDescriptor"/> list.
///
/// Exercises:
///   - happy path (settings populate correctly with the module id stamped on)
///   - error isolation (one bad setting doesn't take down the rest of the module)
/// </summary>
public class ManifestModuleInfoSettingsTests
{
    [Fact]
    public void LoadFromManifest_PopulatesSettingsWithModuleIdStamped()
    {
        var manifest = NewManifest("VirtoCommerce.Demo");
        manifest.Settings =
        [
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.MaxRetries",
                GroupName = "Demo|Reliability",
                DisplayName = "Max retries",
                ValueType = SettingValueType.PositiveInteger,
                DefaultValue = "3",
            },
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.EnableExperimentalUi",
                GroupName = "Demo|UI",
                ValueType = SettingValueType.Boolean,
                DefaultValue = "false",
                IsPublic = true,
            },
        ];

        var module = new ManifestModuleInfo().LoadFromManifest(manifest);

        Assert.Equal(2, module.Settings.Count);
        Assert.Empty(module.Errors);

        var maxRetries = module.Settings.Single(s => s.Name == "VirtoCommerce.Demo.MaxRetries");
        Assert.Equal("VirtoCommerce.Demo", maxRetries.ModuleId);
        Assert.Equal(SettingValueType.PositiveInteger, maxRetries.ValueType);
        Assert.Equal(3, maxRetries.DefaultValue);

        var experimental = module.Settings.Single(s => s.Name == "VirtoCommerce.Demo.EnableExperimentalUi");
        Assert.True(experimental.IsPublic);
        Assert.Equal(false, experimental.DefaultValue);
    }

    [Fact]
    public void LoadFromManifest_OneInvalidSetting_OthersStillRegister_ErrorSurfaced()
    {
        var manifest = NewManifest("VirtoCommerce.Demo");
        manifest.Settings =
        [
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.GoodSetting",
                ValueType = SettingValueType.Integer,
                DefaultValue = "42",
            },
            new ManifestSetting
            {
                // Will fail coercion: "abc" is not a valid integer.
                Name = "VirtoCommerce.Demo.BadSetting",
                ValueType = SettingValueType.Integer,
                DefaultValue = "abc",
            },
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.AnotherGoodSetting",
                ValueType = SettingValueType.Boolean,
                DefaultValue = "true",
            },
        ];

        var module = new ManifestModuleInfo().LoadFromManifest(manifest);

        // Bad setting skipped; others survive.
        Assert.Equal(2, module.Settings.Count);
        Assert.Contains(module.Settings, s => s.Name == "VirtoCommerce.Demo.GoodSetting");
        Assert.Contains(module.Settings, s => s.Name == "VirtoCommerce.Demo.AnotherGoodSetting");

        // Error surfaced against the module so the operator sees it in the
        // Modules admin UI alongside any other module-load errors.
        var error = Assert.Single(module.Errors);
        Assert.Contains("VirtoCommerce.Demo.BadSetting", error);
    }

    [Fact]
    public void LoadFromManifest_NoSettingsElement_LeavesCollectionEmpty()
    {
        // Manifests authored before this feature don't have <settings> at all.
        // The Settings collection must initialise to empty, not null.
        var manifest = NewManifest("VirtoCommerce.Legacy");

        var module = new ManifestModuleInfo().LoadFromManifest(manifest);

        Assert.NotNull(module.Settings);
        Assert.Empty(module.Settings);
        Assert.Empty(module.Errors);
    }

    [Fact]
    public void LoadFromManifest_AllSettingsLandInSingleCollection_TenantFlowsToDescriptor()
    {
        // Both global and tenant-scoped settings land in the single Settings
        // collection. UseSettingsFromModuleManifests then registers all
        // globally and additionally calls RegisterSettingsForType for any
        // descriptor whose Tenant is non-empty (e.g. "UserProfile").
        var manifest = NewManifest("VirtoCommerce.Demo");
        manifest.Settings =
        [
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.MaxRetries",
                ValueType = SettingValueType.Integer,
                DefaultValue = "3",
                // Tenant omitted -> global. Resulting descriptor.Tenant is null.
            },
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.UserDefaultTheme",
                ValueType = SettingValueType.ShortText,
                DefaultValue = "system",
                Tenant = nameof(UserProfile),
            },
        ];

        var module = new ManifestModuleInfo().LoadFromManifest(manifest);

        Assert.Equal(2, module.Settings.Count);

        var maxRetries = module.Settings.Single(s => s.Name == "VirtoCommerce.Demo.MaxRetries");
        Assert.Null(maxRetries.Tenant);

        var userTheme = module.Settings.Single(s => s.Name == "VirtoCommerce.Demo.UserDefaultTheme");
        Assert.Equal("UserProfile", userTheme.Tenant);
        Assert.Equal("VirtoCommerce.Demo", userTheme.ModuleId);
    }

    [Fact]
    public void LoadFromManifest_BadCoercion_OnTenantScopedSetting_SkipsOnly_ThatSetting()
    {
        // A tenant-scoped setting with an invalid integer must be skipped
        // and surfaced via Errors. Other settings (regardless of tenant)
        // still register normally.
        var manifest = NewManifest("VirtoCommerce.Demo");
        manifest.Settings =
        [
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.UserBadInt",
                ValueType = SettingValueType.Integer,
                DefaultValue = "not-a-number",
                Tenant = nameof(UserProfile),
            },
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.GlobalGood",
                ValueType = SettingValueType.Boolean,
                DefaultValue = "true",
            },
        ];

        var module = new ManifestModuleInfo().LoadFromManifest(manifest);

        // Bad setting dropped; the good global one survives.
        Assert.DoesNotContain(module.Settings, s => s.Name == "VirtoCommerce.Demo.UserBadInt");
        var good = Assert.Single(module.Settings);
        Assert.Equal("VirtoCommerce.Demo.GlobalGood", good.Name);
        var error = Assert.Single(module.Errors);
        Assert.Contains("VirtoCommerce.Demo.UserBadInt", error);
    }

    [Fact]
    public void LoadFromManifest_AllowedValues_FlowsThroughToDescriptor()
    {
        var manifest = NewManifest("VirtoCommerce.Demo");
        manifest.Settings =
        [
            new ManifestSetting
            {
                Name = "VirtoCommerce.Demo.Theme",
                ValueType = SettingValueType.ShortText,
                DefaultValue = "system",
                AllowedValues = ["system", "light", "dark"],
            },
        ];

        var module = new ManifestModuleInfo().LoadFromManifest(manifest);

        var theme = Assert.Single(module.Settings);
        Assert.NotNull(theme.AllowedValues);
        Assert.Equal(3, theme.AllowedValues.Length);
        Assert.Equal("system", theme.AllowedValues[0]);
    }

    private static ModuleManifest NewManifest(string id) => new()
    {
        Id = id,
        Version = "1.0.0",
        PlatformVersion = "3.0.0",
    };
}
