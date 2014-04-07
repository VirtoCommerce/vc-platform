using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations
{
	public class CacheViewModel : ViewModelBase, ICacheViewModel
	{
		private readonly ICacheService _service;

		public CacheViewModel(ICacheService service)
		{
			_service = service;
			CacheTypes = new[]
                {
                    new ItemTypeSelectionModel("Html output".Localize(), "This is where rendered html is stored.".Localize()),
                    new ItemTypeSelectionModel("Database".Localize(),"This is where queried objects are stored.".Localize())
                };

			_allCachesText = "All caches".Localize();
			AnimationText = "Processing...".Localize();
			CacheParameters = new ObservableCollection<string>();
			ClearCacheCommand = new DelegateCommand(DoClearCache, () => !string.IsNullOrEmpty(SelectedCacheType) && !string.IsNullOrEmpty(SelectedCacheParameter) && !ShowLoadingAnimation);
		}

		private string _selectedCacheType;
		private readonly string _allCachesText;
		public ItemTypeSelectionModel[] CacheTypes { get; private set; }

		public string SelectedCacheType
		{
			get { return _selectedCacheType; }
			set
			{
				_selectedCacheType = value;
				if (_selectedCacheType != null)
				{
					CacheParameters.Clear();
					if (CacheTypes[0].Value == _selectedCacheType)
					{
						CacheParameters.Add(Constants.ControllerNameHome);
						CacheParameters.Add(Constants.ControllerNameAsset);
						CacheParameters.Add(Constants.ControllerNameCatalog);
						CacheParameters.Add(Constants.ControllerNameSearch);
						CacheParameters.Add(Constants.ControllerNameStore);
						CacheParameters.Add(_allCachesText);
					}
					else
					{
						CacheParameters.Add(Constants.DisplayTemplateCachePrefix);
						CacheParameters.Add(Constants.EmailTemplateCachePrefix);
						CacheParameters.Add(Constants.PricelistCachePrefix);
						CacheParameters.Add(Constants.CatalogOutlineBuilderCachePrefix);
						CacheParameters.Add(Constants.PromotionsCachePrefix);
						CacheParameters.Add(Constants.DynamicContentCachePrefix);
						CacheParameters.Add(Constants.CatalogCachePrefix);
						CacheParameters.Add(Constants.CountriesCachePrefix);
						CacheParameters.Add(Constants.ReviewsCachePrefix);
						CacheParameters.Add(Constants.SeoCachePrefix);
						CacheParameters.Add(Constants.SettingsCachePrefix);
						CacheParameters.Add(Constants.ShippingCachePrefix);
						CacheParameters.Add(Constants.StoreCachePrefix);
						CacheParameters.Add(Constants.UserCachePrefix);
					}

					OnPropertyChanged("IsCacheTypeSelected");
				}

				OnPropertyChanged();
			}
		}

		public ObservableCollection<string> CacheParameters { get; private set; }

		public string SelectedCacheParameter { get; set; }

		public bool IsCacheTypeSelected
		{
			get { return !string.IsNullOrEmpty(SelectedCacheType); }
		}

		public DelegateCommand ClearCacheCommand { get; private set; }

		private async void DoClearCache()
		{
			ShowLoadingAnimation = true;
			ClearCacheCommand.RaiseCanExecuteChanged();
			try
			{
				if (CacheTypes[0].Value == SelectedCacheType)
				{
					var controllerParameter = SelectedCacheParameter == _allCachesText ? null : SelectedCacheParameter;
					await Task.Run(() => _service.ClearOuputCache(controllerParameter, null));
				}
				else
				{
					await Task.Run(() => _service.ClearDatabaseCache(SelectedCacheParameter));
				}
			}
			finally
			{
				ShowLoadingAnimation = false;
				ClearCacheCommand.RaiseCanExecuteChanged();
			}
		}
	}

	public class ItemTypeSelectionModel
	{
		public ItemTypeSelectionModel(string value, string description = null)
		{
			Value = value;
			Description = description;
		}

		public string Value { get; set; }

		public string Description { get; set; }
	}
}
