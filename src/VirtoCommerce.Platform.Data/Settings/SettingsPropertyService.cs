using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class SettingsPropertyService : ISettingsPropertyService
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ISettingsOverrideProvider _overrideProvider;

        public SettingsPropertyService(
            ISettingsManager settingsManager,
            ISettingsOverrideProvider overrideProvider)
        {
            _settingsManager = settingsManager;
            _overrideProvider = overrideProvider;
        }

        public Task<IReadOnlyList<SettingPropertySchema>> GetSchemaAsync(
            SettingsPropertySearchCriteria criteria,
            string tenantType = null)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            var query = _settingsManager.AllRegisteredSettings.AsQueryable();

            // Filter out hidden settings
            query = query.Where(x => !x.IsHidden);

            // Filter by module
            if (!string.IsNullOrEmpty(criteria.ModuleId))
            {
                query = query.Where(x => x.ModuleId == criteria.ModuleId);
            }

            // For tenant scope, also include settings registered for that type
            if (!string.IsNullOrEmpty(tenantType))
            {
                var typeSettings = _settingsManager.GetSettingsForType(tenantType);
                var typeSettingNames = new HashSet<string>(typeSettings.Select(x => x.Name), StringComparer.OrdinalIgnoreCase);
                query = query.Where(x => typeSettingNames.Contains(x.Name));
            }

            // Keyword search
            if (!string.IsNullOrEmpty(criteria.Keyword))
            {
                var keyword = criteria.Keyword;
                query = query.Where(x =>
                    (x.Name != null && x.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (x.DisplayName != null && x.DisplayName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (x.GroupName != null && x.GroupName.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            }

            // Sort by group then name
            query = query.OrderBy(x => x.GroupName).ThenBy(x => x.Name);

            var result = query.Select(descriptor => MapToSchema(descriptor, tenantType)).ToList();

            return Task.FromResult<IReadOnlyList<SettingPropertySchema>>(result);
        }

        public async Task<Dictionary<string, object>> GetValuesAsync(
            string tenantType = null,
            string tenantId = null,
            bool modifiedOnly = false)
        {
            var descriptors = GetDescriptors(tenantType);
            var names = descriptors.Select(x => x.Name).ToArray();

            var settings = await _settingsManager.GetObjectSettingsAsync(names, tenantType, tenantId);
            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var setting in settings)
            {
                var value = setting.Value ?? setting.DefaultValue;

                if (modifiedOnly)
                {
                    // Only include if value differs from default
                    if (!ValuesEqual(value, setting.DefaultValue))
                    {
                        result[setting.Name] = setting.Value;
                    }
                }
                else
                {
                    result[setting.Name] = value;
                }
            }

            return result;
        }

        public async Task SaveValuesAsync(
            Dictionary<string, object> values,
            string tenantType = null,
            string tenantId = null)
        {
            ArgumentNullException.ThrowIfNull(values);

            var allDescriptors = _settingsManager.AllRegisteredSettings
                .ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);

            var settingsToSave = new List<ObjectSettingEntry>();

            foreach (var kvp in values)
            {
                if (!allDescriptors.TryGetValue(kvp.Key, out var descriptor))
                {
                    throw new InvalidOperationException($"Unknown setting: '{kvp.Key}'");
                }

                var entry = new ObjectSettingEntry(descriptor)
                {
                    Value = ConvertValue(kvp.Value, descriptor.ValueType),
                    ObjectType = tenantType,
                    ObjectId = tenantId
                };

                settingsToSave.Add(entry);
            }

            if (settingsToSave.Count > 0)
            {
                await _settingsManager.SaveObjectSettingsAsync(settingsToSave);
            }
        }

        private IEnumerable<SettingDescriptor> GetDescriptors(string tenantType)
        {
            if (!string.IsNullOrEmpty(tenantType))
            {
                return _settingsManager.GetSettingsForType(tenantType);
            }

            return _settingsManager.AllRegisteredSettings.Where(x => !x.IsHidden);
        }

        private SettingPropertySchema MapToSchema(SettingDescriptor descriptor, string tenantType)
        {
            var isReadOnly = _overrideProvider.TryGetCurrentValue(descriptor, tenantType, null, out _);

            return new SettingPropertySchema
            {
                Name = descriptor.Name,
                DisplayName = descriptor.DisplayName,
                GroupName = descriptor.GroupName,
                ModuleId = descriptor.ModuleId,
                ValueType = descriptor.ValueType,
                DefaultValue = descriptor.DefaultValue,
                AllowedValues = descriptor.AllowedValues,
                IsRequired = descriptor.IsRequired,
                IsReadOnly = isReadOnly,
                IsDictionary = descriptor.IsDictionary,
                IsLocalizable = descriptor.IsLocalizable,
                RestartRequired = descriptor.RestartRequired
            };
        }

        private static bool ValuesEqual(object current, object defaultValue)
        {
            if (current == null && defaultValue == null)
            {
                return true;
            }

            if (current == null || defaultValue == null)
            {
                return false;
            }

            return string.Equals(
                current.ToString(),
                defaultValue.ToString(),
                StringComparison.Ordinal);
        }

        private static object ConvertValue(object value, SettingValueType valueType)
        {
            if (value == null)
            {
                return null;
            }

            // Newtonsoft.Json (used by platform via AddNewtonsoftJson) deserializes
            // Dictionary<string, object> values as JValue/JToken.
            // Unwrap and return the correctly typed value directly.
            // Important: we must return here, not fall through to the final switch,
            // because JToken integers are extracted as long and Convert.ToInt32()
            // would throw OverflowException for values exceeding Int32.MaxValue.
            if (value is JToken jToken)
            {
                return jToken.Type switch
                {
                    JTokenType.Null or JTokenType.None => null,
                    JTokenType.Boolean => jToken.Value<bool>(),
                    JTokenType.Integer => valueType switch
                    {
                        SettingValueType.Integer or SettingValueType.PositiveInteger => jToken.Value<int>(),
                        SettingValueType.Decimal => jToken.Value<decimal>(),
                        _ => jToken.Value<long>()
                    },
                    JTokenType.Float => jToken.Value<decimal>(),
                    _ => jToken.ToString()
                };
            }

            // System.Text.Json fallback (in case STJ is used in some contexts).
            if (value is JsonElement jsonElement)
            {
                return valueType switch
                {
                    SettingValueType.Boolean => jsonElement.GetBoolean(),
                    SettingValueType.Integer or SettingValueType.PositiveInteger => jsonElement.GetInt32(),
                    SettingValueType.Decimal => jsonElement.GetDecimal(),
                    _ => jsonElement.ToString()
                };
            }

            return valueType switch
            {
                SettingValueType.Boolean => Convert.ToBoolean(value),
                SettingValueType.Integer or SettingValueType.PositiveInteger => Convert.ToInt32(value),
                SettingValueType.Decimal => Convert.ToDecimal(value),
                _ => value.ToString()
            };
        }
    }
}
