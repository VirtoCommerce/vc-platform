using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
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
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Generators;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
	public class CaseDetailViewModel : ViewModelBase, ITagControlExtended<Account>
	{
		#region Fields

		private Case _innerCase;
		private CaseActionState _caseAction;

		#endregion

		#region Dependencies

		private readonly ICustomerEntityFactory _entityFactory;
		private readonly CustomersDetailViewModel _parentViewModel;
		private readonly ICustomerRepository _customerRepository;
		private readonly IAuthenticationContext _authContext;
		private readonly IRepositoryFactory<ISecurityRepository> _securityRepositoryFactory;
		private readonly IViewModelsFactory<ICustomerViewModel> _customerVmFactory;

		#endregion

		#region Constructor

		public CaseDetailViewModel(CustomersDetailViewModel parentViewModel, Case innerCase,
								   ICustomerRepository customerRepository, CaseActionState caseAction,
			ICustomerEntityFactory entityFactory, IRepositoryFactory<ISecurityRepository> securityRepositoryFactory,
			IAuthenticationContext authContext, IViewModelsFactory<ICustomerViewModel> customerVmFactory)
		{
			_entityFactory = entityFactory;
			_securityRepositoryFactory = securityRepositoryFactory;
			_parentViewModel = parentViewModel;
			_authContext = authContext;
			_customerVmFactory = customerVmFactory;

			InnerItem = innerCase;
			_innerCase.PropertyChanged += _innerCase_PropertyChanged;
			_innerCase.Labels.CollectionChanged += Labels_CollectionChanged;
			_customerRepository = customerRepository;

			_caseAction = caseAction;

			CommandInits();
			CollectionInits();

			OnPropertyChanged("CaseHasTitle");
		}

		private void CommandInits()
		{
			CustomerAddCommand = new DelegateCommand(RaiseCustomerEditInteractionRequest,
													 () => InnerItem.Contact == null);
			CustomerEditCommand = new DelegateCommand(RaiseCustomerEditInteractionRequest,
													  () => InnerItem.Contact != null);
			CustomerDeleteCommand = new DelegateCommand(RaiseCustomerDeleteRequest, () => InnerItem.Contact != null);
			CaseDeleteCommand = new DelegateCommand(RaiseCaseDeleteRequest /* TODO: ????? */);

			TakeItCommand = new DelegateCommand(RaiseTakeIt);
			CCmeCommand = new DelegateCommand(RaiseCCme);


			CommonConfirmRequest = new InteractionRequest<Confirmation>();

			DeleteCaseCommand = new DelegateCommand(RaiseDeleteCase);

			SearchItemsForTagControlCommand = new DelegateCommand<string>(SearchItemsForTagControl);
			RefreshItemsForContactSearchCommand = new DelegateCommand(RaiseRefreshItemsForContactSearch);
		}

		private void CollectionInits()
		{
			AllAvailableCaseTemplates =
				_customerRepository.CaseTemplates.Where(x => x.IsActive).ExpandAll().OrderBy(x => x.Name).ToList();


			AssignedUserList = SecurityRepository.Accounts
											 .Where(
												 x => (x.RegisterType == (int)RegisterType.Administrator ||
													   x.RegisterType == (int)RegisterType.SiteAdministrator) &&
													  x.AccountState == (int)AccountState.Approved)
											 .OrderBy(x => x.UserName)
											 .ToList();

			if (!string.IsNullOrEmpty(InnerItem.AgentId) && !string.IsNullOrEmpty(InnerItem.AgentName))
			{
				var accID = InnerItem.AgentId;
				var acc = AssignedUserList.SingleOrDefault(a => a.MemberId == accID);

				if (acc != null)
				{
					SelectedAssignedUser = acc;
				}
			}
			else
			{
				if (_caseAction == CaseActionState.New)
				{
					var authorId = _authContext.CurrentUserId;

					var acc = AssignedUserList.SingleOrDefault(a => a.MemberId == authorId);
					if (acc != null)
					{
						SelectedAssignedUser = acc;
					}

				}
			}


			if (InnerItem.CaseCc.Count > 0)
			{
				CollectionFromTagControl.Clear();
				foreach (var ccc in InnerItem.CaseCc)
				{
                    var account = SecurityRepository.Accounts.Where(a => a.AccountId == ccc.AccountId).SingleOrDefault();
					if (account != null)
					{
						CollectionFromTagControl.Add(new TagControlItemViewModel(account) { IsEditing = false });
					}
				}
			}
			CollectionFromTagControl.Add(new TagControlItemViewModel(new Account()) { IsEditing = true });

			CollectionFromTagControl.CollectionChanged += (s, e) =>
				{
					_parentViewModel.IsModified = true;
				};


			CaseLabelList = new ObservableCollection<Label>(_innerCase.Labels);

			foreach (var label in CaseLabelList)
			{
				CaseLabelListFromTagControl.Add(new TagControlItemViewModel(label) { IsEditing = false });
			}

			CaseLabelListFromTagControl.Add(new TagControlItemViewModel(new Label()) { IsEditing = true });

			CaseLabelListFromTagControl.CollectionChanged += (s, e) =>
				{
					_parentViewModel.IsModified = true;
					CaseLabelList = new ObservableCollection<Label>(CaseLabelListFromTagControl.Where(i => i.IsEditing == false).Select(i => i.InnerItem as Label).ToList<Label>());
				};
		}

		#endregion

		#region CaseProperties


		public Case InnerItem
		{
			get { return _innerCase; }
			private set
			{
				_innerCase = value;
				OnPropertyChanged();
				SelectedCasePriority = (CasePriority)_innerCase.Priority;
				if (AllAvailableCaseTemplates != null)
				{
					if (!string.IsNullOrEmpty(_innerCase.CaseTemplateId))
					{
						var caseSubj =
							AllAvailableCaseTemplates.SingleOrDefault(cs => cs.CaseTemplateId == _innerCase.CaseTemplateId);
						if (caseSubj != null)
						{
							SelectedCaseTemplate = caseSubj;
						}
					}
					else
					{
						SelectedCaseTemplate = AllAvailableCaseTemplates.FirstOrDefault();
					}
				}


			}
		}

		public string TypeId
		{
			get { return _innerCase.Channel; }
			set
			{
				_innerCase.Channel = value;
				OnPropertyChanged();
				_parentViewModel.IsModified = true;
			}
		}



		#endregion

		#region Commands

		public DelegateCommand CustomerAddCommand { get; private set; }
		public DelegateCommand CustomerEditCommand { get; private set; }
		public DelegateCommand CustomerDeleteCommand { get; private set; }
		public DelegateCommand CaseDeleteCommand { get; private set; }

		public DelegateCommand TakeItCommand { get; private set; }
		public DelegateCommand CCmeCommand { get; private set; }

		public DelegateCommand RefreshItemsForContactSearchCommand { get; private set; }

		public DelegateCommand DeleteCaseCommand { get; private set; }

		#endregion

		#region PublicPropeties

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		public bool IsValid
		{
			get { return Validate(); }
		}

		private List<CaseTemplate> _allAvailableCaseTemplates;
		public List<CaseTemplate> AllAvailableCaseTemplates
		{
			get { return _allAvailableCaseTemplates; }
			private set
			{
				_allAvailableCaseTemplates = value;
				OnPropertyChanged();
			}
		}

		private CaseTemplate _selectedCaseTemplate;
		public CaseTemplate SelectedCaseTemplate
		{
			get { return _selectedCaseTemplate; }
			set
			{
				_selectedCaseTemplate = value;
				if (_selectedCaseTemplate != null)
				{
					var newValuesList = new List<PropertyValueEditViewModel>();
					if (_selectedCaseTemplate.CaseTemplateId == InnerItem.CaseTemplateId)
					{
						// load old property values
						_parentViewModel._oldCasePropertyValues.ForEach(x =>
							{
								x.PropertyChanged += _innerCase_PropertyChanged;
								newValuesList.Add(new PropertyValueEditViewModel(x));
							});
					}
					else
					{
						// create new property values
						_selectedCaseTemplate.CaseTemplateProperties.ToList().ForEach(x =>
						{
							var item = _entityFactory.CreateEntity<CasePropertyValue>();
							//item.CaseId = InnerItem.CaseId;
							item.Name = x.Name;
							item.ValueType = x.ValueType;
							item.CaseId = InnerItem.CaseId;

							item.PropertyChanged += _innerCase_PropertyChanged;

							newValuesList.Add(new PropertyValueEditViewModel(item));

						});

						InnerItem.CaseTemplate = _selectedCaseTemplate;
					}

					CaseTemplateValues = newValuesList;
				}
			}
		}

		private List<PropertyValueEditViewModel> _caseTemplateValues;
		public List<PropertyValueEditViewModel> CaseTemplateValues
		{
			get { return _caseTemplateValues; }
			set
			{
				_caseTemplateValues = value;
				OnPropertyChanged();
			}
		}


		private List<Account> _assignedUserList;
		public List<Account> AssignedUserList
		{
			get
			{
				if (_assignedUserList == null)
				{
					_assignedUserList = SecurityRepository.Accounts
											 .Where(
												 x => (x.RegisterType == (int)RegisterType.Administrator ||
												 x.RegisterType == (int)RegisterType.SiteAdministrator) &&
												 x.AccountState == (int)AccountState.Approved)
											 .OrderBy(x => x.UserName)
											 .ToList();
				}

				return _assignedUserList;
			}
			set
			{
				_assignedUserList = value;
				OnPropertyChanged();
			}
		}

		private Account _selectedAssignedUser;
		public Account SelectedAssignedUser
		{
			get { return _selectedAssignedUser; }
			set
			{
				_selectedAssignedUser = value;
				OnPropertyChanged();

				_parentViewModel.IsModified = true;
				if (InnerItem != null)
				{
					InnerItem.AgentId = _selectedAssignedUser.MemberId;
					InnerItem.AgentName = _selectedAssignedUser.UserName;
				}

			}
		}


		public CasePriority _selectedCasePriority;
		public CasePriority SelectedCasePriority
		{
			get { return _selectedCasePriority; }
			set
			{
				_selectedCasePriority = value;
				OnPropertyChanged();
				InnerItem.Priority = (int)value;
			}
		}

		private ObservableCollection<Label> _caseLabelList = new ObservableCollection<Label>();
		public ObservableCollection<Label> CaseLabelList
		{
			get { return _caseLabelList; }
			set
			{
				_caseLabelList = value;
				OnPropertyChanged();
			}
		}


		private ObservableCollection<TagControlItemViewModel> _caseLabelListFromTagControl = new ObservableCollection<TagControlItemViewModel>();
		public ObservableCollection<TagControlItemViewModel> CaseLabelListFromTagControl
		{
			get { return _caseLabelListFromTagControl; }
			set
			{
				_caseLabelListFromTagControl = value;
				OnPropertyChanged();
			}
		}


		private bool _isContactNameInEditMode = false;
		public bool IsContactNameInEditMode
		{
			get { return _isContactNameInEditMode; }
			set
			{
				_isContactNameInEditMode = value;
				OnPropertyChanged();
				OnPropertyChanged("NewContactFullName");
			}
		}

		private string _newContactFullName;
		public string NewContactFullName
		{
			get
			{
				if (_newContactFullName == null)
				{
					if (_parentViewModel.CurrentCustomer != null)
					{
						_newContactFullName = _parentViewModel.CurrentCustomer.FullName;
					}
					else
					{
						_newContactFullName = string.Empty;
					}
				}
				return _newContactFullName;
			}
			set
			{
				_newContactFullName = value;
				OnPropertyChanged();
			}
		}


		private ObservableCollection<Contact> _contactSearchList;
		public ObservableCollection<Contact> ContactSearchList
		{
			get { return _contactSearchList; }
			set
			{
				_contactSearchList = value;
				OnPropertyChanged();
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

		#endregion

		#region Validation

		private bool Validate()
		{
			return _innerCase.Validate() && !string.IsNullOrEmpty(InnerItem.CaseTemplateId)
				   && (!string.IsNullOrEmpty(InnerItem.AgentId)) && !string.IsNullOrEmpty(InnerItem.Title);
		}

		#endregion

		#region Handlers

		void _innerCase_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_innerCase.PropertyChanged -= _innerCase_PropertyChanged;
			_parentViewModel.IsModified = true;
			OnPropertyChanged("IsValid");
			OnPropertyChanged("CaseHasTitle");
			_innerCase.PropertyChanged += _innerCase_PropertyChanged;
		}

		void Labels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			_parentViewModel.IsModified = true;
			OnPropertyChanged("IsValid");
		}

		#endregion

		#region private methods

		private void RaiseCustomerEditInteractionRequest()
		{
			Contact item;
			string title;

			if (InnerItem.Contact == null)
			{
				item = _entityFactory.CreateEntity<Contact>();
				title = "Create Contact".Localize();
			}
			else
			{
				item = InnerItem.Contact.DeepClone(_entityFactory as IKnownSerializationTypes);
				title = "Edit Contact".Localize();
			}

			var itemVM = _customerVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));
			var confirmation = new ConditionalConfirmation(item.Validate) { Title = title, Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					if (InnerItem.Contact == null)
					{
						_parentViewModel.Repository.Add(item);
						InnerItem.ContactId = item.MemberId;
						InnerItem.Contact = item;
						InnerItem.ContactDisplayName = string.Format("{0}", item.FullName);

						RaiseCustomerAddEditDeleteCommandsCanExecute();

						_parentViewModel.CurrentCustomer = item;
						_parentViewModel.InitializeCustomerDetailViewModel();
					}
					else
						OnUIThread(() => InnerItem.Contact.InjectFrom<CloneInjection>(item));

					_parentViewModel.IsModified = true;
				}
			});
		}

		private void RaiseCustomerDeleteRequest()
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to detach Customer '{0}' from this Case?".Localize(), InnerItem.ContactDisplayName),
				Title = "Detach confirmation".Localize()
			};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					InnerItem.Contact = null;
					InnerItem.ContactId = null;
					InnerItem.ContactDisplayName = null;

					_parentViewModel.CurrentCustomer = null;
					_parentViewModel.CustomerDetailViewModel = null;

					RaiseCustomerAddEditDeleteCommandsCanExecute();
				}
			});
		}

		private void RaiseCustomerAddEditDeleteCommandsCanExecute()
		{
			CustomerAddCommand.RaiseCanExecuteChanged();
			CustomerEditCommand.RaiseCanExecuteChanged();
			CustomerDeleteCommand.RaiseCanExecuteChanged();
		}

		private void RaiseCaseDeleteRequest()
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Case '{0}'?".Localize(), InnerItem.Number),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					/// TODO: implement this
					InnerItem.Number = UniqueNumberGenerator.GetUniqueNumber();
				}
			});
		}

		private void RaiseTakeIt()
		{
			var userName = _authContext.CurrentUserName;
			var userId = _authContext.CurrentUserId;

			InnerItem.AgentId = userId;
			InnerItem.AgentName = userName;


			var sameUser = AssignedUserList.SingleOrDefault(s => s.AccountId == userId);
			if (sameUser != null)
			{
				SelectedAssignedUser = sameUser;
			}
		}

		private void RaiseCCme()
		{
			var userId = _authContext.CurrentUserId;

			var account =
				SecurityRepository.Accounts.Where(a => a.AccountId == userId).SingleOrDefault();
			if (account != null)
			{
				var accInResultCollection =
					CollectionFromTagControl.SingleOrDefault(
						i => (i.InnerItem as Account).AccountId == account.AccountId);

				if (accInResultCollection == null)
				{
					CollectionFromTagControl.Add(new TagControlItemViewModel(account));
				}

				var view = CollectionViewSource.GetDefaultView(CollectionFromTagControl);
				view.SortDescriptions.Add(new SortDescription("IsEditing", ListSortDirection.Ascending));
				view.Refresh();
			}
		}

		private async void RaiseRefreshItemsForContactSearch()
		{
			var contactsFromSearch =
				await
				Task.Run(
					() =>
					{
						return
							_customerRepository.Members.OfType<Contact>()
											   .Where(c => c.FullName.Contains(NewContactFullName))
											   .ToList();
					});

			ContactSearchList = new ObservableCollection<Contact>(contactsFromSearch);
		}

		private void RaiseDeleteCase()
		{
			if (_parentViewModel != null)
			{
				_parentViewModel.DeleteCaseCommand.Execute(InnerItem.CaseId);
			}
		}

		#endregion

		#region ITagControlExtended Members

		private ObservableCollection<Account> _searchedItems = new ObservableCollection<Account>();
		public ObservableCollection<Account> SearchedItems
		{
			get { return _searchedItems; }
			set
			{
				_searchedItems = value;
				OnPropertyChanged();
			}
		}

		public DelegateCommand<string> SearchItemsForTagControlCommand { get; private set; }

		private void SearchItemsForTagControl(string inputText)
		{
			if (string.IsNullOrEmpty(inputText))
				return;

			var acccountsFromSearch = SecurityRepository.Accounts.Where(a => a.UserName.Contains(inputText)).ToList();
			var accountsWithFilter = acccountsFromSearch.Except(CollectionFromTagControl.Where(i => i.IsEditing == false).Select(i => i.InnerItem as Account), new AccountEqualityComparer());
			SearchedItems = new ObservableCollection<Account>(accountsWithFilter);
		}

		//private ObservableCollection<Account> _resultCollection = new ObservableCollection<Account>();
		//public ObservableCollection<Account> ResultCollection { get { return _resultCollection; } }

		private ObservableCollection<TagControlItemViewModel> _collectionFromTagControl = new ObservableCollection<TagControlItemViewModel>();
		public ObservableCollection<TagControlItemViewModel> CollectionFromTagControl
		{
			get { return _collectionFromTagControl; }
			set
			{
				_collectionFromTagControl = value;
				OnPropertyChanged();
			}
		}


		public class AccountEqualityComparer : IEqualityComparer<Account>
		{

			public bool Equals(Account x, Account y)
			{
				if (x == null || y == null)
					return false;

				if (x.AccountId == y.AccountId)
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			public int GetHashCode(Account obj)
			{
				int hashCode = obj.AccountId.GetHashCode() ^ obj.AccountState ^ obj.UserName.Length;
				return hashCode;
			}
		}

		#endregion


	}
}
