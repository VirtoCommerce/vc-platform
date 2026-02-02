using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings;

/// <summary>
/// Reads settings overrides from configuration keys:
/// VirtoCommerce:Settings:Override:{CurrentValue|DefaultValue}:{Global|Tenants}:{...}:{SettingName} = value
/// </summary>
public class ConfigurationSettingsOverrideProvider : ISettingsOverrideProvider
{
    private const string Root = "VirtoCommerce:Settings:Override";
    private readonly IConfiguration _configuration;

    public ConfigurationSettingsOverrideProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool TryGetCurrentValue(SettingDescriptor descriptor, string objectType, string objectId, out object value)
    {
        return TryGet(descriptor, "CurrentValue", objectType, objectId, out value);
    }

    public bool TryGetDefaultValue(SettingDescriptor descriptor, string objectType, string objectId, out object value)
    {
        return TryGet(descriptor, "DefaultValue", objectType, objectId, out value);
    }

    private bool TryGet(SettingDescriptor descriptor, string bucket, string objectType, string objectId, out object value)
    {
        value = null;
        ArgumentNullException.ThrowIfNull(descriptor);

        var name = descriptor.Name;

        if (!TryGetFromTenantAndGlobal(descriptor, bucket, objectType, objectId, name, out value))
        {
            // Virto Cloud doesn't support dots in setting names, try replacing them with underscores
            var virtoCloudName = descriptor.Name.Replace(".", "_");
            return TryGetFromTenantAndGlobal(descriptor, bucket, objectType, objectId, virtoCloudName, out value);
        }

        return true;
    }

    private bool TryGetFromTenantAndGlobal(SettingDescriptor descriptor, string bucket, string objectType, string objectId, string name, out object value)
    {
        // Tenant-specific override first
        if (!string.IsNullOrEmpty(objectType) && !string.IsNullOrEmpty(objectId))
        {
            var tenantPath = $"{Root}:{bucket}:Tenants:{objectType}:{objectId}:{name}";
            if (TryReadSectionValue(_configuration.GetSection(tenantPath), descriptor, out value))
            {
                return true;
            }
        }

        // Global override
        var globalPath = $"{Root}:{bucket}:Global:{name}";
        if (TryReadSectionValue(_configuration.GetSection(globalPath), descriptor, out value))
        {
            return true;
        }

        return false;
    }

    private static bool TryReadSectionValue(IConfigurationSection section, SettingDescriptor descriptor, out object value)
    {
        value = null;

        var children = section.GetChildren().ToArray();

        if (!children.Any())
        {
            // Scalar
            if (string.IsNullOrEmpty(section.Value))
            {
                return false;
            }

            var convertedValue = ConvertToSettingValue(descriptor, section.Value);
            if (convertedValue == null)
            {
                return false;
            }
            value = convertedValue;

            return true;
        }
        else
        {
            // Complex 
            var rawComplex = children.Select(c => c.Value).ToArray();
            var convertedComplexValue = ConvertToSettingValue(descriptor, rawComplex);
            if (convertedComplexValue == null)
            {
                return false;
            }

            value = convertedComplexValue;
            return true;
        }
    }

    private static object ConvertToSettingValue(SettingDescriptor descriptor, object raw)
    {
        if (descriptor.IsDictionary)
        {
            return ConvertToAllowedValues(descriptor.ValueType, raw);
        }
        else if (raw is string str)
        {
            return ConvertScalar(descriptor.ValueType, str);
        }

        return null;
    }

    private static object[] ConvertToAllowedValues(SettingValueType valueType, object raw)
    {
        if (raw == null)
        {
            return Array.Empty<object>();
        }

        if (raw is string[] arr)
        {
            return arr.Select(x => ConvertScalar(valueType, x)).ToArray();
        }

        if (raw is object[] objectArray)
        {
            return objectArray
                .Select(item => item is string str ? ConvertScalar(valueType, str) : item)
                .ToArray();
        }

        if (raw is string s)
        {
            return [ConvertScalar(valueType, s)];
        }

        return Array.Empty<object>();
    }

    private static object ConvertScalar(SettingValueType valueType, string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return null;
        }

        try
        {
            return valueType switch
            {
                SettingValueType.Boolean => bool.Parse(str),
                SettingValueType.DateTime => DateTime.Parse(str, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                SettingValueType.Decimal => decimal.Parse(str, NumberStyles.Number, CultureInfo.InvariantCulture),
                SettingValueType.Integer or SettingValueType.PositiveInteger => int.Parse(str, NumberStyles.Integer, CultureInfo.InvariantCulture),
                _ => str
            };
        }
        catch
        {
            return str;
        }
    }
}
