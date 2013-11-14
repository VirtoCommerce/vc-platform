using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Configuration.Model;
using VirtoCommerce.ManagementClient.Configuration.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ManagementClient.Configuration.ViewModel.Implementations
{
    public class ConfigurationHomeViewModel : ViewModelBase, IConfigurationHomeViewModel, ISupportDelayInitialization
	{
		#region Dependencies

		private readonly NavigationManager _navigationManager;

		#endregion

		public ConfigurationHomeViewModel(NavigationManager navigationManager)
		{
			_navigationManager = navigationManager;
            ViewTitle = new ViewTitleBase { Title = "Settings" };

            TabChangedCommand = new DelegateCommand<object>(RaiseTabChangedRequest);
        }

        public DelegateCommand<object> TabChangedCommand { get; private set; }

        private ConfigurationSection _currentTab;
        public ConfigurationSection CurrentTab
        {
            get { return _currentTab; }
            set 
            {
                _currentTab = value;
                RaiseTabChangedRequest(_currentTab.ViewModel);
                OnPropertyChanged();
            }

        }

        public List<ConfigurationSection> Settings { get; private set; }

        #region ISupportDelayInitialization Members

        public void InitializeForOpen()
        {
			IsInitializing = true;
            if (Settings == null)
            {
                Settings = ConfigurationManager.Settings.OrderByDescending(x => x.Order).ToList();
                
                Settings.ForEach(x => _navigationManager.RegisterCompositeCommand(x.ViewModel));
                if (Settings.Any())
                {
                    CurrentTab = Settings[0];
                }
            }
			IsInitializing = false;
        }

        #endregion

        private ConfigurationSection _currentConfigurationSection;

        private void RaiseTabChangedRequest(object arg)
        {
            if (arg is ConfigurationSection)
            {
                if (_currentConfigurationSection != null && _currentConfigurationSection.ViewModel is IBuildSettingsViewModel)
                {
                    if (!((arg as ConfigurationSection).ViewModel is IBuildSettingsViewModel))
                    {
                        (_currentConfigurationSection.ViewModel as BuildSettingsViewModel).IsActive = false;
                        _currentConfigurationSection = arg as ConfigurationSection;
                    }
                }
                else
                {
                    _currentConfigurationSection = arg as ConfigurationSection;
                    if (_currentConfigurationSection.ViewModel is IBuildSettingsViewModel)
                    {
                        (_currentConfigurationSection.ViewModel as BuildSettingsViewModel).IsActive = true;
                    }
                }
                if (_currentConfigurationSection.ViewModel is ISupportDelayInitialization)
                {
                    (_currentConfigurationSection.ViewModel as ISupportDelayInitialization).InitializeForOpen();
                }
            }
            else if (arg is ISupportDelayInitialization)
            {
                (arg as ISupportDelayInitialization).InitializeForOpen();
            }
        }

        public bool SetCurrentTabById(string id)
        {
            bool retVal = false;
            OnUIThread(async () =>
                {
					while (IsInitializing)
                    {
                        await Task.Run(() => Thread.Sleep(300));
                    }
                    retVal = Settings != null && Settings.Any(x => x.IdTab == id);
                    if (retVal)
                    {
                        CurrentTab = Settings.First(x => x.IdTab==id);
                    }
                });
            return retVal;
        }

    }
}
