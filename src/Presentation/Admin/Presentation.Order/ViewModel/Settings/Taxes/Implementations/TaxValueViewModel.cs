using System.ComponentModel;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Implementations
{
    public class TaxValueViewModel : ViewModelBase, ITaxValueViewModel
    {
        #region Constructor

        public TaxValueViewModel(TaxCategory[] allAvailableTaxCategories, JurisdictionGroup[] allAvailableJurisdictionGroups, TaxValue item)
        {
            AllAvailableTaxCategories = allAvailableTaxCategories;
            AllAvailableJurisdictionGroups = allAvailableJurisdictionGroups;
            InnerItem = item;
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }

        #endregion


        #region Properties

        private TaxValue _innerItem;
        public TaxValue InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(InnerItem.TaxCategory) &&
                         !string.IsNullOrEmpty(InnerItem.JurisdictionGroupId);
            }
        }

        public TaxCategory[] AllAvailableTaxCategories { get; private set; }
        public JurisdictionGroup[] AllAvailableJurisdictionGroups { get; private set; }

        #endregion

        void InnerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
            OnPropertyChanged("IsValid");
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }
    }
}
