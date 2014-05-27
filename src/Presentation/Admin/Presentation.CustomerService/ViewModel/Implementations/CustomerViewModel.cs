using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
	public class CustomerViewModel : ViewModelBase, ICustomerViewModel
	{
		#region Fields

		private Contact _innerContact;

		private object[] _allCountries;

		#endregion

		#region Dependencies

		private readonly ICustomerEntityFactory _entityFactory;
		private readonly IRepositoryFactory<ICustomerRepository> _customerRepositoryFactory;
		private readonly IViewModelsFactory<IAddressDialogViewModel> _addressVmFactory;

		#endregion

		#region Constructor

		public CustomerViewModel(Contact item, IRepositoryFactory<ICustomerRepository> customerRepositoryFactory, ICustomerEntityFactory customerEntityFactory, IRepositoryFactory<ICountryRepository> countryRepositoryFactory, IViewModelsFactory<IAddressDialogViewModel> addressVmFactory)
		{
			_entityFactory = customerEntityFactory;
			_customerRepositoryFactory = customerRepositoryFactory;
			_addressVmFactory = addressVmFactory;
			_innerContact = item;

			_innerContact.PropertyChanged += _innerContact_PropertyChanged;

			CommandsInit();
			RequestInit();
			CollectionInit();

			//Get All Countries
			Action getAllCountiesAction = () =>
			{
				using (var repository = countryRepositoryFactory.GetRepositoryInstance())
				{
					var countries = repository.Countries.Expand(c => c.Regions).OrderBy(c => c.Name).ToArray();
					OnUIThread(() => { _allCountries = countries; });
				}
			};
			getAllCountiesAction.BeginInvoke(null, null);
		}

		private void CommandsInit()
		{
			AddAddressCommand = new DelegateCommand(RaiseAddAddressRequest);
			EditAddressCommand = new DelegateCommand<Address>(RaiseEditAddressRequest);
			DeleteAddressCommand = new DelegateCommand(DeleteAddress);

			ShowAddEmailCommand = new DelegateCommand(ShowAddEmail);
			SaveNewEmailCommand = new DelegateCommand(SaveNewEmail, CanSaveNewEmail);
			DeleteEmailCommand = new DelegateCommand(DeleteEmail);
			CancelSaveEmailCommand = new DelegateCommand(CancelSaveEmail);

			ShowAddPhoneCommand = new DelegateCommand(ShowAddPhone);
			SaveNewPhoneCommand = new DelegateCommand(SaveNewPhone, CanSaveNewPhone);
			DeletePhoneCommand = new DelegateCommand(DeletePhone);
			CancelSavePhoneCommand = new DelegateCommand(CancelSavePhone);

			SaveNewEmailCommand.RaiseCanExecuteChanged();
			SaveNewPhoneCommand.RaiseCanExecuteChanged();
		}

		private void RequestInit()
		{
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			DisableableCommandConfirmRequest = new InteractionRequest<Confirmation>();
		}

		private void CollectionInit()
		{
			using (var repository = _customerRepositoryFactory.GetRepositoryInstance())
			{
				AllLabels = repository.Labels.ToList();
			}

			if (_innerContact != null && _innerContact.Labels != null)
			{
				ExistLabels = new ObservableCollection<Label>();
				foreach (var exLbl in _innerContact.Labels)
				{
					var labelFromDb = _customerRepositoryFactory.GetRepositoryInstance().Labels.Where(l => l.LabelId == exLbl.LabelId).SingleOrDefault();
					if (labelFromDb != null)
					{
						ExistLabels.Add(labelFromDb);
					}
				}
			}

			ExistLabels.CollectionChanged += (o, e) =>
			{
				_innerContact.Labels.Clear();
				foreach (var label in ExistLabels)
				{
					_innerContact.Labels.Add(label);
				}
			};

		}

		#endregion

		#region Commands

		public DelegateCommand ShowAddEmailCommand { get; private set; }
		public DelegateCommand ShowAddPhoneCommand { get; private set; }
		public DelegateCommand AddAddressCommand { get; private set; }
		public DelegateCommand<Address> EditAddressCommand { get; private set; }

		public DelegateCommand SaveNewPhoneCommand { get; private set; }
		public DelegateCommand SaveNewEmailCommand { get; private set; }

		public DelegateCommand DeleteEmailCommand { get; private set; }
		public DelegateCommand DeletePhoneCommand { get; private set; }
		public DelegateCommand DeleteAddressCommand { get; private set; }

		public DelegateCommand CancelSaveEmailCommand { get; private set; }
		public DelegateCommand CancelSavePhoneCommand { get; private set; }

		#endregion

		#region Requests

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		public InteractionRequest<Confirmation> DisableableCommandConfirmRequest { get; private set; }

		#endregion

		#region Commands Implementation

		/// <summary>
		/// raised on add address click
		/// </summary>
		private void RaiseAddAddressRequest()
		{
			var item = new Address() { Type = "Primary" };
			if (RaiseEditAddressRequest(item, "Add Address".Localize()))
			{
				OnUIThread(() => InnerItem.Addresses.Add(item));
			}
		}

		/// <summary>
		/// raised on address edit click
		/// </summary>
		private void RaiseEditAddressRequest(Address originalItem)
		{
			var item = originalItem.DeepClone(_entityFactory as IKnownSerializationTypes);
			if (RaiseEditAddressRequest(item, "Edit Address".Localize()))
			{
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
			}
		}

		private bool RaiseEditAddressRequest(Address item, string title)
		{
			var result = false;
			var itemVM = _addressVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item), new KeyValuePair<string, object>("countries", _allCountries));
			DisableableCommandConfirmRequest.Raise(
				new ConditionalConfirmation(itemVM.Validate)
				{
					Title = title,
					Content = itemVM
				},
				(x) =>
				{
					result = x.Confirmed;
				});

			return result;
		}

		private void ShowAddEmail()
		{
			if (IsEmailInEditMode)
			{
				if (SaveNewEmailCommand.CanExecute())
				{
					SaveNewEmailCommand.Execute();
					IsEmailInEditMode = false;
				}
			}
			else
			{
				NewEmail = new Email { Type = "Primary" };
				IsEmailInEditMode = true;
			}
		}

		private void ShowAddPhone()
		{

			if (!IsPhoneInEditMode)
			{
				IsPhoneInEditMode = true;
				NewPhone = new Phone { Type = "Primary" };
			}
			else
			{
				if (SaveNewPhoneCommand.CanExecute())
				{
					SaveNewPhoneCommand.Execute();
					IsPhoneInEditMode = false;
				}
			}
		}

		private void SaveNewPhone()
		{
			InnerItem.Phones.Add(NewPhone);
			IsPhoneInEditMode = false;
		}

		private bool CanSaveNewPhone()
		{
			return NewPhoneValidate();
		}

		private void SaveNewEmail()
		{
			NewEmail.MemberId = InnerItem.MemberId;
			InnerItem.Emails.Add(NewEmail);
			IsEmailInEditMode = false;
		}


		private bool CanSaveNewEmail()
		{
			return NewEmailValidate();
		}


		private void DeleteEmail()
		{
			if (SelectedEmail != null)
			{
				CommonConfirmRequest.Raise(
				new ConditionalConfirmation
				{
					Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory),
					Content = "Are you sure you want to delete the email?".Localize()
				},
				(x) =>
				{
					if (x.Confirmed)
					{
						InnerItem.Emails.Remove(SelectedEmail);
					}
				});
			}
		}

		private void DeletePhone()
		{
			if (SelectedPhone != null)
			{
				CommonConfirmRequest.Raise(
				new ConditionalConfirmation
				{
					Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory),
					Content = "Are you sure you want to delete the phone number?".Localize()
				},
				(x) =>
				{
					if (x.Confirmed)
					{
						var phoneForDelete = SelectedPhone;
						InnerItem.Phones.Remove(phoneForDelete);
					}
				});
			}
		}

		private void DeleteAddress()
		{
			if (SelectedAddress != null)
			{
				CommonConfirmRequest.Raise(
					new ConditionalConfirmation
					{
						Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory),
						Content = "Are you sure you want to delete the address?".Localize()
					},
					(x) =>
					{
						if (x.Confirmed)
						{
							var addressToDelete = SelectedAddress;
							InnerItem.Addresses.Remove(addressToDelete);
							_customerRepositoryFactory.GetRepositoryInstance().Remove(addressToDelete);
						}
					});
			}
		}



		private void CancelSaveEmail()
		{
			IsEmailInEditMode = false;
		}

		private void CancelSavePhone()
		{
			IsPhoneInEditMode = false;
		}

		#endregion

		#region Contact Properties

		public Contact InnerItem
		{
			get { return _innerContact ?? (_innerContact = new Contact()); }
		}


		public string CustomerRegion
		{
			get
			{
				string result;
				if (InnerItem.Addresses.Count > 0)
				{
					result = !string.IsNullOrEmpty(InnerItem.Addresses[0].RegionName) ? InnerItem.Addresses[0].RegionName : InnerItem.Addresses[0].CountryName;
				}
				else
					result = null;

				return result;
			}
		}

		#endregion

		#region Public Properties

		private Email _selectedEmail;
		public Email SelectedEmail
		{
			get { return _selectedEmail; }
			set
			{
				_selectedEmail = value;
				OnPropertyChanged();
			}
		}


		private Phone _selectedPhone;
		public Phone SelectedPhone
		{
			get { return _selectedPhone; }
			set
			{
				_selectedPhone = value;
				OnPropertyChanged();
			}
		}


		private Address _selectedAddress;
		public Address SelectedAddress
		{
			get { return _selectedAddress; }
			set
			{
				_selectedAddress = value;
				OnPropertyChanged();
			}
		}

		public List<Label> AllLabels { get; private set; }

		private ObservableCollection<Label> _existLabels = new ObservableCollection<Label>();
		public ObservableCollection<Label> ExistLabels
		{
			get { return _existLabels; }
			set
			{
				_existLabels = value;
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

		public bool IsNewPhoneValid
		{
			get
			{
				return NewPhoneValidate();
			}
		}

		public bool IsNewEmailValid
		{
			get
			{
				return NewEmailValidate();
			}
		}

		#endregion

		#region Properties for UserInput

		private bool _isEmailInEditMode;
		public bool IsEmailInEditMode
		{
			get { return _isEmailInEditMode; }
			set
			{
				_isEmailInEditMode = value;
				OnPropertyChanged();
			}
		}

		private bool _isPhoneInEditMode;
		public bool IsPhoneInEditMode
		{
			get { return _isPhoneInEditMode; }
			set
			{
				_isPhoneInEditMode = value;
				OnPropertyChanged();
			}
		}

		private Phone _newPhone = new Phone();
		public Phone NewPhone
		{
			get { return _newPhone; }
			set
			{
				_newPhone = value;
				OnPropertyChanged();
			}
		}

		private Email _newEmail = new Email();
		public Email NewEmail
		{
			get { return _newEmail; }
			set
			{
				_newEmail = value;
				OnPropertyChanged();
			}
		}

		#endregion

		#region Validation

		private bool Validate()
		{
			return _innerContact.Validate();
		}

		private bool NewPhoneValidate()
		{
			return NewPhone.Validate();
		}

		private bool NewEmailValidate()
		{
			return NewEmail.Validate();
		}

		#endregion

		#region Handlers

		void _innerContact_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_innerContact.PropertyChanged -= _innerContact_PropertyChanged;
			OnPropertyChanged("IsValid");
			_innerContact.PropertyChanged += _innerContact_PropertyChanged;
		}

		#endregion
	}
}
