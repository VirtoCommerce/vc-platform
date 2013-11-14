using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Implementations
{
    public class ShippingOptionAddShippingPackageViewModel: ViewModelBase,IShippingOptionAddShippingPackageViewModel
    {

        #region Fields

        private readonly ICatalogRepository  _catalogRepository;
        private readonly List<string> _selectedPackaging;
        #endregion


        #region Constructor

        public ShippingOptionAddShippingPackageViewModel(
			ShippingPackage item, 
			List<string> selectedPackaging, 
			ICatalogRepository catalogRepository)
        {
            InnerItem = item;
            _selectedPackaging = selectedPackaging;
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;

            _catalogRepository = catalogRepository;
            if (_catalogRepository != null)
            {
                AllPackages = _catalogRepository.Packagings.ToList();
            }

            var view = CollectionViewSource.GetDefaultView(AllPackages);
            view.Filter = FilterItems;
            view.Refresh();

        }

        private bool FilterItems(object item)
        {
            var result = true;

            var packaging = item as Packaging;

            var existPackaging = _selectedPackaging.SingleOrDefault(p => p == packaging.PackageId);
            if (existPackaging != null)
            {
                result = false;
            }

            return result;
        }

        void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("IsValid");
        }

        #endregion


        #region IShippingOptionAddShippingPackageViewModel

        private ShippingPackage _innerItem;
        public ShippingPackage InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Properties

        private List<Packaging> _allPackages;
        public List<Packaging> AllPackages
        {
            get { return _allPackages; }
            set
            {
                _allPackages = value;
                OnPropertyChanged();
            }
        }


        public bool IsValid
        {
            get
            {
	            return !string.IsNullOrEmpty(InnerItem.ShippingOptionPackaging) &&
	                          !string.IsNullOrEmpty(InnerItem.MappedPackagingId);
            }
        }

        #endregion

        
    }
}
