using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
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

        // Scalar
        if (!string.IsNullOrEmpty(section.Value) && !section.GetChildren().Any())
        {
            var convertedValue = ConvertToSettingValue(descriptor, section.Value);
            if (convertedValue == null)
            {
                return false;
            }

            value = convertedValue;
            return true;
        }

        // Array/object
        var children = section.GetChildren().ToArray();
        if (children.Length == 0)
        {
            return false;
        }

        var rawComplex = ReadComplex(children);
        var convertedComplexValue = ConvertToSettingValue(descriptor, rawComplex);
        if (convertedComplexValue == null)
        {
            return false;
        }

        value = convertedComplexValue;
        return true;
    }

    private static object ReadComplex(IConfigurationSection[] children)
    {
        // Array if numeric keys 0..n
        if (children.All(c => int.TryParse(c.Key, NumberStyles.Integer, CultureInfo.InvariantCulture, out _)))
        {
            return children
                .OrderBy(c => int.Parse(c.Key, CultureInfo.InvariantCulture))
                .Select(ReadNode)
                .ToArray();
        }

        // Object
        var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        foreach (var child in children)
        {
            dict[child.Key] = ReadNode(child);
        }
        return dict;
    }

    private static object ReadNode(IConfigurationSection section)
    {
        if (!string.IsNullOrEmpty(section.Value) && !section.GetChildren().Any())
        {
            return section.Value;
        }

        var children = section.GetChildren().ToArray();
        if (children.Length == 0)
        {
            return null;
        }

        return ReadComplex(children);
    }

    private static object ConvertToSettingValue(SettingDescriptor descriptor, object raw)
    {
        if (descriptor.IsDictionary)
        {
            return ConvertToDictionaryValue(descriptor.ValueType, raw);
        }

        if (raw is string str)
        {
            return ConvertScalar(descriptor.ValueType, str);
        }

        return null;
    }

    private static object[] ConvertToDictionaryValue(SettingValueType valueType, object raw)
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

        if (raw is IDictionary<string, object>)
        {
            return Array.Empty<object>();
        }

        if (raw is string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                // Explicit "empty" override (useful for env-vars)
                return Array.Empty<object>();
            }

            // Allow JSON array as string for env-vars, otherwise treat as single element
            try
            {
                var parsed = JsonSerializer.Deserialize<string[]>(s);
                if (parsed != null)
                {
                    return parsed.Select(x => ConvertScalar(valueType, x)).ToArray();
                }
            }
            catch
            {
                // ignore
            }

            return new[] { ConvertScalar(valueType, s) };
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
