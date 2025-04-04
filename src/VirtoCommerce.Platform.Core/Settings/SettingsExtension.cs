using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.Settings
{
    public static class SettingsExtension
    {
        /// <summary>
        /// Deep load and populate settings values for entity and all nested objects 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        public static async Task DeepLoadSettingsAsync(this ISettingsManager manager, IHasSettings entity)
        {
            await DeepLoadSettingsAsync(manager, entity, true);
        }

        /// <summary>
        /// Deep load and populate settings values for entity and all nested objects
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        /// <param name="excludeHidden"></param>
        /// <returns></returns>
        public static Task DeepLoadSettingsAsync(this ISettingsManager manager, IHasSettings entity, bool excludeHidden)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return DeepLoadSettingsAsyncImpl(manager, entity, excludeHidden);
        }

        private static async Task DeepLoadSettingsAsyncImpl(ISettingsManager manager, IHasSettings entity, bool excludeHidden)
        {
            //Deep load settings values for all object contains settings
            var hasSettingsObjects = entity.GetFlatObjectsListWithInterface<IHasSettings>();
            foreach (var hasSettingsObject in hasSettingsObjects)
            {
                var typeSettings = manager.GetSettingsForType(hasSettingsObject.TypeName);

                if (excludeHidden)
                {
                    typeSettings = typeSettings.Where(x => !x.IsHidden);
                }

                var names = typeSettings.Select(x => x.Name).ToList();
                if (names.Any())
                {
                    hasSettingsObject.Settings = (await manager.GetObjectSettingsAsync(names, hasSettingsObject.TypeName, hasSettingsObject.Id)).ToList();
                }
            }
        }

        public static async Task DeepSaveSettingsAsync(this ISettingsManager manager, IHasSettings entry)
        {
            await manager.DeepSaveSettingsAsync([entry]);
        }
        /// <summary>
        /// Deep save entity and all nested objects settings values
        /// </summary>
        public static async Task DeepSaveSettingsAsync(this ISettingsManager manager, IEnumerable<IHasSettings> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            var forSaveSettings = new List<ObjectSettingEntry>();
            foreach (var entry in entries)
            {
                var haveSettingsObjects = entry.GetFlatObjectsListWithInterface<IHasSettings>();

                foreach (var haveSettingsObject in haveSettingsObjects.Where(x => x.Settings != null))
                {
                    //Save settings
                    foreach (var setting in haveSettingsObject.Settings)
                    {
                        setting.ObjectId = haveSettingsObject.Id;
                        setting.ObjectType = haveSettingsObject.TypeName;
                        forSaveSettings.Add(setting);
                    }
                }
            }
            if (forSaveSettings.Any())
            {
                await manager.SaveObjectSettingsAsync(forSaveSettings);
            }
        }

        /// <summary>
        /// Deep remove entity and all nested objects settings values
        /// </summary>
        public static async Task DeepRemoveSettingsAsync(this ISettingsManager manager, IHasSettings entry)
        {
            await manager.DeepRemoveSettingsAsync([entry]);
        }

        /// <summary>
        /// Deep remove entity and all nested objects settings values
        /// </summary>
        public static async Task DeepRemoveSettingsAsync(this ISettingsManager manager, IEnumerable<IHasSettings> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }
            var foDeleteSettings = new List<ObjectSettingEntry>();
            foreach (var entry in entries)
            {
                var haveSettingsObjects = entry.GetFlatObjectsListWithInterface<IHasSettings>();
                foDeleteSettings.AddRange(haveSettingsObjects.SelectMany(x => x.Settings).Distinct());
            }
            await manager.RemoveObjectSettingsAsync(foDeleteSettings);
        }

        /// <summary>
        /// Takes default value from the setting descriptor
        /// </summary>
        public static TValue GetValue<TValue>(this ISettingsManager manager, SettingDescriptor descriptor)
        {
            return manager.GetValueAsync<TValue>(descriptor).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Takes default value from the setting descriptor
        /// </summary>
        public static Task<TValue> GetValueAsync<TValue>(this ISettingsManager manager, SettingDescriptor descriptor)
        {
            var defaultValue = default(TValue);

            if (descriptor.DefaultValue is TValue defaultSettingValue)
            {
                defaultValue = defaultSettingValue;
            }

            return manager.GetValueInternalAsync(descriptor.Name, defaultValue);
        }

        private static async Task<T> GetValueInternalAsync<T>(this ISettingsManager manager, string name, T defaultValue)
        {
            var result = defaultValue;

            try
            {
                var objectSetting = await manager.GetObjectSettingAsync(name);
                if (objectSetting?.Value != null)
                {
                    result = (T)objectSetting.Value;
                }
            }
            catch (PlatformException)
            {
                // This exception can be thrown when there is no setting registered with given name.
                // VC Platform 2.x was returning the default value in this case, so the platform 3.x will do the same.
            }
            return result;
        }

        public static void SetValue<T>(this ISettingsManager manager, string name, T value)
        {
            manager.SetValueAsync(name, value).GetAwaiter().GetResult();
        }

        public static async Task SetValueAsync<T>(this ISettingsManager manager, string name, T value)
        {
            var objectSetting = await manager.GetObjectSettingAsync(name);
            objectSetting.Value = value;
            await manager.SaveObjectSettingsAsync([objectSetting]);
        }

        public static TValue GetValue<TValue>(this IEnumerable<ObjectSettingEntry> objectSettings, SettingDescriptor descriptor)
        {
            var defaultValue = default(TValue);

            if (descriptor.DefaultValue is TValue defaultSettingValue)
            {
                defaultValue = defaultSettingValue;
            }

            return objectSettings.GetValueInternal(descriptor.Name, defaultValue);
        }

        private static T GetValueInternal<T>(this IEnumerable<ObjectSettingEntry> objectSettings, string settingName, T defaultValue)
        {
            var retVal = defaultValue;
            var setting = objectSettings.FirstOrDefault(x => x.Name.EqualsIgnoreCase(settingName));
            if (setting != null && setting.Value != null)
            {
                retVal = (T)Convert.ChangeType(setting.Value, typeof(T), CultureInfo.InvariantCulture);
            }
            return retVal;
        }
    }
}
