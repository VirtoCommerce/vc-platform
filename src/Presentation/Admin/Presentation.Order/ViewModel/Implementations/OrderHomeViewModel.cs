using Microsoft.Practices.Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
    public class OrderHomeViewModel : ViewModelBase, IOrderHomeViewModel, IVirtualListLoader<IOrderViewModel>, ISupportDelayInitialization
    {
        #region Search fields
        public string SearchFilterTrackingNumber { get; set; }
        public string SearchFilterCustomerId { get; set; }
        public string SearchFilterEmail { get; set; }
        public string SearchFilterName { get; set; }
        public string SearchFilterSurname { get; set; }
        public string SearchFilterMiddlename { get; set; }
        public string SearchFilterKeyword { get; set; }
        public object SearchFilterOrderStatus { get; set; }
        public object SearchFilterStore { get; set; }
        #endregion

        public Store[] AvailableStores { get; private set; }
        public DelegateCommand ClearFiltersCommand { get; private set; }

        #region Dependencies

        private readonly NavigationManager _navManager;
        private readonly TileManager _tileManager;
        private readonly IViewModelsFactory<IOrderViewModel> _itemVmFactory;
        private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;
        private readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;

        #endregion
#if DESIGN
        public OrderHomeViewModel()
        {
        }
#endif

        public OrderHomeViewModel(
            IViewModelsFactory<IOrderViewModel> itemVmFactory,
            IRepositoryFactory<IOrderRepository> repositoryFactory,
            IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
            NavigationManager navManager, TileManager manager)
        {
            _navManager = navManager;
            _tileManager = manager;
            _itemVmFactory = itemVmFactory;
            _repositoryFactory = repositoryFactory;
            _storeRepositoryFactory = storeRepositoryFactory;

            ItemsListRefreshCommand = new DelegateCommand(RaiseItemsListRefreshInteractionRequest);
            SearchOrdersCommand = new DelegateCommand<string>(SearchOrders);
            ClearFiltersCommand = new DelegateCommand(DoClearFilters);
            OnUIThread(PopulateTiles);

            ViewTitle = new ViewTitleBase() { Title = "Orders", SubTitle = "Customer Service".Localize() };

            EventSystem.Subscribe<GoToOrderEvent>((ordId) => OnUIThread(() => OpenOrderFromCustomerService(ordId.OrderId)));
        }

        #region IOrderHomeViewModel Members

        public DelegateCommand ItemsListRefreshCommand { get; private set; }
        public DelegateCommand<string> SearchOrdersCommand
        {
            get;
            private set;
        }


        private ICollectionView _itemsSource;
        public ICollectionView OrderItemsSource
        {
            get
            {
                return _itemsSource;
            }
            private set
            {
                _itemsSource = value;
                OnPropertyChanged();
            }

        }
        #endregion

        private void RaiseItemsListRefreshInteractionRequest()
        {
            OrderItemsSource.Refresh();
        }

        private const string emailRegex = "^[\\w-]+(\\.[\\w-]+)*@([a-z0-9-]+(\\.[a-z0-9-]+)*?\\.[a-z]{2,6}|(\\d{1,3}\\.){3}\\d{1,3})(:\\d{4})?$";
        private const string orderTrackingNumberRegex = "^PO+[0-9]?";

        private void SearchOrders(string parameter)
        {
            if (parameter == "searchKeyword")
            {
                if (!string.IsNullOrEmpty(SearchFilterKeyword) && System.Text.RegularExpressions.Regex.IsMatch(SearchFilterKeyword, emailRegex))
                {
                    SearchFilterEmail = SearchFilterKeyword;
                    SearchFilterKeyword = null;
                }
                else
                    SearchFilterEmail = null;

                if (!string.IsNullOrEmpty(SearchFilterKeyword) && System.Text.RegularExpressions.Regex.IsMatch(SearchFilterKeyword, orderTrackingNumberRegex))
                {
                    SearchFilterTrackingNumber = SearchFilterKeyword;
                    SearchFilterKeyword = null;
                }
                else
                    SearchFilterTrackingNumber = null;
            }

            OrderItemsSource.Refresh();
        }

        private void DoClearFilters()
        {
            SearchFilterTrackingNumber = null;
            SearchFilterOrderStatus = null;
            SearchFilterStore = null;
            SearchFilterCustomerId = null;
            SearchFilterEmail = null;
            SearchFilterKeyword = null;

            OnPropertyChanged("SearchFilterTrackingNumber");
            OnPropertyChanged("SearchFilterOrderStatus");
            OnPropertyChanged("SearchFilterStore");
            OnPropertyChanged("SearchFilterCustomerId");
            OnPropertyChanged("SearchFilterEmail");
            OnPropertyChanged("SearchFilterKeyword");
        }


        #region IVirtualListLoader<Order> Members

        public bool CanSort
        {
            get
            {
                return true;
            }
        }

        public IList<IOrderViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
        {
            var retVal = new List<IOrderViewModel>();
            //Dictionary<string, string> sortBy = new Dictionary<string, string>();
            //foreach (SortDescription sortDesc in sortDescriptions)
            //{
            //    sortBy.Add(sortDesc.PropertyName, (sortDesc.Direction == ListSortDirection.Ascending ? "ASC" : "DESC"));
            //}

            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                var query = repository.Orders;

                //if (sortBy.Count > 0)
                //    criteria.Sort = new VirtoCommerce.Foundation.Search.SearchSort(sortBy.Keys.First(), sortBy[sortBy.Keys.First()] == "ASC");

                if (SearchFilterOrderStatus is OrderStatus)
                {
                    query = query.Where(x => x.Status == SearchFilterOrderStatus.ToString());
                }

                if (SearchFilterStore is Store)
                {
                    query = query.Where(x => x.StoreId == ((Store)SearchFilterStore).StoreId);
                }

                if (!string.IsNullOrEmpty(SearchFilterTrackingNumber))
                {
                    query = query.Where(x => x.TrackingNumber.Contains(SearchFilterTrackingNumber));
                }

                if (!string.IsNullOrEmpty(SearchFilterEmail))
                {
                    query = query.Where(x => (x.OrderAddresses.Any(b => b.Email.StartsWith(SearchFilterEmail))));
                    // query = query.Where(x => (x.OrderAddresses.Select(b => b.Email.StartsWith(SearchFilterEmail)).Count()) > 0);
                    //  query = query.Where(x => x.CustomerEmail == SearchFilterEmail);
                }

                if (!string.IsNullOrEmpty(SearchFilterCustomerId))
                {
                    query = query.Where(x => x.CustomerId == SearchFilterCustomerId);
                }

                if (!string.IsNullOrEmpty(SearchFilterKeyword))
                {
                    query = query.Where(x => x.TrackingNumber.Contains(SearchFilterKeyword));
                }

                overallCount = query.Count();

                IOrderedQueryable<Foundation.Orders.Model.Order> orderedItems;
                if (sortDescriptions.Count == 0)
                    // default orderBy createDate
                    orderedItems = query.OrderByDescending(x => x.Created);
                else
                    orderedItems = ApplySortDescriptions(query, sortDescriptions);

                var result = orderedItems.Skip(startIndex).Take(count).ToList();
                retVal.AddRange(result.Select(order => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", order))));
            }
            return retVal;
        }

        #endregion

        #region ISupportDelayInitialization Members

        public void InitializeForOpen()
        {
            if (OrderItemsSource == null)
            {
                OnUIThread(() => OrderItemsSource = new VirtualList<IOrderViewModel>(this, 25, SynchronizationContext.Current));

                using (var _storeRepository = _storeRepositoryFactory.GetRepositoryInstance())
                {
                    AvailableStores = _storeRepository.Stores.OrderBy(x => x.Name).ToArray();
                    OnPropertyChanged("AvailableStores");
                }
            }
        }

        #endregion

        #region Tiles

        private void NavigateToHome()
        {
            _navManager.NavigateByName(NavigationNames.HomeName);
        }

        private void PopulateTiles()
        {

            //if (_authContext.CheckPermission(PredefinedPermissions.OrdersAll))
            _tileManager.AddTile(new NumberTileItem()
            {
                IdModule = NavigationNames.MenuName,
                IdTile = "OrdersNeedAttention",
                TileNumber = "",
                TileTitle = "Need attention",
                TileCategory = NavigationNames.ModuleName,
                Order = 0,
                IdColorSchema = TileColorSchemas.Schema1,
                NavigateCommand = new DelegateCommand(NavigateToHome)
            });

            _tileManager.AddTile(new NumberTileItem()
            {
                IdModule = NavigationNames.MenuName,
                IdTile = "OrdersProcessedToday",
                TileNumber = "",
                TileTitle = "Processed today",
                TileCategory = NavigationNames.ModuleName,
                Order = 2,
                IdColorSchema = TileColorSchemas.Schema4,
                NavigateCommand = new DelegateCommand(NavigateToHome)
            });

            _tileManager.AddTile(new NumberTileItem()
            {
                IdModule = NavigationNames.MenuName,
                IdTile = "OrdersActive",
                TileTitle = "ACTIVE ORDERS",
                Order = 5,
                IdColorSchema = TileColorSchemas.Schema3,
                NavigateCommand = new DelegateCommand(NavigateToHome)
            });

            _tileManager.AddTile(new LinearChartTileItem()
            {
                IdModule = NavigationNames.MenuName,
                IdTile = "OrderChart",
                TileTitle = "MONTHLY SALES CHART",
                TileCategory = NavigationNames.ModuleName,
                Order = 6,
                Width = (double)TileSize.Triple,
                Height = (double)TileSize.Triple,
                IdColorSchema = TileColorSchemas.Schema3,
                SeriasBackground1 = new SolidColorBrush(Color.FromRgb(153, 153, 153)),
                SeriasBackground2 = new SolidColorBrush(Color.FromRgb(35, 159, 217)),
                ColumnSeriesName1 = "Last Year Sales".Localize(),
                ColumnSeriesName2 = "This Year Sales".Localize()
            });

        }

        #endregion

        #region PrivateMethods

        private void OpenOrderFromCustomerService(string orderId)
        {
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                var order = repository.Orders.Where(o => o.OrderGroupId == orderId).SingleOrDefault();

                if (order != null)
                {
                    var orderViewModel = _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", order));
                    ((IOpenTracking)orderViewModel).OpenItemCommand.Execute();
                }
            }
        }

        #endregion
    }
}
