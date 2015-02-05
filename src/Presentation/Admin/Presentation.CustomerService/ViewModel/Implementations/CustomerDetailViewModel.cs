using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications.Services;
using System.Waf.VirtoCommerce.ManagementClient.Services;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
	public class CustomerDetailViewModel : ViewModelBase
	{

		#region Fields

		private Contact _innerItem;
		private string _currentContactStoreId;

		#endregion

		#region Dependencies

		private readonly CustomersDetailViewModel _parentViewModel;
		private readonly IAuthenticationContext _authContext;
		private readonly ICustomerRepository _customerRepository;
		private readonly ICustomerEntityFactory _entityFactory;
		private readonly IFileDialogService _fileDialogService;

		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
		private readonly IRepositoryFactory<ISecurityRepository> _securityRepositoryFactory;
		private readonly IRepositoryFactory<ICountryRepository> _countryRepositoryFactory;
		private readonly IViewModelsFactory<IAddressDialogViewModel> _addressVmFactory;
		private readonly IViewModelsFactory<ICreateUserDialogViewModel> _wizardUserVmFactory;
		private readonly IViewModelsFactory<IEmailDialogViewModel> _emailVmFactory;
		private readonly IViewModelsFactory<IPhoneNumberDialogViewModel> _phoneVmFactory;
		private readonly ILoginViewModel _loginViewModel;
		private readonly ISecurityService _securityService;

		#endregion

		#region Constructor

		public CustomerDetailViewModel(ISecurityService securityService, IViewModelsFactory<IPhoneNumberDialogViewModel> phoneVmFactory, IViewModelsFactory<IEmailDialogViewModel> emailVmFactory, IViewModelsFactory<ICreateUserDialogViewModel> wizardUserVmFactory, IViewModelsFactory<IAddressDialogViewModel> addressVmFactory, ICustomerEntityFactory entityFactory,
			IAuthenticationContext authContext, CustomersDetailViewModel parentViewModel, Contact innerContact,
			ICustomerRepository customerRepository, IRepositoryFactory<ISecurityRepository> securityRepositoryFactory,
			IRepositoryFactory<ICountryRepository> countryRepositoryFactory, IRepositoryFactory<IOrderRepository> orderRepositoryFactory, ILoginViewModel loginViewModel)
		{
			_securityService = securityService;
			_loginViewModel = loginViewModel;
			_parentViewModel = parentViewModel;
			_authContext = authContext;
			_customerRepository = customerRepository;
			_orderRepositoryFactory = orderRepositoryFactory;
			_securityRepositoryFactory = securityRepositoryFactory;
			_countryRepositoryFactory = countryRepositoryFactory;
			_entityFactory = entityFactory;
			_addressVmFactory = addressVmFactory;
			_wizardUserVmFactory = wizardUserVmFactory;
			_emailVmFactory = emailVmFactory;
			_phoneVmFactory = phoneVmFactory;
			_fileDialogService = new FileDialogService();

			InnerItem = innerContact;

			InnerItem.PropertyChanged += _innerContact_PropertyChanged;

			CommandsInit();
			RequestInit();
			CollectionInit();

			HasCurrentContactLoginAndSuspendAccessCheck();
		}

		private void CommandsInit()
		{
			RemoveEmailCommand = new DelegateCommand<Email>(RemoveEmail, (e) => SelectedEmail != null && SelectedEmail.Type != EmailType.Primary.ToString());
			RemovePhoneCommand = new DelegateCommand<Phone>(RemovePhone, (p) => SelectedPhone != null);
			MakePrimaryEmailCommand = new DelegateCommand<Email>(MakePrimaryEmail, (e) => SelectedEmail != null && SelectedEmail.Type != EmailType.Primary.ToString());

			RemoveAddressCommand = new DelegateCommand<Address>(RemoveAddress);
			MakeBillingAddressCommand = new DelegateCommand<Address>(MakeBillingAddress);
			MakePrimaryAddressCommand = new DelegateCommand<Address>(MakePrimaryAddress);
			MakeShippingAddressCommand = new DelegateCommand<Address>(MakeShippingAddress);

			AddAddressCommand = new DelegateCommand(AddAddress);
			AddEmailCommand = new DelegateCommand(AddEmail);
			AddPhoneCommand = new DelegateCommand(AddPhone, () => ContactPhones.Count == 0);

			EditAddressCommand = new DelegateCommand<Address>(EditAddress, (a) => SelectedAddress != null);
			EditEmailCommand = new DelegateCommand<Email>(EditEmail, (e) => SelectedEmail != null && SelectedEmail.Type != EmailType.Primary.ToString());
			EditPhoneCommand = new DelegateCommand<Phone>(EditPhone, (p) => SelectedPhone != null);

			CreateLoginPasswordCommand = new DelegateCommand(CreateLoginPassword, () => _authContext.CheckPermission(PredefinedPermissions.CustomersCreateResetPasswords));
			ResetCustomerPasswordCommand = new DelegateCommand(ResetCustomerPassword, () => _authContext.CheckPermission(PredefinedPermissions.CustomersCreateResetPasswords));
			LoginOnBehalfCommand = new DelegateCommand(LoginOnBehalf, () => IsLoginOnBehalfShow);
			SuspendAccessCommand = new DelegateCommand(SuspendAccess, () => _authContext.CheckPermission(PredefinedPermissions.CustomersSuspendAccounts));
			RestoreAccessCommand = new DelegateCommand(RestoreAccess, () => _authContext.CheckPermission(PredefinedPermissions.CustomersSuspendAccounts));

			RefreshCustomerOrdersCommand = new DelegateCommand(RefreshCustomerOrders, () => ShowLoadingAnimation == false);

			GoToSelectedOrderCommand = new DelegateCommand(GoToSelectedOrder, () => SelectedOrder != null);
			ChangeContactImageCommand = new DelegateCommand(ChangeContactImage, () => _authContext.CheckPermission(PredefinedPermissions.CustomersEditCustomer));

			AddAddressPhoneEmailInteractioNRequest = new InteractionRequest<ConditionalConfirmation>();
			SelectStoreRequest = new InteractionRequest<ConditionalConfirmation>();
			NotificationInteractionRequest = new InteractionRequest<ConditionalConfirmation>();
		}

		private void RequestInit()
		{
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		private async void CollectionInit()
		{
			if (InnerItem != null && InnerItem.Labels != null)
			{
				ContactLabels = new ObservableCollection<Label>(InnerItem.Labels);
			}

			foreach (var contactLabel in ContactLabels)
			{
				ContactLabelsFromTagControl.Add(new TagControlItemViewModel(contactLabel) { IsEditing = false });
			}
			ContactLabelsFromTagControl.Add(new TagControlItemViewModel(new Label()) { IsEditing = true });

			ContactLabelsFromTagControl.CollectionChanged += (s, e) =>
				{
					_parentViewModel.IsModified = true;
					ContactLabels =
						new ObservableCollection<Label>(
							ContactLabelsFromTagControl.Where(i => i.IsEditing == false)
													   .Select(i => i.InnerItem as Label).ToList());
				};


			if (InnerItem != null && InnerItem.Emails != null)
			{
				ContactEmails = new ObservableCollection<Email>(InnerItem.Emails);
			}

			ContactEmails.CollectionChanged += (s, e) =>
				{
					_parentViewModel.IsModified = true;
				};


			if (InnerItem != null && InnerItem.Addresses != null)
			{
				ContactAddresses = new ObservableCollection<Address>(InnerItem.Addresses);
			}

			ContactAddresses.CollectionChanged += (s, e) =>
				{
					_parentViewModel.IsModified = true;
				};


			if (InnerItem != null && InnerItem.Phones != null)
			{
				ContactPhones = new ObservableCollection<Phone>(InnerItem.Phones);
			}

			ContactPhones.CollectionChanged += (s, e) =>
				{
					_parentViewModel.IsModified = true;
				};


			RefreshCustomerOrders();

			foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
			{
				CultureList.Add(culture);
			}

			if (string.IsNullOrEmpty(InnerItem.DefaultLanguage))
			{
				var lang =
					CultureList.SingleOrDefault(
						c => c.DisplayName.Contains("English") && c.DisplayName.Contains("United States"));
				if (lang != null)
				{
					DefaultSelectedLanguage = lang;
				}
			}
			else
			{
				var lang = CultureList.SingleOrDefault(c => c.DisplayName == InnerItem.DefaultLanguage);
				if (lang != null)
				{
					DefaultSelectedLanguage = lang;
				}
			}


			var cultureView = CollectionViewSource.GetDefaultView(CultureList);
			cultureView.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
			cultureView.Refresh();


			TimeZones = TimeZoneInfo.GetSystemTimeZones();
			var view = CollectionViewSource.GetDefaultView(TimeZones);
			view.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
			view.Refresh();


			OnPropertyChanged("TimeZones");
			OnPropertyChanged("SelectedTimeZone");


			ContactCases = await Task<List<Case>>.Run(() =>
				{

					var result = new List<Case>();

					result = _customerRepository.Cases.Where(c => c.ContactId == InnerItem.MemberId).ToList();

					return result;
				});
		}

		private async void HasCurrentContactLoginAndSuspendAccessCheck()
		{
			CurrentContactHasLogin = await Task.Run(() =>
			{
				var user =
					SecurityRepository.Accounts.Where(a => a.MemberId == InnerItem.MemberId).SingleOrDefault();

				if (user != null)
				{
					CurrentContactLoginUserName = user.UserName;
					_currentContactStoreId = user.StoreId;
					return true;
				}
				return false;
			});

			var hasSuspendAccess = await Task.Run(() =>
			{

				var user = SecurityRepository.Accounts.Where(a => a.MemberId == InnerItem.MemberId).SingleOrDefault();
				if (user != null)
				{
					if (user.AccountState == (int)AccountState.Rejected)
					{
						return true;
					}
				}
				return false;

			});


			if (!CurrentContactHasLogin)
			{
				ContactAccessState = ContactAccessState.None;
			}
			else
			{
				if (hasSuspendAccess && CurrentContactHasLogin)
				{
					ContactAccessState = ContactAccessState.Suspended;
				}
				else
				{
					ContactAccessState = ContactAccessState.Restored;
				}
			}

		}

		#endregion

		#region Commands

		public DelegateCommand<Phone> RemovePhoneCommand { get; private set; }

		public DelegateCommand<Email> RemoveEmailCommand { get; private set; }
		public DelegateCommand<Email> MakePrimaryEmailCommand { get; private set; }

		public DelegateCommand<Address> RemoveAddressCommand { get; private set; }
		public DelegateCommand<Address> MakePrimaryAddressCommand { get; private set; }
		public DelegateCommand<Address> MakeShippingAddressCommand { get; private set; }
		public DelegateCommand<Address> MakeBillingAddressCommand { get; private set; }

		public DelegateCommand AddAddressCommand { get; private set; }
		public DelegateCommand AddPhoneCommand { get; private set; }
		public DelegateCommand AddEmailCommand { get; private set; }

		public DelegateCommand<Address> EditAddressCommand { get; private set; }
		public DelegateCommand<Phone> EditPhoneCommand { get; private set; }
		public DelegateCommand<Email> EditEmailCommand { get; private set; }


		public DelegateCommand CreateLoginPasswordCommand { get; private set; }
		public DelegateCommand ResetCustomerPasswordCommand { get; private set; }
		public DelegateCommand LoginOnBehalfCommand { get; private set; }
		public DelegateCommand SuspendAccessCommand { get; private set; }
		public DelegateCommand RestoreAccessCommand { get; private set; }

		public DelegateCommand RefreshCustomerOrdersCommand { get; private set; }
		public DelegateCommand GoToSelectedOrderCommand { get; private set; }
		public DelegateCommand ChangeContactImageCommand { get; private set; }


		public InteractionRequest<ConditionalConfirmation> AddAddressPhoneEmailInteractioNRequest { get; private set; }
		public InteractionRequest<ConditionalConfirmation> NotificationInteractionRequest { get; private set; }

		#endregion

		#region Requests

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		public InteractionRequest<ConditionalConfirmation> SelectStoreRequest { get; private set; }

		#endregion

		#region Commands Implementation

		private void RemovePhone(Phone item)
		{
			if (ContactPhones != null)
			{
				ContactPhones.Remove(item);
				AddPhoneCommand.RaiseCanExecuteChanged();
			}
		}

		private void RemoveEmail(Email item)
		{
			if (ContactEmails != null)
			{
				ContactEmails.Remove(item);
			}
		}

		private void MakePrimaryEmail(Email item)
		{
			if (ContactEmails != null)
			{
				foreach (var emailItem in ContactEmails)
				{
					emailItem.Type = null;
				}

				item.Type = EmailType.Primary.ToString();
				_parentViewModel.IsModified = true;
			}
		}

		private void RemoveAddress(Address item)
		{
			if (ContactAddresses != null)
			{
				ContactAddresses.Remove(item);
			}
		}

		private void MakePrimaryAddress(Address item)
		{
			foreach (var addressItem in ContactAddresses.Where(addressItem => addressItem.Type == AddressType.Primary.ToString()))
			{
				addressItem.Type = null;
			}

			item.Type = AddressType.Primary.ToString();
			_parentViewModel.IsModified = true;
		}

		private void MakeShippingAddress(Address item)
		{
			foreach (var addressItem in ContactAddresses.Where(addressItem => addressItem.Type == AddressType.Shipping.ToString()))
			{
				addressItem.Type = null;
			}

			item.Type = AddressType.Shipping.ToString();
			_parentViewModel.IsModified = true;
		}

		private void MakeBillingAddress(Address item)
		{
			foreach (var addressItem in ContactAddresses.Where(addressItem => addressItem.Type == AddressType.Billing.ToString()))
			{
				addressItem.Type = null;
			}

			item.Type = AddressType.Billing.ToString();
			_parentViewModel.IsModified = true;
		}

		private void AddAddress()
		{
			if (AddAddressPhoneEmailInteractioNRequest != null)
			{
				var countries = CountryRepository.Countries.Expand(c => c.Regions).ToArray();

				var parameters = new List<KeyValuePair<string, object>>
					{
						new KeyValuePair<string, object>("item", new Address()),
						new KeyValuePair<string, object>("countries", countries)
					};

				var itemVm = _addressVmFactory.GetViewModelInstance(parameters.ToArray());

				var confirmation = new ConditionalConfirmation { Title = "Enter address details".Localize(), Content = itemVm };

				AddAddressPhoneEmailInteractioNRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var itemToAdd =
								(x.Content as IAddressDialogViewModel)
									.InnerItem;
							itemToAdd.MemberId = InnerItem.MemberId;

							ContactAddresses.Add(itemToAdd);
						}
					});
			}
		}

		private void AddPhone()
		{
			if (AddAddressPhoneEmailInteractioNRequest != null)
			{
				var itemVm = _phoneVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", new Phone()));

				var confirmation = new ConditionalConfirmation { Title = "Enter phone details".Localize(), Content = itemVm };

				AddAddressPhoneEmailInteractioNRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var phoneToAdd =
								(x.Content as IPhoneNumberDialogViewModel)
									.InnerItem;

							ContactPhones.Add(phoneToAdd);
							AddPhoneCommand.RaiseCanExecuteChanged();
						}
					});
			}
		}

		private void AddEmail()
		{
			if (AddAddressPhoneEmailInteractioNRequest != null)
			{
				var itemVm = _emailVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", new Email()));

				var confirmation = new ConditionalConfirmation { Title = "Enter email details".Localize(), Content = itemVm };

				AddAddressPhoneEmailInteractioNRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var itemToAdd =
								(x.Content as IEmailDialogViewModel)
									.InnerItem;
							itemToAdd.MemberId = InnerItem.MemberId;

							if (ContactEmails.Count == 0 || ContactEmails.All(e => e.Type != EmailType.Primary.ToString()))
							{
								itemToAdd.Type =
									EmailType.Primary.ToString();
							}

							ContactEmails.Add(itemToAdd);
						}
					});
			}
		}

		private void EditAddress(Address item)
		{
			if (AddAddressPhoneEmailInteractioNRequest != null)
			{
				var addressToEdit = item.DeepClone(_entityFactory as IKnownSerializationTypes);
				var countries = CountryRepository.Countries.Expand(c => c.Regions).ToArray();

				var parameters = new List<KeyValuePair<string, object>>
					{
						new KeyValuePair<string, object>("item", addressToEdit),
						new KeyValuePair<string, object>("countries", countries)
					};

				var itemVm = _addressVmFactory.GetViewModelInstance(parameters.ToArray());

				var confirmation = new ConditionalConfirmation { Title = "Enter address details".Localize(), Content = itemVm };

				AddAddressPhoneEmailInteractioNRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var itemToUpdate =
								(x.Content as IAddressDialogViewModel).InnerItem;

							var itemFromInnerItem =
								ContactAddresses.SingleOrDefault(
									a => a.AddressId == itemToUpdate.AddressId);

							if (itemFromInnerItem != null)
							{
								itemFromInnerItem.InjectFrom(itemToUpdate);
								_parentViewModel.IsModified = true;
							}

						}
					});
			}
		}

		private void EditPhone(Phone item)
		{
			if (AddAddressPhoneEmailInteractioNRequest != null)
			{
				var phoneToEdit = item.DeepClone(_entityFactory as IKnownSerializationTypes);

				var itemVm = _phoneVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", phoneToEdit));

				var confirmation = new ConditionalConfirmation { Title = "Enter phone details".Localize(), Content = itemVm };

				AddAddressPhoneEmailInteractioNRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var phoneToUpdate =
								(x.Content as IPhoneNumberDialogViewModel)
									.InnerItem;

							var itemFromInnerItem =
								ContactPhones.SingleOrDefault(
									p => p.PhoneId == phoneToUpdate.PhoneId);

							if (itemFromInnerItem != null)
							{
								itemFromInnerItem.InjectFrom(phoneToUpdate);
								_parentViewModel.IsModified = true;
							}

						}
					});


			}
		}

		private void EditEmail(Email item)
		{
			if (AddAddressPhoneEmailInteractioNRequest != null)
			{
				var emailToEdit = item.DeepClone(_entityFactory as IKnownSerializationTypes);

				var itemVm = _emailVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", emailToEdit));

				var confirmation = new ConditionalConfirmation { Title = "Enter email details".Localize(), Content = itemVm };

				AddAddressPhoneEmailInteractioNRequest.Raise(confirmation,
					(x) =>
					{
						if (x.Confirmed)
						{
							var itemTUpdate =
								(x.Content as IEmailDialogViewModel)
									.InnerItem;

							var itemFromInnerItem =
								ContactEmails.SingleOrDefault(
									e => e.EmailId == itemTUpdate.EmailId);

							if (itemFromInnerItem != null)
							{
								itemFromInnerItem.InjectFrom(itemTUpdate);
								_parentViewModel.IsModified = true;
							}
						}
					});
			}
		}

		private void CreateLoginPassword()
		{
			if (SelectStoreRequest != null)
			{
				var itemVm =
					_wizardUserVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("operationType",
						CreateUserDialogOperationType.CreateLogin));

				var confirmation = new ConditionalConfirmation { Title = "Enter login details".Localize(), Content = itemVm };

				SelectStoreRequest.Raise(confirmation,
					async (x) =>
					{
						if (x.Confirmed)
						{
							var storeId = (x.Content as ICreateUserDialogViewModel).StoreId;
							var userName = (x.Content as ICreateUserDialogViewModel).UserName;
							var password = (x.Content as ICreateUserDialogViewModel).Password;

							try
							{
								await _securityService.CreateUserAsync(InnerItem.MemberId, userName, password, storeId);
								HasCurrentContactLoginAndSuspendAccessCheck();
							}
							catch (Exception ex)
							{
								ShowErrorDialog(ex,
									string.Format("An error occurred when trying to create the login: {0}", ex.InnerException.Message));
							}
						}
					});
			}

		}

		private void ResetCustomerPassword()
		{
			if (SelectStoreRequest != null)
			{

				var itemVm =
					_wizardUserVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("operationType",
						CreateUserDialogOperationType
							.ResetPassword));

				var confirmation = new ConditionalConfirmation { Title = "Enter new password".Localize(), Content = itemVm };

				SelectStoreRequest.Raise(confirmation,
					async (x) =>
					{
						if (x.Confirmed)
						{
							var newPass = (x.Content as ICreateUserDialogViewModel).Password;

							try
							{
								await _securityService.ResetPasswordAsync(CurrentContactLoginUserName, newPass);
								HasCurrentContactLoginAndSuspendAccessCheck();
							}
							catch (Exception ex)
							{
								ShowErrorDialog(ex,
									string.Format("An error occurred when trying to reset the password: {0}", ex.InnerException.Message));
							}
						}
					});

			}
		}

		private void LoginOnBehalf()
		{
			var url = string.Format("{0}?loginAs={1}&store={2}", _loginViewModel.CurrentUser.BaseUrl, CurrentContactLoginUserName, _currentContactStoreId);
			// open the browser
			Process.Start(url);
		}

		private void SuspendAccess()
		{
			if (CommonConfirmRequest != null)
			{
				var confirmation = new ConditionalConfirmation()
					{
						Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory),
						Content = string.Format("Suspend access for user '{0}'?".Localize(), InnerItem.FullName)
					};

				CommonConfirmRequest.Raise(confirmation,
					async (x) =>
					{
						if (x.Confirmed)
						{
							try
							{
								await Task.Run(() => _securityService.Lock(InnerItem.MemberId));
								HasCurrentContactLoginAndSuspendAccessCheck();
							}
							catch (Exception ex)
							{
								ShowErrorDialog(ex, string.Format("An error occurred when trying to suspend access: {0}", ex.InnerException.Message));
							}
						}
					});
			}

		}

		private void RestoreAccess()
		{
			if (CommonConfirmRequest != null)
			{
				var confirmation = new ConditionalConfirmation()
				{
					Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory),
					Content = string.Format("Restore access for user '{0}'?".Localize(), InnerItem.FullName)
				};

				CommonConfirmRequest.Raise(confirmation,
					async (x) =>
					{
						if (x.Confirmed)
						{
							try
							{
								await Task.Run(() => _securityService.Unlock(InnerItem.MemberId));
								HasCurrentContactLoginAndSuspendAccessCheck();
							}
							catch (Exception ex)
							{
								ShowErrorDialog(ex, string.Format("An error occurred when trying to restore access: {0}".Localize(), ex.InnerException.Message));
							}
						}
					});
			}
		}

		private async void RefreshCustomerOrders()
		{

			ShowLoadingAnimation = true;
			RefreshCustomerOrdersCommand.RaiseCanExecuteChanged();
			if (ContactOrders != null)
			{
				ContactOrders.Clear();
			}

			ContactOrders =
				await
				Task<List<Order>>.Run(
					() =>
						 OrderRepository.Orders.Where(o => o.CustomerId == InnerItem.MemberId).ToList());
			ShowLoadingAnimation = false;
			RefreshCustomerOrdersCommand.RaiseCanExecuteChanged();

		}

		private void GoToSelectedOrder()
		{
			try
			{
				if (SelectedOrder != null)
				{
					string orderId = SelectedOrder.OrderGroupId;
					var mes = new GoToOrderEvent() { OrderId = orderId };
					EventSystem.Publish<GoToOrderEvent>(mes);
				}
			}
			catch (Exception)
			{
			}
		}

		private void ChangeContactImage()
		{
			IEnumerable<System.Waf.Applications.Services.FileType> fileTypes = new System.Waf.Applications.Services.FileType[]
			{
				new System.Waf.Applications.Services.FileType("jpg image", ".jpg"),
				new System.Waf.Applications.Services.FileType("bmp image", ".bmp"),
				new System.Waf.Applications.Services.FileType("png image", ".png")
			};

			FileDialogResult result = _fileDialogService.ShowOpenFileDialog(this, fileTypes);
			if (result.IsValid)
			{
				try
				{
					string fileName = result.FileName;

					if (fileName == null || !IsImageFileValid(fileName))
						return;

					byte[] buff = null;
					buff = File.ReadAllBytes(fileName);

					if (buff.Length > 20000)
					{
						NotificationInteractionRequest.Raise(new ConditionalConfirmation()
						{
							Title = "Error file selection".Localize(),
							Content = "Image must be less than 20 kilobytes".Localize()
						},
							x =>
							{

							});

					}
					else
					{
						InnerItem.Photo = buff;
					}

				}
				catch (Exception)
				{

				}
			}
		}

		private bool IsImageFileValid(string fileName)
		{
			string[] extensions = { ".jpg", ".png", ".bmp" };

			string extension = Path.GetExtension(fileName);

			bool result = false;

			if (extensions.Contains(extension))
			{
				result = true;
			}

			return result;
		}

		#endregion

		#region Contact Properties

		public Contact InnerItem
		{
			get
			{
				if (_innerItem == null)
					_innerItem = new Contact();

				return _innerItem;
			}
			private set
			{
				_innerItem = value;
				OnPropertyChanged();
			}
		}

		public string CustomerRegion
		{
			get
			{
				string result;
				if (InnerItem.Addresses.Count > 0)
				{
					if (!string.IsNullOrEmpty(InnerItem.Addresses[0].RegionName))
						result = InnerItem.Addresses[0].RegionName;
					else
						result = InnerItem.Addresses[0].CountryName;
				}
				else
					result = null;

				return result;
			}
		}

		#endregion

		#region Public Properties

		private List<Order> _contactOrders;
		public List<Order> ContactOrders
		{
			get { return _contactOrders; }
			set
			{
				_contactOrders = value;
				OnPropertyChanged();
			}
		}

		private Order _selectedOrder;
		public Order SelectedOrder
		{
			get { return _selectedOrder; }
			set
			{
				_selectedOrder = value;
				OnPropertyChanged();
				GoToSelectedOrderCommand.RaiseCanExecuteChanged();
			}
		}

		private List<CultureInfo> _culturesList = new List<CultureInfo>();
		public List<CultureInfo> CultureList
		{
			get { return _culturesList; }
			set
			{
				_culturesList = value;
				OnPropertyChanged();
			}
		}

		private CultureInfo _defaultSelectedLanguage;
		public CultureInfo DefaultSelectedLanguage
		{
			get
			{
				return _defaultSelectedLanguage;
			}
			set
			{
				_defaultSelectedLanguage = value;
				OnPropertyChanged();
				if (value != null)
				{
					InnerItem.DefaultLanguage = value.DisplayName;
				}
			}
		}

		public ReadOnlyCollection<TimeZoneInfo> TimeZones { get; set; }

		private TimeZoneInfo _selectedTimeZone;
		public TimeZoneInfo SelectedTimeZone
		{
			get
			{
				if (string.IsNullOrEmpty(InnerItem.TimeZone))
				{
					_selectedTimeZone = TimeZoneInfo.Local;
				}
				else
				{
					if (TimeZones != null)
					{
						var tz = TimeZones.SingleOrDefault(t => t.DisplayName.Contains(InnerItem.TimeZone));
						if (tz != null)
						{
							_selectedTimeZone = tz;
						}
					}
					else
					{
						_selectedTimeZone = TimeZoneInfo.Local;
					}
				}

				return _selectedTimeZone;
			}
			set
			{
				_selectedTimeZone = value;
				OnPropertyChanged();
				if (value != null)
				{
					InnerItem.TimeZone = value.DisplayName;
				}
			}
		}



		private Email _selectedEmail;
		public Email SelectedEmail
		{
			get { return _selectedEmail; }
			set
			{
				if (value == null)
					return;

				_selectedEmail = value;
				OnPropertyChanged();
				EditEmailCommand.RaiseCanExecuteChanged();
				RemoveEmailCommand.RaiseCanExecuteChanged();
				MakePrimaryEmailCommand.RaiseCanExecuteChanged();
			}
		}

		private Address _selectedAddress;
		public Address SelectedAddress
		{
			get { return _selectedAddress; }
			set
			{
				if (value == null)
					return;

				_selectedAddress = value;
				OnPropertyChanged();
				EditAddressCommand.RaiseCanExecuteChanged();
				RemoveAddressCommand.RaiseCanExecuteChanged();
				MakeBillingAddressCommand.RaiseCanExecuteChanged();
				MakePrimaryAddressCommand.RaiseCanExecuteChanged();
				MakeShippingAddressCommand.RaiseCanExecuteChanged();
			}
		}

		private Phone _selectedPhone;
		public Phone SelectedPhone
		{
			get { return _selectedPhone; }
			set
			{
				if (value == null)
					return;

				_selectedPhone = value;
				OnPropertyChanged();
				EditPhoneCommand.RaiseCanExecuteChanged();
				RemovePhoneCommand.RaiseCanExecuteChanged();
			}
		}

		private ObservableCollection<Email> _contactEmails;
		public ObservableCollection<Email> ContactEmails
		{
			get { return _contactEmails; }
			set
			{
				_contactEmails = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Phone> _contactPhones;
		public ObservableCollection<Phone> ContactPhones
		{
			get { return _contactPhones; }
			set
			{
				_contactPhones = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Label> _contactLabels;
		public ObservableCollection<Label> ContactLabels
		{
			get { return _contactLabels; }
			set
			{
				_contactLabels = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<TagControlItemViewModel> _contactLabelsFromTagControl =
			new ObservableCollection<TagControlItemViewModel>();
		public ObservableCollection<TagControlItemViewModel> ContactLabelsFromTagControl
		{
			get { return _contactLabelsFromTagControl; }
			set
			{
				_contactLabelsFromTagControl = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Address> _contactAddresses;
		public ObservableCollection<Address> ContactAddresses
		{
			get { return _contactAddresses; }
			set
			{
				_contactAddresses = value;
				OnPropertyChanged();
			}
		}

		private List<Case> _contactCases;
		public List<Case> ContactCases
		{
			get { return _contactCases; }
			set
			{
				_contactCases = value;
				OnPropertyChanged();
			}
		}

		public bool IsCreateLoginPasswordShow
		{
			get { return _authContext.CheckPermission(PredefinedPermissions.CustomersCreateResetPasswords) && !CurrentContactHasLogin; }
		}

		public bool IsLoginOnBehalfShow
		{
			get { return _authContext.CheckPermission(PredefinedPermissions.CustomersLoginAsCustomer) && CurrentContactHasLogin; }
		}

		public bool IsResetPasswordShow
		{
			get { return _authContext.CheckPermission(PredefinedPermissions.CustomersCreateResetPasswords) && CurrentContactHasLogin; }
		}

		private bool _currentContactHasLogin = false;
		public bool CurrentContactHasLogin
		{
			get { return _currentContactHasLogin; }
			set
			{
				_currentContactHasLogin = value;
				OnUIThread(() =>
					{
						OnPropertyChanged();
						OnPropertyChanged("IsCreateLoginPasswordShow");
						OnPropertyChanged("IsLoginOnBehalfShow");
						OnPropertyChanged("IsResetPasswordShow");
						LoginOnBehalfCommand.RaiseCanExecuteChanged();
					});
			}
		}

		private ContactAccessState _contactAccessState = ContactAccessState.None;
		public ContactAccessState ContactAccessState
		{
			get { return _contactAccessState; }
			set
			{
				_contactAccessState = value;
				OnUIThread(() =>
				{
					OnPropertyChanged();
				});
			}
		}


		private string _currentContactLoginUserName = string.Empty;
		public string CurrentContactLoginUserName
		{
			get { return _currentContactLoginUserName; }
			set
			{
				_currentContactLoginUserName = value;
				OnPropertyChanged();
			}
		}


		public bool IsValid
		{
			get
			{
				bool result = false;

				if (InnerItem != null)
				{
					result = !string.IsNullOrEmpty(InnerItem.FullName);
				}

				return result;
			}
		}

		private IOrderRepository _orderRepository;
		public IOrderRepository OrderRepository
		{
			get
			{
				if (_orderRepository == null)
				{
					_orderRepository = _orderRepositoryFactory.GetRepositoryInstance();
				}
				return _orderRepository;
			}
		}

		private ISecurityRepository _securityRepository;
		public ISecurityRepository SecurityRepository
		{
			get
			{
				if (_securityRepository == null)
				{
					_securityRepository = _securityRepositoryFactory.GetRepositoryInstance();
				}
				return _securityRepository;
			}
		}

		private ICountryRepository _countryRepository;
		public ICountryRepository CountryRepository
		{
			get
			{
				if (_countryRepository == null)
				{
					_countryRepository = _countryRepositoryFactory.GetRepositoryInstance();
				}
				return _countryRepository;
			}
		}

		#endregion

		#region Handlers

		void _innerContact_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_innerItem.PropertyChanged -= _innerContact_PropertyChanged;
			_parentViewModel.IsModified = true;
			_innerItem.PropertyChanged += _innerContact_PropertyChanged;

			if (_parentViewModel != null)
			{
				_parentViewModel.DisplayNameUpdateCommand.Execute();
			}
		}

		#endregion
	}
}
