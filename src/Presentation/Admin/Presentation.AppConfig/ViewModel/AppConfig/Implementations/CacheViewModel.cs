using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using Microsoft.Practices.Prism;

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
			CacheParameters = new ObservableCollection<KeyValuePair<string, string>>();
            ClearCacheCommand = new DelegateCommand(DoClearCache, () => !string.IsNullOrEmpty(SelectedCacheType) && !string.IsNullOrEmpty(SelectedCacheParameter) && !ShowLoadingAnimation);
        }

        private string _selectedCacheType;
        private readonly string _allCachesText;
        public ItemTypeSelectionModel[] CacheTypes { get; private set; }
        private KeyValuePair<string, string>[] databaseCacheParameters, webOutputCacheParameters;

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
                        EnsureWebOutputCacheParametersInitialized();
                        CacheParameters.AddRange(webOutputCacheParameters);
                    }
                    else
                    {
                        EnsureDatabaseCacheParametersInitialized();
                        CacheParameters.AddRange(databaseCacheParameters);
                    }

                    OnPropertyChanged("IsCacheTypeSelected");
                }

                OnPropertyChanged();
            }
        }

		public ObservableCollection<KeyValuePair<string, string>> CacheParameters { get; private set; }

        public string SelectedCacheParameter { get; set; }

        public bool IsCacheTypeSelected
        {
            get { return !string.IsNullOrEmpty(SelectedCacheType); }
        }

        public DelegateCommand ClearCacheCommand { get; private set; }

        private void EnsureWebOutputCacheParametersInitialized()
        {
            if (webOutputCacheParameters == null)
            {
                webOutputCacheParameters = new[] 
                {
                    CreateDisplayOption(Constants.ControllerNameHome),
                    CreateDisplayOption(Constants.ControllerNameAsset),
                    CreateDisplayOption(Constants.ControllerNameCatalog),
                    CreateDisplayOption(Constants.ControllerNameSearch),
                    CreateDisplayOption(Constants.ControllerNameStore),
                    CreateDisplayOption(_allCachesText)
                };
            }
        }

        private void EnsureDatabaseCacheParametersInitialized()
        {
            if (databaseCacheParameters == null)
            {
                databaseCacheParameters = new[]
                {
                    CreateDisplayOption(Constants.DisplayTemplateCachePrefix),
                    CreateDisplayOption(Constants.EmailTemplateCachePrefix),
                    CreateDisplayOption(Constants.PricelistCachePrefix),
                    CreateDisplayOption(Constants.CatalogOutlineBuilderCachePrefix),
                    CreateDisplayOption(Constants.PromotionsCachePrefix),
                    CreateDisplayOption(Constants.DynamicContentCachePrefix),
                    CreateDisplayOption(Constants.CatalogCachePrefix),
                    CreateDisplayOption(Constants.CountriesCachePrefix),
                    CreateDisplayOption(Constants.ReviewsCachePrefix),
                    CreateDisplayOption(Constants.SeoCachePrefix),
                    CreateDisplayOption(Constants.SettingsCachePrefix),
                    CreateDisplayOption(Constants.ShippingCachePrefix),
                    CreateDisplayOption(Constants.StoreCachePrefix),
                    CreateDisplayOption(Constants.UserCachePrefix),
                    CreateDisplayOption(_allCachesText)
                };
            }
        }

        private KeyValuePair<string, string> CreateDisplayOption(string parameter)
        {
            var result = new KeyValuePair<string, string>(parameter, MakeUserFrendlyText(parameter));
            //databaseCacheParameters.Add(result);
            return result;
        }

        private string MakeUserFrendlyText(string parameter)
        {
            var sb = new StringBuilder();

            parameter = parameter.Trim().Trim('_');
            for (int i = 0; i < parameter.Length; i++)
            {
                if (char.IsUpper(parameter, i) && i > 0 && char.IsLower(parameter, i - 1))
                    sb.Append(' ');

                sb.Append(parameter[i]);
            }

            return sb.ToString();
        }

        private async void DoClearCache()
        {
            ShowLoadingAnimation = true;
            ClearCacheCommand.RaiseCanExecuteChanged();
            try
            {
                var cacheParameter = SelectedCacheParameter == _allCachesText ? null : SelectedCacheParameter;
                if (CacheTypes[0].Value == SelectedCacheType)
                {
                    await Task.Run(() => _service.ClearOuputCache(cacheParameter, null));
                }
                else
                {
                    await Task.Run(() => _service.ClearDatabaseCache(cacheParameter));
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
