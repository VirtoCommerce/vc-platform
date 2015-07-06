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

		protected void LoadObjectSettings(ISettingsManager settingManager, object obj)
		{
			var haveSettingsObjects = obj.GetFlatListObjectsWithInterface<IHaveSettings>();
			foreach (var haveSettingsObject in haveSettingsObjects)
			{
				var entity = haveSettingsObject as Entity;
				if (entity != null && !entity.IsTransient())
				{
					var storedSettings = settingManager.GetObjectSettings(entity.GetType().Name, entity.Id);
					//Merge default settings and stored in db 
					if (haveSettingsObject.Settings != null)
					{
						var resultSettings = new List<SettingEntry>();
						foreach (var setting in haveSettingsObject.Settings)
						{
							var storedSetting = storedSettings.FirstOrDefault(x => x.Name == setting.Name);
							if (storedSetting != null)
							{
								resultSettings.Add(storedSetting);
							}
							else
							{
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
		}

		protected void SaveObjectSettings(ISettingsManager settingManager, object obj)
		{
			var haveSettingsObjects = obj.GetFlatListObjectsWithInterface<IHaveSettings>();
			foreach (var haveSettingsObject in haveSettingsObjects)
			{
				var entity = haveSettingsObject as Entity;

				if (entity != null && !entity.IsTransient())
				{
					var settings = new List<SettingEntry>();
					if (haveSettingsObject.Settings != null)
					{
						//Save settings
						foreach (var setting in haveSettingsObject.Settings)
						{
							setting.ObjectId = entity.Id;
							setting.ObjectType = entity.GetType().Name;
							settings.Add(setting);
						}
					}
					settingManager.SaveSettings(settings.ToArray());
				}
			}
		}

		protected void RemoveObjectSettings(ISettingsManager settingManager, object obj)
		{
			var haveSettingsObjects = obj.GetFlatListObjectsWithInterface<IHaveSettings>();
			foreach (var haveSettingsObject in haveSettingsObjects)
			{
				var entity = haveSettingsObject as Entity;

				if (entity != null && !entity.IsTransient())
				{
					settingManager.RemoveObjectSettings(entity.GetType().Name, entity.Id);
				}
			}
		}

	
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
				RemoveAction = (x) =>
				{
					repository.Remove(x);
				},
				AddAction = (x) =>
				{
					repository.Add(x);
				}
			};

			return retVal;
		}

		
	}
}
