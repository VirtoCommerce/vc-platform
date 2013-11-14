using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using System.Linq;


namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public abstract class SubTabsDefaultViewModel : ViewModelBase, ISupportDelayInitialization
    {

	    public List<ItemTypeHomeTab> SubItems { get; protected set; }

        ItemTypeHomeTab _currentTab;
        public ItemTypeHomeTab CurrentTab
        {
			get { return _currentTab; }
            set
            {
                if (_currentTab != value && value != null)
                {
                    var viewModel = value.ViewModel as ISupportDelayInitialization;
                    if (viewModel != null && _currentTab != null)
                    {
						Task.Run(() => viewModel.InitializeForOpen());
					}
                    _currentTab = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SetCurrentTabById(string id)
        {
            bool retVal = false;
            if (SubItems != null)
            {
                var tab = SubItems.FirstOrDefault(x => x.IdTab == id);
                if (tab != null)
                {
                    CurrentTab = tab;
                    retVal = true;
                }
            }
            return retVal;
        }

        #region ISupportdelayInitialization

        public void InitializeForOpen()
        {
			if (CurrentTab != null && CurrentTab.ViewModel is ISupportDelayInitialization)
            {
                ((ISupportDelayInitialization)CurrentTab.ViewModel).InitializeForOpen();
            }
        }

        #endregion


    }
}
