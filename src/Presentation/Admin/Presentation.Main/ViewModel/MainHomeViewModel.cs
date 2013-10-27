using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;

namespace VirtoCommerce.ManagementClient.Main.ViewModel
{
	public class MainHomeViewModel : ViewModelBase, IMainHomeViewModel, ISupportDelayInitialization
	{

		#region Dependencies

		private readonly IAppConfigRepository _repository;
		private readonly TileManager _tileManager;
		private readonly NavigationManager _navManager;

		#endregion

#if DESIGN
		public MainHomeViewModel()
		{
		}
#endif

		public override bool IsBackTrackingDisabled { get { return true; } }

		#region ctor

		public MainHomeViewModel(IAppConfigRepository repository, NavigationManager navManager, TileManager tileManager)
		{
			_repository = repository;
			_tileManager = tileManager;
			_navManager = navManager;

			PopulateTiles();
			CommandsInit();
		}

		#endregion

		#region Private Methods

		private void CommandsInit()
		{
			NavigateToCustomersCommand = new DelegateCommand(NavigateToCustomers);
			NavigateToCatalogCommand = new DelegateCommand(NavigateToCatalog);
			NavigateToOrdersCommand = new DelegateCommand(NavigateToOrders);
			NavigateToMarketingCommand = new DelegateCommand(NavigateToMarketing);
			NavigateToSettingsCommand = new DelegateCommand(NavigateToSettings);
		}



		#endregion

		#region Commands

		public DelegateCommand NavigateToCustomersCommand { get; private set; }
		public DelegateCommand NavigateToCatalogCommand { get; private set; }
		public DelegateCommand NavigateToOrdersCommand { get; private set; }
		public DelegateCommand NavigateToMarketingCommand { get; private set; }
		public DelegateCommand NavigateToSettingsCommand { get; private set; }

		#endregion

		#region Command implementation

		private void NavigateToSettings()
		{
			NavigateTo(Configuration.NavigationNames.HomeName);
		}

		private void NavigateToMarketing()
		{
			NavigateTo(Marketing.NavigationNames.HomeName);
		}

		private void NavigateToCustomers()
		{
			NavigateTo(Customers.NavigationNames.HomeName);
		}

		private void NavigateToCatalog()
		{
			NavigateTo(Catalog.NavigationNames.HomeName);
		}

		private void NavigateToOrders()
		{
			NavigateTo(Order.NavigationNames.HomeName);
		}

		private void NavigateTo(string name)
		{
			var navigationData = _navManager.GetNavigationItemByName(name);
			if (navigationData == null)
			{
				string navigationItemId = Guid.NewGuid().ToString();
				navigationData = new NavigationItem(navigationItemId, null);
			}

			_navManager.Navigate(navigationData);
		}

		#endregion

		#region Properties

		public ObservableCollection<Statistic> StatisticsList { get; set; }


		#endregion

		#region ISupportDelayInitialization

		public void InitializeForOpen()
		{
			OnUIThread(async () =>
				{
					_tileManager.Refresh();
					StatisticsList = await Task.Run(() => new ObservableCollection<Statistic>(_repository.Statistics.ToList()));

					GetStatisticForOrders();
					GetStatisticForChartLastYear();
					GetStatisticForChartThisYear();
				});
		}

		#endregion

		#region private methods


		private void GetStatisticForOrders()
		{
			var statItem = StatisticsList.FirstOrDefault(st => st.Key.Contains("Primary"));
			var tile = _tileManager.GetTile("OrderTestPrimaryKey") as NumberTileItem;
			if (statItem != null && tile != null)
			{
				tile.TileNumber = statItem.Value;
				tile.TileTitle = statItem.Name;
			}

		}

		private void GetStatisticForChartLastYear()
		{
			var tile = _tileManager.GetTile("OrderChart") as LinearChartTileItem;

			if (StatisticsList != null && tile != null)
			{
				var result = StatisticsList.Where(st => st.Key.Contains("chart") && st.Key.Contains("last"))
							  .OrderBy(st => st.Key)
							  .ToList();
				tile.SeriasArrays1.Clear();
				foreach (var statistic in result)
				{
					int val;
					Int32.TryParse(statistic.Value, out val);
					tile.SeriasArrays1.Add(statistic.Name, val);
				}

			}
		}

		private void GetStatisticForChartThisYear()
		{
			var tile = _tileManager.GetTile("OrderChart") as LinearChartTileItem;

			if (StatisticsList != null && tile != null)
			{
				var result = StatisticsList == null
											 ? new List<Statistic>()
											 : StatisticsList.Where(
												 st => st.Key.Contains("chart") && st.Key.Contains("this"))
															 .OrderBy(st => st.Key)
															 .ToList();
				tile.SeriasArrays2.Clear();
				foreach (var statistic in result)
				{
					int val;
					Int32.TryParse(statistic.Value, out val);
					tile.SeriasArrays2.Add(statistic.Name, val);
				}

			}
		}

		#endregion

		#region Tiles

		public List<ITileItem> CustomerTiles { get; private set; }
		public List<ITileItem> OrderTiles { get; private set; }
		public List<ITileItem> CatalogTiles { get; private set; }
		public List<ITileItem> MarketingTiles { get; private set; }
		public List<ITileItem> SettingTiles { get; private set; }

		private void PopulateTiles()
		{
			OnUIThread(() =>
				{
					CustomerTiles = _tileManager.GetTilesByIdModule("CustomersMenu"); //TODO: replace to const
					OrderTiles = _tileManager.GetTilesByIdModule("OrderMenu"); //TODO: replace to const
					CatalogTiles = _tileManager.GetTilesByIdModule("CatalogMenu"); //TODO: replace to const
					MarketingTiles = _tileManager.GetTilesByIdModule("MarketingMenu"); //TODO: replace to const
					SettingTiles = _tileManager.GetTilesByIdModule("ConfigurationMenu"); //TODO: replace to const
				});
		}

		#endregion

	}
}
