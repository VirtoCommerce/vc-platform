using System;
using System.Linq;
using System.Windows;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
    public class OrderAddressViewModel : ViewModelBase, IOrderAddressViewModel
    {
        private readonly ICountryRepository _countryRepository;

        public object[] AllCountries
        {
            get
            {
                return _countryRepository.Countries.Expand("Regions").OrderBy(x => x.Name).ToArray();
            }
        }

        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }

        #region constructor
        public OrderAddressViewModel(OrderAddress addressItem, ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;

            this.AddressItem = addressItem;
            this.AddressItem.PropertyChanged += InnerItem_PropertyChanged;
        }

        #endregion

        #region IOrderAddressViewModel Members

        public OrderAddress AddressItem { get; private set; }

        #endregion

        public bool Validate()
        {
            bool result = AddressItem.Validate();

            // EXTERNAL VALIDATION e.g. does this postal code exist in given address
            //if (AddressItem.City == "Vilnius" && AddressItem.PostalCode != null && !AddressItem.PostalCode.StartsWith("1"))
            //    AddressItem.SetError("PostalCode", "Invalid ZIP code for this City");
            //else
            //    AddressItem.ClearError("PostalCode");

            // result = string.IsNullOrEmpty(AddressItem.Error);
            return result;
        }

        private void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {            
            Action callback = () => { OnPropertyChanged("IsValid"); };
            Application.Current.Dispatcher.BeginInvoke(callback);
        }
    }
}
