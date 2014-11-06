using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Implementations
{
    [ImplementPropertyChanged]
    class AccountHomeViewModel : ViewModelHomeEditableBase<Account>, IAccountHomeViewModel, IVirtualListLoader<IAccountViewModel>, ISupportDelayInitialization
    {
        #region Dependencies

        private readonly IAuthenticationContext _authContext;
        private readonly IRepositoryFactory<ISecurityRepository> _securityRepository;
        private readonly IViewModelsFactory<IAccountViewModel> _itemVmFactory;
        private readonly IViewModelsFactory<ICreateAccountViewModel> _wizardVmFactory;
        private readonly NavigationManager _navManager;
        private readonly TileManager _tileManager;
        private readonly ISecurityService _securityService;

        #endregion

        #region Constructor

        public AccountHomeViewModel(
            ISecurityService securityService,
            IRepositoryFactory<ISecurityRepository> securityRepository,
            IViewModelsFactory<IAccountViewModel> itemVmFactory,
            IViewModelsFactory<ICreateAccountViewModel> wizardVmFactory,
            IAuthenticationContext authContext,
            NavigationManager navManager,
            TileManager tileManager)
        {
            _authContext = authContext;
            _securityRepository = securityRepository;
            _itemVmFactory = itemVmFactory;
            _wizardVmFactory = wizardVmFactory;
            _securityService = securityService;
            _navManager = navManager;
            _tileManager = tileManager;

            ClearFiltersCommand = new DelegateCommand(DoClearFilters);
            PopulateTiles();
        }

        #endregion

        #region SearchFields
        public string SearchFilterAccountName { get; set; }
        public object SearchFilterType { get; set; }
        public object SearchFilterStatus { get; set; }
        #endregion

        #region Commands
        public DelegateCommand ClearFiltersCommand { get; private set; }
        #endregion

        #region Private methods
        private void DoClearFilters()
        {
            SearchFilterType = null;
            SearchFilterStatus = null;
            SearchFilterAccountName = null;
        }
        #endregion

        #region ViewModelHomeEditableBase

        protected override bool CanItemAddExecute()
        {
            return true;
        }

        protected override bool CanItemDeleteExecute(IList x)
        {
            return x != null && x.Count > 0;
        }

        protected override void RaiseItemAddInteractionRequest()
        {
            var item = new Account();
            var itemVM = _wizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

            CommonWizardDialogRequest.Raise(new ConditionalConfirmation { Title = "Create account".Localize(), Content = itemVM }, (x) =>
            {
                if (x.Confirmed)
                {
                    // always create SiteAdministrator in here. Simple users are created in Customers (Cases) module
                    // create via MemberShip without roles
                    _securityService.CreateAdminUserAsync(item.MemberId, item.UserName, itemVM.Password);

                    //Add roles
                    using (var itemRepository = _securityRepository.GetRepositoryInstance())
                    {
                        var originalItem = itemRepository.Accounts.Where(a => a.UserName == item.UserName).Expand("RoleAssignments/Role").SingleOrDefault();
                        if (originalItem != null)
                        {

                            //TODO this property MemberId must be set in service while creating user
                            originalItem.MemberId = originalItem.AccountId;

                            foreach (var roleAssignments in item.RoleAssignments)
                            {
                                originalItem.RoleAssignments.Add(roleAssignments);
                            }
                            itemRepository.UnitOfWork.Commit();
                        }
                    }
                    Refresh();
                }
            });
        }

        protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
        {
            var selectionCount = selectedItemsList.Count;
            if (selectionCount > 0)
            {
                var selectedItems = selectedItemsList.Cast<VirtualListItem<IAccountViewModel>>().ToList();
                string joinedNames;
                var names = selectedItems.
                    Take(4).
                    Select(x => ((ViewModelBase)x.Data).DisplayName);

                joinedNames = string.Join(", ", names);
                if (selectionCount > 4)
                    joinedNames += "...";

                CommonConfirmRequest.Raise(new ConditionalConfirmation()
                {
                    Content = string.Format("Are you sure you want to delete \'{0}\'?".Localize(null, LocalizationScope.DefaultCategory), joinedNames),
                    Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
                },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        selectedItems.ForEach(y => _securityService.DeleteUserAsync(y.Data.InnerItem.UserName));
                        Refresh();
                    }
                });
            }
        }

        #endregion

        #region IAccountHomeViewModel

        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set { _searchKeyword = value; }

        }

        private ICollectionView _selectedItems;
        public ICollectionView SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            private set
            {
                _selectedItems = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region IVirtualListLoader<ICustomerServiceDetailViewModel> Members

        public bool CanSort
        {
            get { return true; }
        }

        public IList<IAccountViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            var retVal = new List<IAccountViewModel>();

            using (var repository = _securityRepository.GetRepositoryInstance())
            {
                var query = repository.Accounts.Expand(x => x.RoleAssignments);

                if (!string.IsNullOrEmpty(SearchKeyword))
                {
                    query = query.Where(a => a.UserName.Contains(SearchKeyword) || a.MemberId.Contains(SearchKeyword) || a.RoleAssignments.Any(x => x.Role.Name.Contains(SearchKeyword)));
                }

                if (SearchFilterStatus is AccountState)
                {
                    query = query.Where(x => x.AccountState == (int)((AccountState)SearchFilterStatus));
                }

                if (SearchFilterType is RegisterType)
                {
                    query = query.Where(x => x.RegisterType == (int)((RegisterType)SearchFilterType));
                }

                if (!string.IsNullOrEmpty(SearchFilterAccountName))
                {
                    query = query.Where(x => x.UserName.Contains(SearchFilterAccountName));
                }

                overallCount = query.Count();

                var results = query.OrderBy(c => c.AccountId).Skip(startIndex).Take(count).ToList();

                retVal.AddRange(results.Select(item => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item))));
            }
            return retVal;
        }

        #endregion

        #region ISupportdelayInitialization

        public void InitializeForOpen()
        {
            if (ListItemsSource == null)
            {
                OnUIThread(() => ListItemsSource = new VirtualList<IAccountViewModel>(this, 25, SynchronizationContext.Current));
            }

        }

        #endregion

        #region Tiles

        private bool NavigateToTabPage(string id)
        {
            var navigationData = _navManager.GetNavigationItemByName(NavigationNames.HomeName);
            if (navigationData != null)
            {
                _navManager.Navigate(navigationData);
                var mainViewModel = _navManager.GetViewFromRegion(navigationData) as SubTabsDefaultViewModel;

                return (mainViewModel != null && mainViewModel.SetCurrentTabById(id));
            }
            return false;
        }

        private void PopulateTiles()
        {
            if (_authContext.CheckPermission(PredefinedPermissions.SecurityAccounts))
            {
                _tileManager.AddTile(new NumberTileItem()
                    {
                        IdModule = "ConfigurationMenu",
                        IdTile = "Users",
                        TileTitle = "Users",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 0,
                        IdColorSchema = TileColorSchemas.Schema1,
                        NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeName)),
                        Refresh = async (tileItem) =>
                            {
                                try
                                {
                                    using (var repository = _securityRepository.GetRepositoryInstance())
                                    {
                                        if (tileItem is NumberTileItem)
                                        {
                                            var query = await Task.Factory.StartNew(() => repository.Accounts.Count());
                                            (tileItem as NumberTileItem).TileNumber = query.ToString();
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                    });
            }

        }

        #endregion
    }
}
