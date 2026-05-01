using System;
using System.Globalization;
using System.Xml.Serialization;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// XML POCO mirroring <see cref="SettingDescriptor"/> field-for-field.
    /// Lets frontend-only modules (no .NET assembly) declare platform settings
    /// in <c>module.manifest</c> and have them registered automatically at
    /// platform startup via <see cref="ISettingsRegistrar"/>.
    ///
    /// See <c>docs/developer-guide/manifest-settings.md</c> for the full
    /// proposal, schema reference, and v2-API consumption examples.
    /// </summary>
    public class ManifestSetting
    {
        /// <summary>
        /// Optional tenant-type name. Default (null/empty) registers the
        /// setting as a global platform setting. Set to <c>UserProfile</c>
        /// to register it as a per-user setting; the caller's value is
        /// then read/written via the <c>/api/platform/settings/v2/me/*</c>
        /// endpoints under the authenticated user's profile. Any other
        /// non-empty value is treated as a custom tenant-type name and
        /// passed verbatim to <see cref="ISettingsRegistrar.RegisterSettingsForType"/>.
        /// Compared ordinally — match the casing of the registered tenant
        /// type (canonical for <c>UserProfile</c> is the form
        /// <c>nameof(UserProfile)</c> resolves to).
        /// </summary>
        [XmlAttribute("tenant")]
        public string Tenant { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("groupName")]
        public string GroupName { get; set; }

        [XmlElement("displayName")]
        public string DisplayName { get; set; }

        [XmlElement("valueType")]
        public SettingValueType ValueType { get; set; }

        /// <summary>
        /// Default value as a string. Coerced to the declared
        /// <see cref="ValueType"/> when materialised into a
        /// <see cref="SettingDescriptor"/>.
        /// </summary>
        [XmlElement("defaultValue")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Optional enumerated list of permitted values (rendered as a
        /// dropdown in the admin settings UI). Each entry is coerced to
        /// <see cref="ValueType"/> at registration time.
        /// </summary>
        [XmlArray("allowedValues")]
        [XmlArrayItem("value")]
        public string[] AllowedValues { get; set; }

        [XmlElement("isRequired")]
        public bool IsRequired { get; set; }

        [XmlElement("isHidden")]
        public bool IsHidden { get; set; }

        [XmlElement("isPublic")]
        public bool IsPublic { get; set; }

        [XmlElement("isDictionary")]
        public bool IsDictionary { get; set; }

        [XmlElement("isLocalizable")]
        public bool IsLocalizable { get; set; }

        [XmlElement("restartRequired")]
        public bool RestartRequired { get; set; }

        /// <summary>
        /// Builds a <see cref="SettingDescriptor"/> ready to feed into
        /// <see cref="ISettingsRegistrar.RegisterSettings"/>. Coerces
        /// <see cref="DefaultValue"/> and <see cref="AllowedValues"/>
        /// strings to the declared <see cref="ValueType"/>.
        /// </summary>
        /// <param name="moduleId">
        /// Stamped onto <see cref="SettingDescriptor.ModuleId"/> so the v2
        /// schema endpoint's <c>?moduleId=</c> filter works automatically.
        /// </param>
        /// <returns>
        /// A populated <see cref="SettingDescriptor"/>.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown when <see cref="DefaultValue"/> or any
        /// <see cref="AllowedValues"/> entry can't be coerced to
        /// <see cref="ValueType"/>. Callers should catch this and surface
        /// the error against the owning module rather than failing
        /// the whole module load.
        /// </exception>
        public SettingDescriptor ToSettingDescriptor(string moduleId)
        {
            return new SettingDescriptor
            {
                Name = Name,
                ModuleId = moduleId,
                GroupName = GroupName,
                DisplayName = DisplayName,
                ValueType = ValueType,
                DefaultValue = CoerceValue(DefaultValue, ValueType, nameof(DefaultValue)),
                AllowedValues = AllowedValues == null
                    ? null
                    : Array.ConvertAll(AllowedValues, x => CoerceValue(x, ValueType, nameof(AllowedValues))),
                IsRequired = IsRequired,
                IsHidden = IsHidden,
                IsPublic = IsPublic,
                IsDictionary = IsDictionary,
                IsLocalizable = IsLocalizable,
                RestartRequired = RestartRequired,
                Tenant = Tenant,
            };
        }

        /// <summary>
        /// Coerces an XML-string value into the runtime CLR type implied
        /// by <see cref="SettingValueType"/>. Returns <c>null</c> for
        /// null/empty input.
        /// </summary>
        private static object CoerceValue(string raw, SettingValueType valueType, string field)
        {
            if (string.IsNullOrEmpty(raw))
            {
                return null;
            }

            try
            {
                return valueType switch
                {
                    SettingValueType.ShortText or
                    SettingValueType.LongText or
                    SettingValueType.SecureString or
                    SettingValueType.Json => raw,

                    SettingValueType.Integer or
                    SettingValueType.PositiveInteger => int.Parse(raw, CultureInfo.InvariantCulture),

                    SettingValueType.Decimal => decimal.Parse(raw, CultureInfo.InvariantCulture),

                    SettingValueType.Boolean => bool.Parse(raw),

                    SettingValueType.DateTime => DateTime.Parse(
                        raw,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind),

                    // Future-proofing: treat unknown types as raw strings
                    // rather than throwing, so adding a new SettingValueType
                    // doesn't retroactively break old manifests.
                    _ => raw,
                };
            }
            catch (Exception ex) when (ex is FormatException or OverflowException or ArgumentException)
            {
                throw new FormatException(
                    $"Setting <{field}> value \"{raw}\" cannot be coerced to {valueType}: {ex.Message}",
                    ex);
            }
        }
    }
}
