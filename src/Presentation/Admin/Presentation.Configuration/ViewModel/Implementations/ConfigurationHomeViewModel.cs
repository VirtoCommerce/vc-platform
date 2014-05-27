using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        }

        private ConfigurationSection _currentTab;
        public ConfigurationSection CurrentTab
        {
            get { return _currentTab; }
            set
            {
                _currentTab = value;
                InitializeViewModel(_currentTab.ViewModel);
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

        private IViewModel _previousViewModel;

        private void InitializeViewModel(IViewModel currentVM)
        {
            if (currentVM != _previousViewModel)
            {
                if (_previousViewModel is IBuildSettingsViewModel)
                {
                    (_previousViewModel as BuildSettingsViewModel).IsActive = false;
                }

                if (currentVM is IBuildSettingsViewModel)
                {
                    (currentVM as BuildSettingsViewModel).IsActive = true;
                }

                _previousViewModel = currentVM;
            }

            if (currentVM is ISupportDelayInitialization)
            {
                (currentVM as ISupportDelayInitialization).InitializeForOpen();
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
                        CurrentTab = Settings.First(x => x.IdTab == id);
                    }
                });
            return retVal;
        }

    }
}
