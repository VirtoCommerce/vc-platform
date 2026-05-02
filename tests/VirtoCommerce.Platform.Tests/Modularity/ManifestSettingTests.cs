using System;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;
using static VirtoCommerce.Platform.Core.PlatformConstants.Settings;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// Locks in the coercion rules for <see cref="ManifestSetting.ToSettingDescriptor"/>.
/// XML carries everything as strings; the platform must convert
/// <c>&lt;defaultValue&gt;</c> and <c>&lt;allowedValues&gt;/&lt;value&gt;</c>
/// into the runtime CLR type implied by <c>&lt;valueType&gt;</c>.
/// </summary>
public class ManifestSettingTests
{
    [Theory]
    [InlineData(SettingValueType.ShortText, "hello", "hello")]
    [InlineData(SettingValueType.LongText, "the quick brown fox", "the quick brown fox")]
    [InlineData(SettingValueType.SecureString, "p@ssw0rd", "p@ssw0rd")]
    [InlineData(SettingValueType.Json, "{\"a\":1}", "{\"a\":1}")]
    public void Coerce_StringTypes_PassThrough(SettingValueType type, string raw, string expected)
    {
        var descriptor = NewSetting(type, raw).ToSettingDescriptor("MyModule");
        Assert.Equal(expected, descriptor.DefaultValue);
    }

    [Theory]
    [InlineData(SettingValueType.Integer, "42", 42)]
    [InlineData(SettingValueType.Integer, "-7", -7)]
    [InlineData(SettingValueType.PositiveInteger, "0", 0)]
    [InlineData(SettingValueType.PositiveInteger, "1000", 1000)]
    public void Coerce_IntegerTypes_ProducesInt(SettingValueType type, string raw, int expected)
    {
        var descriptor = NewSetting(type, raw).ToSettingDescriptor("MyModule");
        Assert.Equal(expected, descriptor.DefaultValue);
    }

    [Fact]
    public void Coerce_Boolean_ProducesBool()
    {
        Assert.Equal(true, NewSetting(SettingValueType.Boolean, "true").ToSettingDescriptor("M").DefaultValue);
        Assert.Equal(false, NewSetting(SettingValueType.Boolean, "false").ToSettingDescriptor("M").DefaultValue);
        // bool.Parse is case-insensitive
        Assert.Equal(true, NewSetting(SettingValueType.Boolean, "True").ToSettingDescriptor("M").DefaultValue);
    }

    [Fact]
    public void Coerce_Decimal_UsesInvariantCulture()
    {
        // Invariant culture: dot decimal separator. Locale-dependent commas
        // would be invalid input.
        var descriptor = NewSetting(SettingValueType.Decimal, "3.14").ToSettingDescriptor("M");
        Assert.Equal(3.14m, descriptor.DefaultValue);
    }

    [Fact]
    public void Coerce_DateTime_RoundtripKind()
    {
        var descriptor = NewSetting(SettingValueType.DateTime, "2026-04-29T10:00:00Z").ToSettingDescriptor("M");
        var asDate = (DateTime)descriptor.DefaultValue;
        Assert.Equal(2026, asDate.Year);
        Assert.Equal(4, asDate.Month);
        Assert.Equal(29, asDate.Day);
    }

    [Fact]
    public void Coerce_NullOrEmpty_DefaultValue_ProducesNull()
    {
        // <defaultValue/> with no text should yield null, not throw, so a
        // setting can be declared without a default.
        Assert.Null(NewSetting(SettingValueType.Integer, null).ToSettingDescriptor("M").DefaultValue);
        Assert.Null(NewSetting(SettingValueType.Integer, "").ToSettingDescriptor("M").DefaultValue);
    }

    [Fact]
    public void Coerce_AllowedValues_AppliesToEachEntry()
    {
        var setting = new ManifestSetting
        {
            Name = "M.PreferredAccountType",
            ValueType = SettingValueType.ShortText,
            AllowedValues = ["EmailAndPassword", "EmailWithLink", "Sso"],
        };

        var descriptor = setting.ToSettingDescriptor("M");

        Assert.NotNull(descriptor.AllowedValues);
        Assert.Equal(3, descriptor.AllowedValues.Length);
        Assert.Equal("EmailAndPassword", descriptor.AllowedValues[0]);
        Assert.Equal("Sso", descriptor.AllowedValues[2]);
    }

