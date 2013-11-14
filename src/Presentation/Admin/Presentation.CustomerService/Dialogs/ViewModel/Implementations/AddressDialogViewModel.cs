using System.ComponentModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;


namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
	public class AddressDialogViewModel : ViewModelBase, IAddressDialogViewModel
	{
		#region Fields

		private Address _innerItem;

		#endregion

		#region Constructor

		public AddressDialogViewModel(Address item, object[] countries)
		{

			InnerItem = item;
			_innerItem.PropertyChanged += _innerItem_PropertyChanged;
			AllCountries = countries;

			OnPropertyChanged("AddressType");
		}
		
		#endregion
		
		#region IAddressDialogViewModel

		public Address InnerItem
		{
			get { return _innerItem; }
			set
			{
				_innerItem = value;
				OnPropertyChanged();
			}
		}

		public bool Validate()
		{
			InnerItem.Validate(false);
			var retval = InnerItem.Errors.Count == 0;
			InnerItem.Errors.Clear();
			return retval;
		}

		#endregion

		#region Address Properties
		
		public string CountryName
		{
			get { return _innerItem.CountryName; }
			set
			{
				_innerItem.CountryName = value;
				OnPropertyChanged();
				OnPropertyChanged("IsValid");
			}
		}


		public string AddressType
		{
			get 
			{
				return string.IsNullOrEmpty(_innerItem.Type) ? string.Empty : _innerItem.Type;
			}
			set
			{
				_innerItem.Type = value;
				OnPropertyChanged();
			}
		}

		#endregion

		#region Pulbic Properties
		
		private object[] _allCountries;
		public object[] AllCountries
		{
			get
			{
				return _allCountries;
			}
			set
			{
				_allCountries = value;
				OnPropertyChanged();
			}
		}

		public bool IsValid
		{
			get
			{
				return Validate();
			}
		}

		#endregion
		
		#region Handlers

		void _innerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_innerItem.PropertyChanged -= _innerItem_PropertyChanged;
			OnPropertyChanged("IsValid");
			_innerItem.PropertyChanged += _innerItem_PropertyChanged;
		}

		#endregion

	}
}
