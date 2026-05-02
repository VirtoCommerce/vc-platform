using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Represent setting meta description
    /// </summary>
    public class SettingDescriptor : ValueObject, IEntity
    {
        public string Id { get; set; }
        /// <summary>
        /// The flag indicates that you need to restart the application to apply this setting changes.
        /// </summary>
        public bool RestartRequired { get; set; }
        /// <summary>
        /// The module id which setting belong to
        /// </summary>
        public string ModuleId { get; set; }
        /// <summary>
        /// Setting group name
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Setting name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display setting name
        /// </summary>
        public string DisplayName { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Flag indicates that this setting doesn't need to be displayed on the UI
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Flag indicates that this settings is accessible for client application via XAPI. By default, false.
        /// </summary>
        public bool IsPublic { get; set; }

        public SettingValueType ValueType { get; set; }
        public object[] AllowedValues { get; set; }
        public object DefaultValue { get; set; }
        /// <summary>
        /// The flag indicates that current setting is just an editable dictionary and hasn't any concrete value
        /// </summary>
        public bool IsDictionary { get; set; }

        /// <summary>
        /// This flag indicates that the setting value(s) can be localized
        /// </summary>
        public bool IsLocalizable { get; set; }

        /// <summary>
        /// Optional tenant-type name. Empty/null means a global setting;
        /// any non-empty value means the setting is also registered for
        /// that tenant type via
        /// <see cref="ISettingsRegistrar.RegisterSettingsForType"/>.
        /// Used by <c>UseSettingsFromModuleManifests</c> to mirror the
        /// platform's own pattern (register globally first, then register
        /// per-tenant subsets) without two parallel descriptor collections.
        /// Populated automatically when a setting is parsed from
        /// <c>module.manifest</c>'s <c>tenant="…"</c> attribute; left null
        /// for settings registered programmatically from
        /// <c>IModule.Initialize</c> (those are routed by the call-site
        /// choosing <c>RegisterSettings</c> vs <c>RegisterSettingsForType</c>
        /// directly).
        /// </summary>
        public string Tenant { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
