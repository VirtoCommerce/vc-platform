using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
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
                    new ItemTypeSelectionModel("Html output", "This is where rendered html is stored."),
                    new ItemTypeSelectionModel("Database","This is where queried objects are stored.")
                };

            CacheParameters = new ObservableCollection<string>();
            ClearCacheCommand = new DelegateCommand(DoClearCache, () => !string.IsNullOrEmpty(SelectedCacheType) && !string.IsNullOrEmpty(SelectedCacheParameter));
        }

        private string _selectedCacheType;
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
                        CacheParameters.Add("All caches");
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

        private void DoClearCache()
        {
            if (CacheTypes[0].Value == SelectedCacheType)
            {
                _service.ClearOuputCache(SelectedCacheParameter, null);
            }
            else
            {
                _service.ClearDatabaseCache(SelectedCacheParameter);
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
