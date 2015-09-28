using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
	public abstract class ServiceBase
	{
        #region Settings

		protected void LoadEntitySettings(ISettingsManager settingManager, Entity entity)
		{
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.IsTransient())
            {
                throw new ArgumentException("entity transistent");
            }
            var objectType = entity.GetType().Name;
            var storedSettings = settingManager.GetObjectSettings(objectType, entity.Id);

            var haveSettingsObjects = entity.GetFlatObjectsListWithInterface<IHaveSettings>();
            foreach (var haveSettingsObject in haveSettingsObjects)
            {
                // Replace in-memory settings with stored in database
                if (haveSettingsObject.Settings != null)
                {
                    var resultSettings = new List<SettingEntry>();

                    foreach (var setting in haveSettingsObject.Settings)
                    {
                        //First find object setting by name 
                        var storedSetting = storedSettings.FirstOrDefault(x => x.Name == setting.Name);
                        if (storedSetting != null)
                        {
                            resultSettings.Add(storedSetting);
                        }
                        else
                        {
                            //If not found take a setting value from global setting
                            var globalSetting = settingManager.GetSettingByName(setting.Name);
                            setting.Value = globalSetting != null ? globalSetting.Value : setting.DefaultValue;
                            resultSettings.Add(setting);
                        }
                    }
                    haveSettingsObject.Settings = resultSettings;
                }
                else
                {
                    haveSettingsObject.Settings = storedSettings;
                }
            }
        }

        protected void SaveEntitySettings(ISettingsManager settingManager, Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.IsTransient())
            {
                throw new ArgumentException("entity transistent");
            }
            var objectType = entity.GetType().Name;

            var haveSettingsObjects = entity.GetFlatObjectsListWithInterface<IHaveSettings>();

            foreach (var haveSettingsObject in haveSettingsObjects)
            {
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

        protected void RemoveEntitySettings(ISettingsManager settingManager, Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.IsTransient())
            {
                throw new ArgumentException("entity transistent");
            }
            var objectType = entity.GetType().Name;
            settingManager.RemoveObjectSettings(objectType, entity.Id);
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