    [Fact]
    public void Coerce_AllowedValues_IntegerType_AllEntriesParsed()
    {
        var setting = new ManifestSetting
        {
            Name = "M.RetryAttempts",
            ValueType = SettingValueType.Integer,
            AllowedValues = ["1", "3", "5"],
        };

        var descriptor = setting.ToSettingDescriptor("M");

        Assert.Equal(new object[] { 1, 3, 5 }, descriptor.AllowedValues);
    }

    [Fact]
    public void Coerce_InvalidInteger_ThrowsFormatException()
    {
        var setting = NewSetting(SettingValueType.Integer, "not-a-number");

        var ex = Assert.Throws<FormatException>(() => setting.ToSettingDescriptor("M"));
        Assert.Contains("DefaultValue", ex.Message);
        Assert.Contains("Integer", ex.Message);
    }

    [Fact]
    public void Coerce_AllowedValues_OneInvalid_ThrowsFormatException()
    {
        var setting = new ManifestSetting
        {
            Name = "M.RetryAttempts",
            ValueType = SettingValueType.Integer,
            AllowedValues = ["1", "oops", "3"],
        };

        var ex = Assert.Throws<FormatException>(() => setting.ToSettingDescriptor("M"));
        Assert.Contains("AllowedValues", ex.Message);
    }

    [Fact]
    public void ToSettingDescriptor_StampsModuleId()
    {
        var setting = NewSetting(SettingValueType.Boolean, "true");

        var descriptor = setting.ToSettingDescriptor("VirtoCommerce.MyModule");

        Assert.Equal("VirtoCommerce.MyModule", descriptor.ModuleId);
    }

    // --- tenant attribute (UserProfile or any custom tenant type) ---

    [Fact]
    public void Tenant_DefaultsToNull_WhenAttributeOmitted()
    {
        // Plain <setting> with no tenant="…" attribute means "global".
        // The descriptor's Tenant must be null so UseSettingsFromModuleManifests
        // skips the RegisterSettingsForType pass for it.
        var setting = NewSetting(SettingValueType.Boolean, "true");

        Assert.Null(setting.Tenant);
        Assert.Null(setting.ToSettingDescriptor("M").Tenant);
    }

    [Fact]
    public void Tenant_FlowsToDescriptor_Verbatim()
    {
        // <setting tenant="UserProfile"> → descriptor.Tenant == "UserProfile".
        // No normalisation: the tenant-type comparison in
        // ISettingsRegistrar.GetSettingsForType is ordinal, so the manifest
        // must match the casing of the registered tenant type.
        var setting = NewSetting(SettingValueType.Boolean, "true");
        setting.Tenant = nameof(UserProfile);

        var descriptor = setting.ToSettingDescriptor("M");
        Assert.Equal("UserProfile", descriptor.Tenant);
    }

    [Fact]
    public void Tenant_CustomValue_PassesThroughVerbatim()
    {
        // Any non-empty tenant value passes through. UseSettingsFromModuleManifests
        // groups by Tenant and calls RegisterSettingsForType once per group, so a
        // module wiring up its own tenant type (e.g. "Store") can declare it
        // declaratively in the manifest with no platform-side change.
        var setting = NewSetting(SettingValueType.Boolean, "true");
        setting.Tenant = "Store";

        var descriptor = setting.ToSettingDescriptor("M");
        Assert.Equal("Store", descriptor.Tenant);
    }

    [Fact]
    public void ToSettingDescriptor_PreservesAllMetadataFlags()
    {
        var setting = new ManifestSetting
        {
            Name = "M.Foo",
            GroupName = "MyGroup|Section",
            DisplayName = "Foo display",
            ValueType = SettingValueType.Boolean,
            DefaultValue = "true",
            IsRequired = true,
            IsHidden = true,
            IsPublic = true,
            IsDictionary = false,
            IsLocalizable = true,
            RestartRequired = true,
        };

        var d = setting.ToSettingDescriptor("M");

        Assert.Equal("M.Foo", d.Name);
        Assert.Equal("MyGroup|Section", d.GroupName);
        Assert.Equal("Foo display", d.DisplayName);
        Assert.Equal(SettingValueType.Boolean, d.ValueType);
        Assert.Equal(true, d.DefaultValue);
        Assert.True(d.IsRequired);
        Assert.True(d.IsHidden);
        Assert.True(d.IsPublic);
        Assert.False(d.IsDictionary);
        Assert.True(d.IsLocalizable);
        Assert.True(d.RestartRequired);
    }

    private static ManifestSetting NewSetting(SettingValueType type, string defaultValue) => new()
    {
        Name = "M.TestSetting",
        ValueType = type,
        DefaultValue = defaultValue,
    };
}
