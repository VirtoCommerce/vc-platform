using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public abstract class ServiceBase
    {
        protected string GetObjectTypeName(object obj)
        {
            return obj.GetType().FullName;
        }

        #region Dynamic Properties

        protected void LoadDynamicProperties(IDynamicPropertyService service, object obj)
        {
            var objectsWithDynamicProperties = obj.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            foreach (var objectWithDynamicProperties in objectsWithDynamicProperties)
            {
                var entity = objectWithDynamicProperties as Entity;

                if (entity != null && !entity.IsTransient())
                {
                    var storedProperties = service.GetObjectValues(GetObjectTypeName(entity), entity.Id);

                    // Replace in-memory properties with stored in database
                    if (objectWithDynamicProperties.DynamicPropertyValues != null)
                    {
                        var result = new List<DynamicPropertyObjectValue>();

                        foreach (var value in objectWithDynamicProperties.DynamicPropertyValues)
                        {
                            var storedProperty = storedProperties.FirstOrDefault(v => v.Property.Name == value.Property.Name);
                            result.Add(storedProperty ?? value);
                        }

                        objectWithDynamicProperties.DynamicPropertyValues = result;
                    }
                    else
                    {
                        objectWithDynamicProperties.DynamicPropertyValues = storedProperties;
                    }
                }
            }
        }

        protected void SaveDynamicProperties(IDynamicPropertyService service, object obj)
        {
            var objectsWithDynamicProperties = obj.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            foreach (var objectWithDynamicProperties in objectsWithDynamicProperties)
            {
                var entity = objectWithDynamicProperties as Entity;

                if (entity != null && !entity.IsTransient())
                {
                    var objectType = GetObjectTypeName(entity);
                    var result = new List<DynamicPropertyObjectValue>();

                    if (objectWithDynamicProperties.DynamicPropertyValues != null)
                    {
                        foreach (var value in objectWithDynamicProperties.DynamicPropertyValues)
                        {
                            value.Property.ObjectType = objectType;
                            value.ObjectId = entity.Id;
                            result.Add(value);
                        }
                    }

                    service.SaveObjectValues(result.ToArray());
                }
            }
        }

        protected void RemoveDynamicProperties(IDynamicPropertyService service, object obj)
        {
            var objectsWithDynamicProperties = obj.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            foreach (var haveSettingsObject in objectsWithDynamicProperties)
            {
                var entity = haveSettingsObject as Entity;

                if (entity != null && !entity.IsTransient())
                {
                    service.DeleteObjectValues(GetObjectTypeName(entity), entity.Id);
                }
            }
        }

        #endregion

        #region Settings

        protected void LoadObjectSettings(ISettingsManager settingManager, object obj)
        {
            var haveSettingsObjects = obj.GetFlatObjectsListWithInterface<IHaveSettings>();

            foreach (var haveSettingsObject in haveSettingsObjects)
            {
                var entity = haveSettingsObject as Entity;

                if (entity != null && !entity.IsTransient())
                {
                    var storedSettings = settingManager.GetObjectSettings(entity.GetType().Name, entity.Id);

                    // Replace in-memory settings with stored in database
                    if (haveSettingsObject.Settings != null)
                    {
                        var resultSettings = new List<SettingEntry>();

                        foreach (var setting in haveSettingsObject.Settings)
                        {
                            var storedSetting = storedSettings.FirstOrDefault(x => x.Name == setting.Name);
                            resultSettings.Add(storedSetting ?? setting);
                        }

                        haveSettingsObject.Settings = resultSettings;
                    }
                    else
                    {
                        haveSettingsObject.Settings = storedSettings;
                    }
                }
            }
        }

        protected void SaveObjectSettings(ISettingsManager settingManager, object obj)
        {
            var haveSettingsObjects = obj.GetFlatObjectsListWithInterface<IHaveSettings>();

            foreach (var haveSettingsObject in haveSettingsObjects)
            {
                var entity = haveSettingsObject as Entity;

                if (entity != null && !entity.IsTransient())
                {
                    var objectType = entity.GetType().Name;
                    var settings = new List<SettingEntry>();

                    if (haveSettingsObject.Settings != null)
                    {
                        //Save settings
                        foreach (var setting in haveSettingsObject.Settings)
                        {
                            setting.ObjectId = entity.Id;
                            setting.ObjectType = objectType;
                            settings.Add(setting);
                        }
                    }

                    settingManager.SaveSettings(settings.ToArray());
                }
            }
        }

        protected void RemoveObjectSettings(ISettingsManager settingManager, object obj)
        {
            var haveSettingsObjects = obj.GetFlatObjectsListWithInterface<IHaveSettings>();

            foreach (var haveSettingsObject in haveSettingsObjects)
            {
                var entity = haveSettingsObject as Entity;

                if (entity != null && !entity.IsTransient())
                {
                    settingManager.RemoveObjectSettings(entity.GetType().Name, entity.Id);
                }
            }
        }

        #endregion

        protected virtual void CommitChanges(IRepository repository)
        {
            try
            {
                repository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                ex.ThrowFaultException();
            }
        }

        protected virtual ObservableChangeTracker GetChangeTracker(IRepository repository)
        {
            var retVal = new ObservableChangeTracker
            {
                RemoveAction = x => repository.Remove(x),
                AddAction = x => repository.Add(x)
            };

            return retVal;
        }
    }
}
