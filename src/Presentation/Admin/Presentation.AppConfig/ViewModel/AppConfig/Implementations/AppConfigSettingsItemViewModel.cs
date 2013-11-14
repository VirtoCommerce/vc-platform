using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations
{
	public class AppConfigSettingsItemViewModel : ViewModelBase, IAppConfigSettingsItemViewModel
	{
		#region IAppConfigSettingsItemViewModel members

		public string Id { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public bool IsSystem { get; set; }

		#endregion


		public void SetProperties(Setting inputItem)
		{
			if (inputItem != null)
			{
				Id = inputItem.SettingId;
				Name = inputItem.Name;

				Value = string.Empty;
				if (inputItem.SettingValues != null)
				{
					foreach (var settingValue in inputItem.SettingValues)
					{
						Value += settingValue.ToString() + " ";
					}
				}

				IsSystem = inputItem.IsSystem;

				OnPropertyChanged("Name");
				OnPropertyChanged("Value");
				OnPropertyChanged("IsSystem");
			}
		}

	}
}
