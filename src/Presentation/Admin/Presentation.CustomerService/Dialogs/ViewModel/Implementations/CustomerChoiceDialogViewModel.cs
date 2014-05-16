using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
	public class CustomerChoiceDialogViewModel : ViewModelBase, ICustomerChoiceDialogViewModel
	{
		#region Dependencies
		private readonly IRepositoryFactory<ICustomerRepository> _customerRepositoryFactory;
		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
		#endregion

		#region Constructor

		public CustomerChoiceDialogViewModel(IRepositoryFactory<ICustomerRepository> customerRepositoryFactory, IRepositoryFactory<IOrderRepository> orderRepositoryFactory)
		{
			_customerRepositoryFactory = customerRepositoryFactory;
			_orderRepositoryFactory = orderRepositoryFactory;

			InitCommands();
			IsNew = false;

			ButtonName = "Create new case".Localize();
			Confirmed = false;
		}

		#endregion

		#region Commands

		private void InitCommands()
		{
			SearchCustomerCommand = new DelegateCommand(RaiseSearchCustomers, () => !IsInSearch);

			CommonNotificationRequest = new InteractionRequest<Notification>();
			CreateCustomerRequest = new InteractionRequest<Confirmation>();
			CloseNotificationRequest = new InteractionRequest<Notification>();
			OpenCustomerRequest = new InteractionRequest<Confirmation>();
		}

		public DelegateCommand SearchCustomerCommand { get; private set; }

		private async void RaiseSearchCustomers()
		{
			IsInSearch = true;
			ContactList.Clear();

			var highPriorityResult = await SearchCustomerWithHighPriorityAsync();

			if (highPriorityResult != null)
			{

				HighPriorityContact.Clear();
				HighPriorityContact = new ObservableCollection<Contact>(highPriorityResult);


				foreach (var contact in HighPriorityContact)
				{
					ContactList.Add(contact);
				}

				ShowLoadingAnimation = false;
				OnPropertyesChanged();
			}

			var lowPriorityResult = await SearchCustomerWithLowPriorityAsync();

			if (lowPriorityResult != null)
			{
				LowPriorityContact.Clear();
				LowPriorityContact = new ObservableCollection<Contact>(lowPriorityResult);

				foreach (var contact in LowPriorityContact)
				{

					var contactInCommonResultList =
						ContactList.SingleOrDefault(cont => cont.MemberId == contact.MemberId);

					if (contactInCommonResultList == null)
					{
						ContactList.Add(contact);
					}
				}

				ShowLoadingAnimation = false;
				OnPropertyesChanged();
			}
			IsInSearch = false;
		}

		private Task<List<Contact>> SearchCustomerWithHighPriorityAsync()
		{
			IsAfterSearch = true;
			ShowLoadingAnimation = true;
			var searchTask = Task.Factory.StartNew(() =>
				{
					var returnValue = new List<Contact>();
					if (string.IsNullOrEmpty(SearchCaseNumberKeyword) && string.IsNullOrEmpty(SearchCustomerKeyword)
						&& string.IsNullOrEmpty(SearchEmailKeyword) && string.IsNullOrEmpty(SearchOrderNumberKeyword))
					{
						return returnValue;
					}

					var query = _customerRepositoryFactory.GetRepositoryInstance().Members.OfType<Contact>().Expand(c => c.Addresses)
														  .Expand(c => c.Emails).Expand(c => c.Labels).Expand(c => c.Notes)
														  .Expand(c => c.Phones).Expand(c => c.Cases);

					//customer number
					if (!string.IsNullOrEmpty(SearchCustomerKeyword))
					{
						query = query.Where(
							cont => cont.MemberId.Contains(SearchCustomerKeyword));
					}


					//case number
					if (!string.IsNullOrEmpty(SearchCaseNumberKeyword))
					{
						var caseList = _customerRepositoryFactory.GetRepositoryInstance().Cases;

						var caseFromDb =
							caseList.Where(cs => cs.CaseId.Contains(SearchCaseNumberKeyword)).FirstOrDefault();

						if (caseFromDb != null && !string.IsNullOrEmpty(caseFromDb.ContactId))
						{
							query = query.Where(cont => cont.MemberId.Contains(caseFromDb.ContactId));
						}
					}

					//email
					if (!string.IsNullOrEmpty(SearchEmailKeyword))
					{
						query = query.Where(c => c.Emails.Any(e => e.Address.Contains(SearchEmailKeyword)));
					}


					//order number
					if (!string.IsNullOrEmpty(SearchOrderNumberKeyword))
					{
						List<Order> orders;
						using (var ordersRepository = _orderRepositoryFactory.GetRepositoryInstance())
						{
							orders =
								ordersRepository.Orders.Where(ord => ord.TrackingNumber.Contains(SearchOrderNumberKeyword))
												.ToList();
						}
						var ids = orders.Select(o => o.CustomerId).Distinct().ToList();

						foreach (var id in ids)
						{
							var contacts = _customerRepositoryFactory.GetRepositoryInstance().Members.OfType<Contact>().Expand(c => c.Addresses)
															  .Expand(c => c.Emails).Expand(c => c.Labels).Expand(c => c.Notes)
															  .Expand(c => c.Phones).Expand(c => c.Cases)
															  .Where(c => c.MemberId.Contains(id) && id.Contains(c.MemberId)).ToList();
							returnValue.AddRange(contacts);
						}
					}
					else
					{
						returnValue = query.ToList();
					}

					return returnValue;
				});


			return searchTask;

		}

		private Task<List<Contact>> SearchCustomerWithLowPriorityAsync()
		{
			IsAfterSearch = true;
			ShowLoadingAnimation = true;
			var searchTask = Task.Factory.StartNew(() =>
				{

					if (string.IsNullOrEmpty(SearchPhoneNumberKeyword) &&
						string.IsNullOrEmpty(SearchCustomerNameKeyword))
					{
						return new List<Contact>();
					}
					else
					{
						var query = _customerRepositoryFactory.GetRepositoryInstance().Members.OfType<Contact>().Expand(c => c.Addresses)
													   .Expand(c => c.Emails).Expand(c => c.Labels).Expand(c => c.Notes)
													   .Expand(c => c.Phones).Expand(c => c.Cases);

						//phone number
						if (!string.IsNullOrEmpty(SearchPhoneNumberKeyword))
						{
							var phoneList = _customerRepositoryFactory.GetRepositoryInstance().Phones;

							var phone = phoneList.Where(pn => pn.Number.Contains(SearchPhoneNumberKeyword))
												 .FirstOrDefault();

							if (phone != null && !string.IsNullOrEmpty(phone.MemberId))
							{
								query = query.Where(cont => cont.MemberId.Contains(phone.MemberId));
							}
						}

						//customer name
						if (!string.IsNullOrEmpty(SearchCustomerNameKeyword))
						{

							query = query.Where(
								cont =>
								cont.FullName.Contains(SearchCustomerNameKeyword)
								);
						}


						return query.ToList();
					}
				});


			return searchTask;

		}


		public InteractionRequest<Notification> CommonNotificationRequest { get; private set; }
		public InteractionRequest<Notification> CloseNotificationRequest { get; private set; }


		public DelegateCommand CreateNewCustomerCommand { get; private set; }
		public InteractionRequest<Confirmation> CreateCustomerRequest { get; private set; }

		public DelegateCommand OpenCustomerCommand { get; private set; }
		public InteractionRequest<Confirmation> OpenCustomerRequest { get; private set; }

		#endregion

		#region Properties

		public string ButtonName { get; private set; }

		public bool Confirmed { get; private set; }

		private CaseActionState _caseAction;
		public CaseActionState CaseAction
		{
			get { return _caseAction; }
			set
			{
				_caseAction = value;
				OnPropertyChanged();
			}
		}

		private ContactActionState _contactAction;
		public ContactActionState ContactAction
		{
			get { return _contactAction; }
			set
			{
				_contactAction = value;
				OnPropertyChanged();
			}
		}

		private Case _selectedCase;
		public Case SelectedCase
		{
			get { return _selectedCase; }
			set
			{
				_selectedCase = value;
				OnPropertyChanged();
			}
		}


		private bool _isInSearch;
		public bool IsInSearch
		{
			get { return _isInSearch; }
			set
			{
				_isInSearch = value;
				OnPropertyChanged();
			}
		}

		#endregion

		#region Private method

		private void OnPropertyesChanged()
		{
			OnPropertyChanged("IsValid");
			OnPropertyChanged("IsEmpty");
			OnPropertyChanged("IsAfterSearch");
			OnPropertyChanged("TotalSearched");
			OnPropertyChanged("ContactList");
		}

		#endregion

		#region ICustomerChoiceDialogViewModel

		public bool IsEmpty
		{
			get
			{
				return ContactList.Count == 0;
			}
		}

		public bool IsAfterSearch { get; private set; }

		//search keywords
		public string SearchPhoneNumberKeyword { get; set; }
		public string SearchEmailKeyword { get; set; }
		public string SearchCaseNumberKeyword { get; set; }
		public string SearchCustomerNameKeyword { get; set; }
		public string SearchOrderNumberKeyword { get; set; }
		public string SearchCustomerKeyword { get; set; }


		public long TotalSearched { get; set; }

		public bool IsValid
		{
			get
			{
				return ContactList.Count > 0 && CurrentContact != null;
			}
		}


		public bool IsNew { get; private set; }

		private ObservableCollection<Contact> _contactList = new ObservableCollection<Contact>();
		public ObservableCollection<Contact> ContactList
		{
			get { return _contactList; }
			set { _contactList = value; }
		}


		private ObservableCollection<Contact> _highPriorityContact = new ObservableCollection<Contact>();
		public ObservableCollection<Contact> HighPriorityContact
		{
			get { return _highPriorityContact; }
			set
			{
				_highPriorityContact = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Contact> _lowPriorityContact = new ObservableCollection<Contact>();
		public ObservableCollection<Contact> LowPriorityContact
		{
			get { return _lowPriorityContact; }
			set
			{
				_lowPriorityContact = value;
				OnPropertyChanged();
			}
		}

		private Contact _currentContact;
		public Contact CurrentContact
		{
			get
			{
				return _currentContact;
			}
			set
			{
				_currentContact = value;
				OnPropertyChanged();
				OnPropertyChanged("IsValid");
			}
		}

		public string CurrentContactId
		{
			get
			{
				return IsValid ? CurrentContact.MemberId : null;
			}
		}

		#endregion

		#region Handlers

		public void ContactListSelectionChanged()
		{
			OnPropertyesChanged();
		}

		public void ChangeContactAndCaseAtionsToNew()
		{
			ContactAction = ContactActionState.New;
			CaseAction = CaseActionState.New;
		}

		public void ChangeContactActionToOpenAndCaseActionToNew()
		{
			ContactAction = ContactActionState.Open;
			CaseAction = CaseActionState.New;
		}

		public void ChangeContactAndCaseActionsToOpen()
		{
			ContactAction = ContactActionState.Open;
			CaseAction = CaseActionState.Open;
		}



		#endregion

	}
}
