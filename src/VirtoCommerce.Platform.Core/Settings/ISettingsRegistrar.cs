using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Settings
{
    public interface ISettingsRegistrar
    {
        IEnumerable<SettingDescriptor> AllRegisteredSettings { get; }
        /// <summary>
        /// Register new setting definitions 
        /// </summary>
        void RegisterSettings(IEnumerable<SettingDescriptor> settings, string moduleId = null);
        /// <summary>
        /// Assign settings for concrete type
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="typeName"></param>
        void RegisterSettingsForType(IEnumerable<SettingDescriptor> settings, string typeName);

        /// <summary>
        /// Returns all settings descriptors for given type name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IEnumerable<SettingDescriptor> GetSettingsForType(string typeName);
    }
}
