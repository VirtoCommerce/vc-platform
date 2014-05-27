using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Extensions;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
    public class CustomersHomeViewModel : CustomersCommonViewModel, ICustomersHomeViewModel,
                                          IVirtualListLoader<ICustomersDetailViewModel>, ISupportDelayInitialization
    {
        #region Dependencies

        private readonly IAuthenticationContext _authContext;
        private readonly IRepositoryFactory<ICustomerRepository> _customerRepository;
        private readonly IViewModelsFactory<ICustomersDetailViewModel> _customersDetailVmFactory;
        private readonly IViewModelsFactory<ICreateCustomerDialogViewModel> _wizardVmFactory;
        private readonly IViewModelsFactory<ICustomerChoiceDialogViewModel> _customerChoiceVmFactory;
        private readonly INavigationManager _navigationManager;
        private readonly TileManager _tileManager;

        #endregion

        #region Constructor

        public CustomersHomeViewModel(
            TileManager tileManager,
            INavigationManager navigationManager,
            IViewModelsFactory<ICustomerChoiceDialogViewModel> customerChoiceVmFactory,
            IViewModelsFactory<ICreateCustomerDialogViewModel> wizardVmFactory,
            IViewModelsFactory<ICustomersDetailViewModel> customersDetailVmFactory,
            IRepositoryFactory<ICustomerRepository> customerRepository,
            IAuthenticationContext authContext)
            : base(customerRepository, customersDetailVmFactory)
        {
            _tileManager = tileManager;
            _navigationManager = navigationManager;
            _customerChoiceVmFactory = customerChoiceVmFactory;
            _authContext = authContext;
            _customerRepository = customerRepository;
            _customersDetailVmFactory = customersDetailVmFactory;
            _wizardVmFactory = wizardVmFactory;

            OnPropertyChanged("UserId");

            CommandsInits();
            RequestInits();
            CaseFiltersInit();
            PopulateTiles();

            ViewTitle = new ViewTitleBase() { Title = "Customers", SubTitle = "Customer Service".Localize() };
            SendEventToShell();
            UpdateActivityTileOnShell();
        }

        #endregion

        #region Commands

        public DelegateCommand ResolveCasesCommand { get; private set; }
        public DelegateCommand TakeCasesCommand { get; private set; }
        public DelegateCommand ContextReopenCasesCommand { get; private set; }

        public DelegateCommand<IList<object>> ListViewSelectedItemsChangedCommand { get; private set; }

        public DelegateCommand FilterListBoxSelectedItemsChangedCommand { get; private set; }

        public DelegateCommand CreateCustomerCommand { get; private set; }
        public DelegateCommand ShowCustomerChoiseDialogEmailCaseCommand { get; private set; }

        #endregion

        #region Commands Implementation

        /// <summary>
        /// get selected items from HomeGridView
        /// </summary>
        /// <param name="items"></param>
        private void ListViewSelectedItemsChanged(IList<object> items)
        {
            SelectedItems.Clear();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    SelectedItems.Add(item);
                }
            }

            RaiseChangeCaseStatusCanExecute();
        }

        /// <summary>
        /// get selected items from filterListBox and refresh CaseList
        /// </summary>
        /// <param name="items"></param>
        private void FilterListboxSelectedItemsChanged()
        {
            RefreshCommand.Execute();
        }

        private bool CanShowCustomerChoiceDialog()
        {
            return true;
        }


        private async void ResolveCases(IEnumerable<object> selectedItems)
        {
            await ChangeCasesStatus(selectedItems, CaseStatus.Resolved);
        }

        private async void TakeCases(IEnumerable<object> selectedItems)
        {
            await ChangeCasesStatus(selectedItems, CaseStatus.Pending);
        }

        private async void ReopenCases(IEnumerable<object> selectedItems)
        {
            await ChangeCasesStatus(selectedItems, CaseStatus.Open);
        }

        private async Task ChangeCasesStatus(IEnumerable<object> selectedItems, CaseStatus status)
        {
            ShowLoadingAnimation = true;
            try
            {
                await Task.Run(() =>
                {
                    var selectedListViewItems =
                        selectedItems.Select(item => (VirtualListItem<ICustomersDetailViewModel>)item).ToList();
                    var selectedCases =
                        selectedListViewItems.Select(listItem => (CustomersDetailViewModel)listItem.Data).ToList();

                    using (var repository = _customerRepository.GetRepositoryInstance())
                    {
                        foreach (var customersDetailViewModel in selectedCases)
                        {
                            var caseFromDb =
                                repository.Cases.Where(c => c.CaseId == customersDetailViewModel.InnerItem.CaseId).SingleOrDefault();
                            if (caseFromDb != null)
                            {
                                caseFromDb.Status = status.ToString();
                            }
                        }

                        repository.UnitOfWork.Commit();
                    }

                });
            }
            catch (Exception ex)
            {
                string errorActionString = null;
                switch (status)
                {
                    case CaseStatus.Open:
                        errorActionString = "reopen";
                        break;

                    case CaseStatus.Pending:
                        errorActionString = "take";
                        break;

                    case CaseStatus.Resolved:
                        errorActionString = "resolve";
                        break;

                }

                ShowErrorDialog(ex, string.Format("An error occurred when trying to {0} selected cases: {1}", errorActionString, ex.InnerException.Message));
            }
            finally
            {
                ShowLoadingAnimation = false;
                RaiseChangeCaseStatusCanExecute();
                RefreshCommand.Execute();
            }
        }

        private bool CanResolveCases(IList<object> selectedItems)
        {
            return CanChangeCaseStatus(selectedItems, CaseStatus.Pending);
        }

        private bool CanTakeCases(IList<object> selectedItems)
        {
            return CanChangeCaseStatus(selectedItems, CaseStatus.Open);
        }

        private bool CanReopenCases(IList<object> selectedItems)
        {
            return CanChangeCaseStatus(selectedItems, CaseStatus.Resolved);
        }

        private bool CanChangeCaseStatus(IList<object> selectedItems, CaseStatus status)
        {
            var result = false;

            if (selectedItems != null && selectedItems.Count > 0)
            {
                var selectedListViewItems =
                    selectedItems.Select(item => (VirtualListItem<ICustomersDetailViewModel>)item).ToList();
                var selectedCases =
                    selectedListViewItems.Select(listItem => (CustomersDetailViewModel)listItem.Data).ToList();

                if (selectedCases.All(c => c.Status == status))
                {
                    result = true;
                }
            }

            switch (status)
            {
                case CaseStatus.Open:
                    IsTakeContextVisible = result;
                    if (result)
                    {
                        IsResolveContextVisible = !result;
                        IsReopenContextVisible = !result;
                    }

                    break;

                case CaseStatus.Pending:
                    IsResolveContextVisible = result;
                    if (result)
                    {
                        IsTakeContextVisible = !result;
                        IsReopenContextVisible = !result;
                    }
                    break;

                case CaseStatus.Resolved:
                    IsReopenContextVisible = result;
                    if (result)
                    {
                        IsTakeContextVisible = !result;
                        IsResolveContextVisible = !result;
                    }

                    break;
            }

            return result;
        }

        private void CreateCustomer()
        {
            if (CreateCustomerDialogRequest != null)
            {
                var itemVm =
                    _wizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", new Contact()));

                var confirmation = new ConditionalConfirmation { Title = "Enter customer details".Localize(), Content = itemVm };

                CreateCustomerDialogRequest.Raise(confirmation,
                                                  (x) =>
                                                  {
                                                      if (x.Confirmed)
                                                      {
                                                          var contactToAdd =
                                                              (x.Content as ICreateCustomerDialogViewModel)
                                                                  .InnerItem;
                                                          var emailToAdd =
                                                              (x.Content as ICreateCustomerDialogViewModel)
                                                                  .EmailForUserInput;
                                                          var phoneToAdd =
                                                              (x.Content as ICreateCustomerDialogViewModel)
                                                                  .PhoneForUserInput;

                                                          if (emailToAdd.Validate())
                                                          {
                                                              emailToAdd.Type = EmailType.Primary.ToString();
                                                              contactToAdd.Emails.Add(emailToAdd);
                                                              emailToAdd.MemberId = contactToAdd.MemberId;
                                                          }
                                                          if (phoneToAdd.Validate() &&
                                                              !string.IsNullOrEmpty(phoneToAdd.Number))
                                                          {
                                                              contactToAdd.Phones.Add(phoneToAdd);
                                                              phoneToAdd.MemberId = contactToAdd.MemberId;
                                                          }

                                                          using (var repository = _customerRepository.GetRepositoryInstance())
                                                          {
                                                              repository.Add(contactToAdd);
                                                              repository.UnitOfWork.Commit();
                                                          }

                                                          CreateNewCommerceManagerCase(contactToAdd, ContactActionState.Open,
                                                                                       CaseActionState.None);
                                                      }
                                                  });
            }
        }


        #endregion

        #region Requests

        public InteractionRequest<Confirmation> ShowCustomerChoiceDialogRequest { get; private set; }

        public InteractionRequest<ConditionalConfirmation> CreateCustomerDialogRequest { get; private set; }

        #endregion

        #region RequestImplementation

        private void RaiseShowCustomerChoiseDialogForEmailCase()
        {
            RaiseShowCustomerChoiseDialog();
        }

        private void RaiseShowCustomerChoiseDialog()
        {
            ShowCustomerChoiceDialogRequest.Raise(
                new Confirmation
                    {
                        Title = "Contact Choice".Localize(),
                        Content = _customerChoiceVmFactory.GetViewModelInstance()
                    },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        var viewModel = x.Content as CustomerChoiceDialogViewModel;

                        var parameters = new List<KeyValuePair<string, object>>
							{
								new KeyValuePair<string, object>("parentViewModel", this),
								new KeyValuePair<string, object>("caseAction", viewModel.CaseAction),
								new KeyValuePair<string, object>("contactAction", viewModel.ContactAction)
							};

                        switch (viewModel.CaseAction)
                        {
                            case CaseActionState.New:
                                parameters.Add(new KeyValuePair<string, object>("innerCase", new Case()));
                                break;

                            case CaseActionState.Open:
                                parameters.Add(new KeyValuePair<string, object>("innerCase", viewModel.SelectedCase));
                                break;
                        }

                        switch (viewModel.ContactAction)
                        {
                            case ContactActionState.New:
                                parameters.Add(new KeyValuePair<string, object>("innerContact", new Contact()));
                                break;

                            case ContactActionState.Open:
                                parameters.Add(new KeyValuePair<string, object>("innerContact", viewModel.CurrentContact));
                                break;
                        }


                        var customerServiceDetailViewModel = _customersDetailVmFactory.GetViewModelInstance(parameters.ToArray());
                        ((IOpenTracking)customerServiceDetailViewModel).OpenItemCommand.Execute();
                    }
                });
        }

        #endregion

        #region Private Methods

        private void CommandsInits()
        {
            ShowCustomerChoiseDialogEmailCaseCommand = new DelegateCommand(RaiseShowCustomerChoiseDialogForEmailCase);

            ResolveCasesCommand = new DelegateCommand(() => ResolveCases(SelectedItems),
                                                      () => CanResolveCases(SelectedItems));
            TakeCasesCommand = new DelegateCommand(() => TakeCases(SelectedItems), () => CanTakeCases(SelectedItems));
            ContextReopenCasesCommand = new DelegateCommand(() => ReopenCases(SelectedItems),
                                                            () => CanReopenCases(SelectedItems));

            ListViewSelectedItemsChangedCommand = new DelegateCommand<IList<object>>(ListViewSelectedItemsChanged);

            FilterListBoxSelectedItemsChangedCommand = new DelegateCommand(FilterListboxSelectedItemsChanged);

            CreateCustomerCommand = new DelegateCommand(CreateCustomer, () => _authContext.CheckPermission(PredefinedPermissions.CustomersCreateCustomer));
        }

        private void RequestInits()
        {
            ShowCustomerChoiceDialogRequest = new InteractionRequest<Confirmation>();
            CreateCustomerDialogRequest = new InteractionRequest<ConditionalConfirmation>();
        }


        /// <summary>
        /// Initialize ListBoxFilter
        /// </summary>
        private void CaseFiltersInit()
        {
            _caseFilterTypes = new List<CaseFilterTypeViewModel>();


            foreach (var value in Enum.GetValues(typeof(CaseFilterType)))
            {
                var fullDesc = CaseFilterTypeToDescriptionConverter.Current.Convert(value, null, null, null) as string;
                var shortDesc =
                    fullDesc == null
                        ? null
                        : fullDesc.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Take(2);

                var filterViewModel = new CaseFilterTypeViewModel()
                    {
                        Model = (CaseFilterType)value,
                        FullDescription = fullDesc,
                        ShortDescription =
                            shortDesc == null || shortDesc.Count() != 2
                                ? null
                                : shortDesc.ElementAt(0) + " " + shortDesc.ElementAt(1)
                    };

                _caseFilterTypes.Add(filterViewModel);
            }
        }


        private CaseFilterType[] GetSelectedFilters()
        {
            var retVal = new List<CaseFilterType>();
            //all Cases by default
            retVal = CaseFilterTypes.Select(x => x.Model).ToList();
            if (CaseFilterTypes.Any(x => x.IsActive))
            {
                retVal = CaseFilterTypes.Where(x => x.IsActive).Select(x => x.Model).ToList();
            }

            if (retVal.Any(x => x == CaseFilterType.AllCases))
            {
                retVal = retVal.Where(x => x == CaseFilterType.AllCases).Select(x => x).ToList();
            }

            return retVal.ToArray();
        }

        private void RaiseChangeCaseStatusCanExecute()
        {
            ResolveCasesCommand.RaiseCanExecuteChanged();
            TakeCasesCommand.RaiseCanExecuteChanged();
            ContextReopenCasesCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Properties

        private IList<object> _selectedItems = new List<object>();
        public IList<object> SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; }
        }


        private IList<object> _selectedFilterItems = new List<object>();
        public IList<object> SelectedFilterItems
        {
            get { return _selectedFilterItems; }
            set { _selectedFilterItems = value; }
        }


        private int _overallCount;
        public int OverallCount
        {
            get { return _overallCount; }
            set
            {
                _overallCount = value;
                OnPropertyChanged();
            }
        }


        private string _searchKeyword = string.Empty;
        public string SearchKeyWord
        {
            get { return _searchKeyword; }
            set { _searchKeyword = value; }
        }

        private ICollection<CaseFilterType> _selectedFilters;
        public ICollection<CaseFilterType> SelectedFilters
        {
            get { return _selectedFilters; }
            set
            {
                _selectedFilters = value;
                OnPropertyChanged();
            }
        }

        private bool _isReopenContextVisible;
        public bool IsReopenContextVisible
        {
            get { return _isReopenContextVisible; }
            set
            {
                _isReopenContextVisible = value;
                OnPropertyChanged();
            }
        }


        private bool _isResolveContextVisible;
        public bool IsResolveContextVisible
        {
            get { return _isResolveContextVisible; }
            set
            {
                _isResolveContextVisible = value;
                OnPropertyChanged();
            }
        }


        private bool _isTakeContextVisible;
        public bool IsTakeContextVisible
        {
            get { return _isTakeContextVisible; }
            set
            {
                _isTakeContextVisible = value;
                OnPropertyChanged();
            }
        }


        private string _tagSearchKeyword;
        public string TagSearchKeyword
        {
            get { return _tagSearchKeyword; }
            set
            {
                _tagSearchKeyword = value;
                OnPropertyChanged();
            }
        }



        public string UserId
        {
            get
            {
                return _authContext.CurrentUserId;
            }
        }


        #endregion

        #region IVirtualListLoader<ICustomerServiceDetailViewModel> Members

        public bool CanSort
        {
            get { return true; }
        }

        public IList<ICustomersDetailViewModel> LoadRange(int startIndex, int count,
                                                          SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            var retVal = new List<ICustomersDetailViewModel>();
            overallCount = 0;

            if (_authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll) ||
                _authContext.CheckPermission(PredefinedPermissions.CustomersViewAssignedCases))
            {
                using (var repository = _customerRepository.GetRepositoryInstance())
                {
                    var query = repository.Cases.Expand(c => c.Contact);

                    if (_authContext.CheckPermission(PredefinedPermissions.CustomersViewAssignedCases) &&
                        !_authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll))
                    {
                        query = query.Where(c => c.AgentId == UserId);
                    }
                    //keyword
                    if (!string.IsNullOrEmpty(SearchKeyWord))
                    {
                        query = query.Where(c => c.Title.Contains(SearchKeyWord));
                    }


                    //filter results
                    foreach (var filter in GetSelectedFilters())
                    {
                        switch (filter)
                        {
                            case CaseFilterType.AllCases:
                                query = query.Select(c => c);
                                break;
                            case CaseFilterType.AllUnresolvedCases:
                                string resolvedStatus = CaseStatus.Resolved.ToString();
                                query = query.Where(c => c.Status != resolvedStatus);
                                break;
                            case CaseFilterType.RecentlyUpdatedCases:
                                DateTime recentlyDate = DateTime.Now.AddDays(-3);
                                query = query.Where(c => c.LastModified >= recentlyDate);
                                break;
                            case CaseFilterType.UnassignedCases:
                                query = query.Where(c => c.AgentId != UserId);
                                break;
                        }
                    }

                    overallCount = query.ToList().Count();
                    OverallCount = overallCount;

                    var results = query.OrderBy(c => c.Title).Skip(startIndex).Take(count);

                    foreach (var item in results)
                    {
                        var parameters = new List<KeyValuePair<string, object>>();
                        parameters.Add(new KeyValuePair<string, object>("parentViewModel", this));
                        parameters.Add(new KeyValuePair<string, object>("innerCase", item));
                        if (item.Contact != null)
                        {
                            parameters.Add(new KeyValuePair<string, object>("innerContact", item.Contact));
                            parameters.Add(new KeyValuePair<string, object>("contactAction", ContactActionState.Open));
                        }
                        else
                        {
                            parameters.Add(new KeyValuePair<string, object>("innerContact", new Contact()));
                            parameters.Add(new KeyValuePair<string, object>("contactAction", ContactActionState.New));
                        }
                        parameters.Add(new KeyValuePair<string, object>("caseAction", CaseActionState.Open));

                        var viewModel = _customersDetailVmFactory.GetViewModelInstance(parameters.ToArray());
                        retVal.Add(viewModel);
                    }
                }
            }

            return retVal;
        }

        #endregion

        #region ISupportdelayInitialization

        public virtual void InitializeForOpen()
        {
            if (ListItemsSource == null)
            {
                OnUIThread(() => ListItemsSource = new VirtualList<ICustomersDetailViewModel>(this, 20, SynchronizationContext.Current));
            }
        }

        #endregion

        #region Tiles

        private bool NavigateToTabPage(string id)
        {
            _navigationManager.NavigateByName(NavigationNames.HomeName);

            var navigationData = _navigationManager.GetNavigationItemByName(NavigationNames.HomeName);
            if (navigationData != null)
            {
                _navigationManager.Navigate(navigationData);
                var customersMainViewModel =
                    _navigationManager.GetViewFromRegion(navigationData) as CustomersMainViewModel;

                return (customersMainViewModel != null && customersMainViewModel.SetCurrentTabById(id));
            }
            return false;
        }

        private void PopulateTiles()
        {
            //customers:customer:create && (customers:case:view:all || customers:case:view:assigned)
            if (_authContext.CheckPermission(PredefinedPermissions.CustomersCreateCustomer))
            {
                _tileManager.AddTile(new IconTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "NewCustomer",
                        TileIconSource = "Icon_Module_Customers",
                        TileTitle = "New customer",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 0,
                        IdColorSchema = TileColorSchemas.Schema1,
                        NavigateCommand = new DelegateCommand(async () =>
                            {
                                if (NavigateToTabPage(NavigationNames.HomeName))
                                {
                                    await Task.Run(() => Thread.Sleep(300)); // we need some time to parse xaml  
                                    CreateCustomerCommand.Execute();
                                }
                            })
                    });
            }

            if (_authContext.CheckPermission(PredefinedPermissions.CustomersCreateNewCase))
            {
                _tileManager.AddTile(new IconTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "NewEmailCase",
                        TileIconSource = "Icon_New_Case",
                        TileTitle = "New case",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 1,
                        IdColorSchema = TileColorSchemas.Schema2,
                        NavigateCommand = new DelegateCommand(async () =>
                            {
                                if (NavigateToTabPage(NavigationNames.HomeName))
                                {
                                    await Task.Run(() => Thread.Sleep(300)); // we need some time to parse xaml  
                                    CreateNewCommerceManagerCase(new Contact(), ContactActionState.New, CaseActionState.New);
                                }
                            })
                    });
            }

            if (_authContext.CheckPermission(PredefinedPermissions.Name_CustomersViewAssignedCases)
                || _authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll))
            {
                _tileManager.AddTile(new ListTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "Activity",
                        TileTitle = "Activity",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 3,
                        IdColorSchema = TileColorSchemas.Schema3,
                        NavigateCommand = new DelegateCommand(() => { }),
                        Refresh = async (tileItem) =>
                            {
                                var stringResult = await UpdateActivityTileOnShell();

                                var listTile = (ListTileItem)tileItem;
                                listTile.InfoList = new ObservableCollection<string>(stringResult);
                            }
                    });
            }

            if (_authContext.CheckPermission(PredefinedPermissions.Name_CustomersViewAssignedCases)
                || _authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll))
            {
                _tileManager.AddTile(new NumberTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "PendingCases",
                        TileTitle = "Pending cases",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 4,
                        IdColorSchema = TileColorSchemas.Schema2,
                        NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeName)),
                        Refresh = async (tileItem) =>
                            {
                                using (var repository = _customerRepository.GetRepositoryInstance())
                                {
                                    try
                                    {
                                        if (tileItem is NumberTileItem)
                                        {
                                            var query =
                                                await
                                                Task.Run(
                                                    () =>
                                                    repository.Cases.Where(c => c.Status == CaseStatus.Pending.ToString())
                                                              .Count());
                                            (tileItem as NumberTileItem).TileNumber = query.ToString();
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                    });
            }

            if (_authContext.CheckPermission(PredefinedPermissions.Name_CustomersViewAssignedCases)
                || _authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll))
            {
                _tileManager.AddTile(new ListTileItem()
                    {
                        IdModule = NavigationNames.MenuName,
                        IdTile = "LastestCases",
                        TileTitle = "Latest cases",
                        TileCategory = NavigationNames.ModuleName,
                        Order = 2,
                        Width = (double)TileSize.Double,
                        IdColorSchema = TileColorSchemas.Schema4,
                        NavigateCommand = new DelegateCommand(() => { }),
                        Refresh = async (tileItem) =>
                            {
                                using (var repository = _customerRepository.GetRepositoryInstance())
                                {
                                    try
                                    {
                                        var currTile = tileItem as ListTileItem;
                                        if (currTile != null)
                                        {
                                            var query =
                                                await
                                                Task.Run(() => repository.Cases.OrderBy(x => x.Created).Take(4));
                                            if (currTile.InfoList == null)
                                            {
                                                currTile.InfoList = new ObservableCollection<string>();
                                            }
                                            currTile.InfoList.Clear();
                                            foreach (var item in query)
                                            {
                                                currTile.InfoList.Add(item.Title);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                    });
            }

        }

        private Task<string[]> UpdateActivityTileOnShell()
        {
            var taskResult = Task.Run(() =>
                {
                    using (var repository = _customerRepository.GetRepositoryInstance())
                    {
                        var assignToMeCount =
                            repository.Cases.Where(c => c.AgentId == UserId).Count();


                        var nowBeginTime = DateTime.Now.Date;
                        var nowEndTime = DateTime.Now;
                        var newCasesArray =
                            repository.Cases.Where(c => c.Created >= nowBeginTime && c.Created < nowEndTime)
                                      .Expand(c => c.CommunicationItems)
                                      .Expand(c => c.Notes)
                                      .ToArray();
                        var newCasesCount =
                            newCasesArray.Count(c => c.CommunicationItems.Count == 0 && c.Notes.Count == 0);


                        var startOfWeekTime = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                        var dueThisWeekCount =
                            repository.Cases.Where(c => c.LastModified >= startOfWeekTime && c.LastModified < nowEndTime)
                                      .Count();


                        var pastDueCount = repository.Cases.Where(c => c.LastModified < startOfWeekTime)
                                                     .Count();

                        string[] result =
							{
								string.Format("{0}  Assigned to me".Localize(), assignToMeCount),
								string.Format("{0}  New".Localize(), newCasesCount),
								string.Format("{0}  Due this week".Localize(), dueThisWeekCount),
								string.Format("{0}  Past due".Localize(), pastDueCount)
							};
                        return result;
                    }
                });

            return taskResult;
        }

        #endregion
    }
}