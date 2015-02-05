using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Adaptors;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Controls;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Extensions;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Generators;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
    public class CustomersDetailViewModel : ViewModelBase, ICustomersDetailViewModel, IClosable, IMinimizable,
                                            IOpenTracking, ISupportAcceptChanges, ISupportDelayInitialization
    {
        #region Fields

        private Case _innerItem;
        private Contact _innerContact;

        private Case _originalItem;
        private Contact _originalContact;

        private CaseDetailViewModel _caseDetailViewModel;
        private CustomerDetailViewModel _customerDetailViewModel;

        internal List<CasePropertyValue> _oldCasePropertyValues = new List<CasePropertyValue>();

        private readonly string _authorId;
        private readonly string _authorName;

        private bool _isItemsInitialized;
        private bool _isNewContactInitialized;

        #endregion

        #region Dependencies

        private readonly NavigationManager _navManager;
        private readonly ICustomerEntityFactory _entityFactory;
        private readonly IAuthenticationContext _authContext;
        private readonly ICustomersCommonViewModel _parentViewModel;
        private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;
        private readonly IViewModelsFactory<CaseDetailViewModel> _caseDetailVmFactory;
        private readonly IViewModelsFactory<CustomerDetailViewModel> _customerDetailVmFactory;
        private readonly IViewModelsFactory<ICreateCustomerDialogViewModel> _wizardCustomerVmFactory;
        private readonly IViewModelsFactory<IKnowledgeBaseDialogViewModel> _knowledgeBaseGroupVmFactory;

        #endregion

        #region Const

        private const string NameCustomerInformation = "Customer Information";
        private const string NameCasesInformation = "Case Information";

        #endregion

        #region Constructor

        public CustomersDetailViewModel(ICustomerEntityFactory entityFactory, ICustomerRepository repository,
                    NavigationManager navManager, IRepositoryFactory<ICustomerRepository> repositoryFactory,
                    IAuthenticationContext authContext, ICustomersCommonViewModel parentViewModel,
                    Case innerCase, Contact innerContact,
                    CaseActionState caseAction, ContactActionState contactAction,
                    IViewModelsFactory<CaseDetailViewModel> caseDetailVmFactory, IViewModelsFactory<CustomerDetailViewModel> customerDetailVmFactory,
                    IViewModelsFactory<ICreateCustomerDialogViewModel> wizardCustomerVmFactory,
                    IViewModelsFactory<IKnowledgeBaseDialogViewModel> knowledgeBaseGroupVmFactory)
        {
            _entityFactory = entityFactory;
            Repository = repository;
            _authContext = authContext;
            _navManager = navManager;
            _repositoryFactory = repositoryFactory;
            _caseDetailVmFactory = caseDetailVmFactory;
            _customerDetailVmFactory = customerDetailVmFactory;
            _wizardCustomerVmFactory = wizardCustomerVmFactory;
            _knowledgeBaseGroupVmFactory = knowledgeBaseGroupVmFactory;
            _parentViewModel = parentViewModel;

            OriginalItem = innerCase;
            OriginalContact = innerContact;

            InnerItem = innerCase;
            CurrentCustomer = innerContact;

            CaseActionState = caseAction;
            ContactActionState = contactAction;

            InitializeCommands();

            _isItemsInitialized = false;

            ViewTitle = new ViewTitleBase
                {
                    SubTitle = string.Format("Case #{0}".Localize(), InnerItem.Number),
                    Title = "Customer Service"
                };


            _authorId = _authContext.CurrentUserId;
            _authorName = _authContext.CurrentUserName;
        }

        protected void InitializeCommands()
        {
            OpenItemCommand = new DelegateCommand(RiseOpenItemCommand);
            MinimizeCommand = new DelegateCommand(() => MinimizableViewRequestedEvent(this, null));

            SaveChangesCommand = new DelegateCommand<object>(RaiseSaveInteractionRequest, x => IsValid);
            CancelCommand = new DelegateCommand<object>(RaiseCancelInteractionRequest);
            CancelConfirmRequest = new InteractionRequest<Confirmation>();
            SaveConfirmRequest = new InteractionRequest<Confirmation>();

            ResolveAndSaveCommand = new DelegateCommand<object>(RaiseResolveAndSaveInteractionRequest,
                                                                (x) => CanRaiseResolveAndSaveChangedInteractionRequest());
            ProcessAndSaveCommand = new DelegateCommand<object>(RaiseProcessAndSaveInteractionRequest,
                                                                (x) => CanRaiseProcessAndSaveInteractionRequest());
            ResolveAndSaveCommand.RaiseCanExecuteChanged();
            ProcessAndSaveCommand.RaiseCanExecuteChanged();

            OpenCaseCommand = new DelegateCommand<Case>(RiseOpenCaseInteractionRequest,
                                                        x => x != null && x.CaseId != InnerItem.CaseId);

            SwitchContactNameEditModeCommand = new DelegateCommand(SwitchContactNameEditMode);


            CreateNewCaseForCurrentContactCommand = new DelegateCommand(CreateNewCaseForCurrentContact);
            DeleteCurrentContactCommand = new DelegateCommand(DeleteCurrentContact, () => _authContext.CheckPermission(PredefinedPermissions.CustomersDeleteCustomer));

            CreateCustomerInteractionRequest = new InteractionRequest<ConditionalConfirmation>();
            CommonInfoRequest = new InteractionRequest<ConditionalConfirmation>();

            DisplayNameUpdateCommand = new DelegateCommand(DisplayNameUpdate);

            RefreshContactOrdersPanelCommand = new DelegateCommand(RefreshContactOrdersPanel);
            LoadCompletedCommand = new DelegateCommand(LoadCompleted);

            DeleteCaseCommand = new DelegateCommand<string>(DeleteCase);
        }

        /// <summary>
        /// Initialize leftTab case and customer viewmodels.
        /// Initialize communication control
        /// </summary>
        private void InitializeViewModels()
        {
            InitializeCaseViewModel();
            InitializeCustomerDetailViewModel();

            CasePropertySets = Repository.CasePropertySets
                                          .Expand(x => x.CaseProperties)
                                          .OrderBy(x => x.Priority)
                                          .ToList();
            OnPropertyChanged("CasePropertySets");

            // InnerItem.ContactId
            if (CurrentCustomer != null)
            {
                var contactInfoSet = CasePropertySets.Where(x => x.Name == NameCustomerInformation).SingleOrDefault();
                if (contactInfoSet != null)
                {
                    ContactRootPropertyForContactCustomFields = contactInfoSet.DeepClone(_entityFactory as IKnownSerializationTypes);
                    var contactPropertyValues =
                        Repository.ContactPropertyValues.Where(x => x.ContactId == CurrentCustomer.MemberId).ToList();
                    contactPropertyValues.ForEach(
                        x =>
                        {
                            contactInfoSet.CaseProperties.Add(new CaseProperty
                                {
                                    Name = x.Name,
                                    FieldName = x.ToString(),
                                    Priority = x.Priority
                                });
                        });
                }
            }

            var casesInfoSet = CasePropertySets.Where(x => x.Name == NameCasesInformation).SingleOrDefault();
            if (casesInfoSet != null)
            {
                CaseRootPropertyForCaseCustomFields = casesInfoSet.DeepClone(_entityFactory as IKnownSerializationTypes);
                var CasePropertyValues =
                    Repository.CasePropertyValues.Where(x => x.CaseId == InnerItem.CaseId).ToList();
                CasePropertyValues.ForEach(
                    x =>
                    {
                        casesInfoSet.CaseProperties.Add(new CaseProperty
                            {
                                Name = x.Name,
                                FieldName = x.ToString(),
                                Priority = x.Priority
                            });
                    });
            }

            if (CurrentCustomer != null)
            {
                var query = Repository.Cases;
                if (_authContext.CheckPermission(PredefinedPermissions.CustomersViewAssignedCases) ||
                    _authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll))
                {
                    if (_authContext.CheckPermission(PredefinedPermissions.CustomersViewAssignedCases) &&
                        !_authContext.CheckPermission(PredefinedPermissions.CustomersViewAllCasesAll))
                    {
                        query = query.Where(c => c.AgentId == _authorId);
                    }
                    CustomerCaseList = query.Where(x => x.ContactId == CurrentCustomer.MemberId)
                                            .OrderByDescending(x => x.Created)
                                            .ToList();
                }
                else
                {
                    CustomerCaseList = new List<Case>();
                }
            }
        }

        private void InitializeCaseViewModel()
        {
            if (InnerItem != null)
            {
                var caseVmParameters = new List<KeyValuePair<string, object>>
					{
						new KeyValuePair<string, object>("parentViewModel", this),
						new KeyValuePair<string, object>("innerCase", InnerItem),
						new KeyValuePair<string, object>("customerRepository", Repository),
						new KeyValuePair<string, object>("caseAction", CaseActionState)
					};
                CaseDetailViewModel = _caseDetailVmFactory.GetViewModelInstance(caseVmParameters.ToArray());
            }
            else
            {
                CaseDetailViewModel = null;
            }
        }

        internal void InitializeCustomerDetailViewModel()
        {
            if (CurrentCustomer != null)
            {
                var customerVmParameters = new List<KeyValuePair<string, object>>
					{
						new KeyValuePair<string, object>("parentViewModel", this),
						new KeyValuePair<string, object>("innerContact", CurrentCustomer),
						new KeyValuePair<string, object>("customerRepository", Repository)
					};
                CustomerDetailViewModel = _customerDetailVmFactory.GetViewModelInstance(customerVmParameters.ToArray());
            }
            else
            {
                CustomerDetailViewModel = null;
            }
        }

        public DelegateCommand ShowPreviousAlertCommand { get; private set; }
        public DelegateCommand ShowNextAlertCommand { get; private set; }
        public ICollectionView CaseAlertsView { get; private set; }

        public int CaseAlertCurrentPosition
        {
            get { return CaseAlertsView != null ? CaseAlertsView.CurrentPosition + 1 : 0; }
        }

        public int CaseAlertCount
        {
            get { return CaseAlertsView != null ? CaseAlertsView.Cast<object>().Count() : 0; }
        }

        #endregion

        #region Commands

        public DelegateCommand<object> ProcessAndSaveCommand { get; private set; }
        public DelegateCommand<object> ResolveAndSaveCommand { get; private set; }

        public DelegateCommand SwitchContactNameEditModeCommand { get; private set; }

        public DelegateCommand CreateNewCaseForCurrentContactCommand { get; private set; }
        public DelegateCommand DeleteCurrentContactCommand { get; private set; }

        public InteractionRequest<ConditionalConfirmation> CreateCustomerInteractionRequest { get; private set; }
        public InteractionRequest<ConditionalConfirmation> CommonInfoRequest { get; private set; }

        public DelegateCommand DisplayNameUpdateCommand { get; private set; }

        public DelegateCommand RefreshContactOrdersPanelCommand { get; private set; }
        public DelegateCommand LoadCompletedCommand { get; private set; }

        public DelegateCommand<string> DeleteCaseCommand { get; private set; }

        #endregion

        #region Commands Implementation

        private void LoadCompleted()
        {
            if (CaseActionState == CaseActionState.New && ContactActionState == ContactActionState.New
                && !_isNewContactInitialized)
            {
                ShowCreateCustomerDialog();
                InitializeCustomerDetailViewModel();
            }
        }

        public void RiseOpenItemCommand()
        {
            _navManager.Navigate(NavigationData);
        }

        private bool Validate()
        {
            var result = CaseActionState == CaseActionState.None
                             ? CustomerDetailViewModel.IsValid
                             : CaseDetailViewModel.IsValid && CustomerDetailViewModel.IsValid;
            return result;

        }

        private void RaiseSaveInteractionRequest(object operationForSave)
        {
            SaveChangesInteraction(operationForSave);
        }

        private async void SaveChangesInteraction(object operation)
        {
            if (Validate())
            {
                await DoSaveChanges();
                IsModified = false;
            }
        }

        private void RaiseCancelInteractionRequest(object arg)
        {
            if (IsModified)
            {

                var confirmation = new RefusedConfirmation { Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory) };
                if (CaseActionState == CaseActionState.None)
                {
                    confirmation.Content = string.Format("Save changes to contact '{0}'?".Localize(), CurrentCustomer.FullName);
                }
                else
                {
                    confirmation.Content = string.Format("Save changes to case '{0}'?".Localize(), InnerItem.Number);
                }

                CancelConfirmRequest.Raise(confirmation,
                       async x =>
                       {
                           if (x.Confirmed)
                           {
                               if (Validate())
                               {
                                   //Save
                                   await DoSaveChanges();
                                   Close();
                               }
                               else
                               {
                                   if (CommonInfoRequest != null)
                                   {
                                       CommonInfoRequest.Raise(new ConditionalConfirmation
                                           {
                                               Title = "Warning".Localize(null, LocalizationScope.DefaultCategory),
                                               Content = "Can't be saved, because not all the fields are filled".Localize()
                                           }, y => { });
                                   }
                               }
                           }
                           else if (((RefusedConfirmation)x).Refused)
                           {
                               //Reloading viewModels
                               InnerItem = _originalItem;
                               IsModified = false;
                               _isItemsInitialized = false;
                               CurrentCustomer = _originalContact;
                               InitializeViewModels();
                               Close();
                           }
                           else
                           {
                               var cancelArg = arg as CancelEventArgs;
                               if (cancelArg != null)
                               {
                                   cancelArg.Cancel = true;
                                   OpenItemCommand.Execute();
                               }
                           }
                       });
            }
            else
                Close();
        }

        private async void RaiseResolveAndSaveInteractionRequest(object operationForSave)
        {
            InnerItem.Status = CaseStatus.Resolved.ToString();
            OnPropertyChanged("IsReadOnlyActive");
            await DoSaveChanges();
            IsModified = false;
        }

        private bool CanRaiseResolveAndSaveChangedInteractionRequest()
        {
            var result = false;

            if (CaseActionState == CaseActionState.Open)
            {
                result = Status == CaseStatus.Pending && IsValidForSave;
            }

            return result;
        }

        private async void RaiseProcessAndSaveInteractionRequest(object operationForSave)
        {
            InnerItem.Status = CaseStatus.Pending.ToString();
            OnPropertyChanged("IsReadOnlyActive");
            await DoSaveChanges();
            IsModified = false;
        }

        private bool CanRaiseProcessAndSaveInteractionRequest()
        {
            var result = false;

            if (CaseActionState == CaseActionState.Open)
            {
                result = Status == CaseStatus.Open && IsValidForSave;
            }

            return result;

        }

        private void RaiseShowPreviousAlertInteractionRequest()
        {
            StepCurrentAlert(false);
        }

        private void RaiseShowNextAlertInteractionRequest()
        {
            StepCurrentAlert(true);
        }

        private void StepCurrentAlert(bool forward)
        {
            if (forward)
                CaseAlertsView.MoveCurrentToNext();
            else
                CaseAlertsView.MoveCurrentToPrevious();

            OnPropertyChanged("CaseAlertCurrentPosition");
            ShowPreviousAlertCommand.RaiseCanExecuteChanged();
            ShowNextAlertCommand.RaiseCanExecuteChanged();
        }

        private void SwitchContactNameEditMode()
        {
            if (CaseDetailViewModel != null)
            {
                if (CaseDetailViewModel.IsContactNameInEditMode)
                {
                    CaseDetailViewModel.IsContactNameInEditMode = false;
                    if (CurrentCustomer.FullName != CaseDetailViewModel.NewContactFullName)
                    {
                        CurrentCustomer.FullName = CaseDetailViewModel.NewContactFullName;
                        InnerItem.ContactDisplayName = CurrentCustomer.FullName;
                        OnPropertyChanged("IsModified");
                    }
                }
                else
                {
                    CaseDetailViewModel.IsContactNameInEditMode = true;
                }
            }
        }

        private void CreateNewCaseForCurrentContact()
        {
            if (CaseActionState == CaseActionState.None)
            {
                InnerItem = new Case { Status = CaseStatus.Open.ToString(), Number = UniqueNumberGenerator.GetUniqueNumber() };
                InitializeCaseViewModel();
                CaseActionState = CaseActionState.New;
            }
            else
            {
                if (_parentViewModel != null)
                {
                    _parentViewModel.CreateNewEmailCaseCommand.Execute(CurrentCustomer);
                }
            }
        }

        private void DeleteCurrentContact()
        {
            if (CommonInfoRequest != null)
            {
                var confirmation = new ConditionalConfirmation()
                    {
                        Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory),
                        Content = string.Format("Are you sure you want to delete contact '{0}'?".Localize(), CurrentCustomer.FullName)
                    };

                CommonInfoRequest.Raise(confirmation,
                    (x) =>
                    {
                        if (x.Confirmed)
                        {
                            if (_parentViewModel != null)
                            {
                                Close();
                                _parentViewModel.DeleteCustomerCommand.Execute(CurrentCustomer);
                            }
                        }
                    });
            }
        }

        private void DisplayNameUpdate()
        {
            OnPropertyChanged("DisplayName");
            if (ViewTitle != null && IsCustomerOptionSelected)
            {
                ViewTitle.SubTitle = CurrentCustomer != null ? CurrentCustomer.FullName : string.Empty;
            }
            OnPropertyChanged("ViewTitle");
        }

        private void RefreshContactOrdersPanel()
        {
            if (CustomerDetailViewModel != null)
            {
                CustomerDetailViewModel.RefreshCustomerOrdersCommand.Execute();
            }
        }

        private void DeleteCase(string item)
        {
            if (_parentViewModel != null && CommonInfoRequest != null)
            {
                var confirmation = new ConditionalConfirmation()
                {
                    Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory),
                    Content = string.Format("Are you sure you want to delete case '{0}'?".Localize(), InnerItem.Number)
                };

                CommonInfoRequest.Raise(confirmation,
                    (x) =>
                    {
                        if (x.Confirmed)
                        {
                            _parentViewModel.DeleteCaseCommand.Execute(item);
                            Close();
                        }
                    });



            }
        }

        private void Close()
        {
            CloseViewRequestedEvent(this, EventArgs.Empty);
            _isItemsInitialized = false;
        }

        #endregion

        #region Public Properties

        private bool _isModified;
        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValidForSave");
                OnPropertyChanged("IsValid");
                SaveChangesCommand.RaiseCanExecuteChanged();
                ResolveAndSaveCommand.RaiseCanExecuteChanged();
                ProcessAndSaveCommand.RaiseCanExecuteChanged();

                var caseCommunications = CaseCommunications as CaseCommunicationControlViewModel;
                if (caseCommunications != null && caseCommunications.Items != null && caseCommunications.IsModified != value)
                {
                    caseCommunications.IsModified = value;
                }
            }
        }

        public bool CaseHasOneCommunicationItem
        {
            get
            {
                var result = false;
                if (CaseCommunications != null)
                {
                    result = CaseCommunications.HasOneValidItem;
                }
                return result;
            }
        }

        public CaseDetailViewModel CaseDetailViewModel
        {
            get { return _caseDetailViewModel; }
            set
            {
                _caseDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public CustomerDetailViewModel CustomerDetailViewModel
        {
            get { return _customerDetailViewModel; }
            set
            {
                _customerDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private CaseActionState _caseActionState = CaseActionState.None;

        public CaseActionState CaseActionState
        {
            get { return _caseActionState; }
            set
            {
                _caseActionState = value;
                OnPropertyChanged();
                OnCaseOrContactStateChanged();
            }
        }

        private ContactActionState _contactActionState = ContactActionState.None;

        public ContactActionState ContactActionState
        {
            get { return _contactActionState; }
            set
            {
                _contactActionState = value;
                OnPropertyChanged();
                OnCaseOrContactStateChanged();
            }
        }


        public bool IsValidForSave
        {
            get
            {
                bool result;

                if (CaseActionState == CaseActionState.None)
                {
                    result = CustomerDetailViewModel != null && CustomerDetailViewModel.IsValid;
                }
                else
                {
                    result = CaseDetailViewModel != null && CustomerDetailViewModel != null && CaseDetailViewModel.IsValid && CustomerDetailViewModel.IsValid;
                }

                return result;
            }
        }

        public bool IsValid
        {
            get
            {
                return IsModified && IsValidForSave;
            }
        }

        public List<CasePropertySet> CasePropertySets { get; private set; }


        private DetailViewOptions _selectedDetailViewOption;
        public DetailViewOptions SelectedDetailViewOption
        {
            get { return _selectedDetailViewOption; }
            set
            {
                _selectedDetailViewOption = value;
                OnPropertyChanged();
            }
        }

        private bool _isCaseOptionSelected;
        public bool IsCaseOptionSelected
        {
            get { return _isCaseOptionSelected; }
            set
            {
                _isCaseOptionSelected = value;
                OnPropertyChanged();
                if (value)
                {
                    SelectedDetailViewOption = DetailViewOptions.CaseOptions;
                    if (ViewTitle != null)
                    {
                        ViewTitle.SubTitle = string.Format("Case #{0}".Localize(), InnerItem.Number);
                    }
                }
            }
        }

        private bool _isCustomerOptionSelected;
        public bool IsCustomerOptionSelected
        {
            get { return _isCustomerOptionSelected; }
            set
            {
                _isCustomerOptionSelected = value;
                OnPropertyChanged();
                if (value)
                {
                    SelectedDetailViewOption = DetailViewOptions.CustomerOptions;
                    if (ViewTitle != null)
                    {
                        if (ViewTitle != null)
                        {
                            ViewTitle.SubTitle = CurrentCustomer != null ? CurrentCustomer.FullName : string.Empty;
                        }
                    }
                }
            }
        }

        public ICustomerRepository Repository { get; private set; }

        private CasePropertySet _contactRootPropertyForContactCustomFields;
        public CasePropertySet ContactRootPropertyForContactCustomFields
        {
            get { return _contactRootPropertyForContactCustomFields; }
            set
            {
                _contactRootPropertyForContactCustomFields = value;
                OnPropertyChanged();
                if (value == null)
                    return;

                var newValueList = new List<PropertyValueWithFieldNameEditViewModel>();

                //load values
                value.CaseProperties.ToList().ForEach(x =>
                    {
                        x.PropertyChanged += (s, e) =>
                            {
                                IsModified = true;
                            };

                        if (CurrentCustomer.HasProperty(GetPropertyWithoutType(x.FieldName)))
                        {
                            var propertyValueType = CurrentCustomer.GetPropertyValueType(GetPropertyWithoutType(x.FieldName));

                            var item = new ContactPropertyValue
                                {
                                    Name = x.Name,
                                    Priority = x.Priority,
                                    ContactId = CurrentCustomer.MemberId,
                                    ValueType = (int)propertyValueType
                                };


                            newValueList.Add(new PropertyValueWithFieldNameEditViewModel(item)
                                {
                                    FieldName = GetPropertyWithoutType(x.FieldName)
                                });
                        }
                    });

                ContactCustomFields = newValueList.OrderBy(x => x.Priority).ToList();
            }
        }

        private CasePropertySet _caseRootPropertyForCaseCustomFields;
        public CasePropertySet CaseRootPropertyForCaseCustomFields
        {
            get { return _caseRootPropertyForCaseCustomFields; }
            set
            {
                _caseRootPropertyForCaseCustomFields = value;
                OnPropertyChanged();

                if (value == null)
                    return;

                var newValueList = new List<PropertyValueWithFieldNameEditViewModel>();
                //load values
                value.CaseProperties.ToList().ForEach(x =>
                    {
                        x.PropertyChanged += (s, e) =>
                        {
                            IsModified = true;
                        };

                        if (InnerItem.HasProperty(GetPropertyWithoutType(x.FieldName)))
                        {
                            var propertyValueType = InnerItem.GetPropertyValueType(GetPropertyWithoutType(x.FieldName));

                            var item = new CasePropertyValue
                                {
                                    Name = x.Name,
                                    Priority = x.Priority,
                                    CaseId = InnerItem.CaseId,
                                    ValueType = (int)propertyValueType
                                };

                            newValueList.Add(new PropertyValueWithFieldNameEditViewModel(item) { FieldName = x.FieldName });
                        }
                    });

                CaseCustomFields = newValueList.OrderBy(x => x.Priority).ToList();
            }
        }

        private List<PropertyValueWithFieldNameEditViewModel> _contactCustomFields;
        public List<PropertyValueWithFieldNameEditViewModel> ContactCustomFields
        {
            get { return _contactCustomFields; }
            set
            {
                _contactCustomFields = value;
                OnPropertyChanged();
            }
        }

        private List<PropertyValueWithFieldNameEditViewModel> _caseCustomFields;
        public List<PropertyValueWithFieldNameEditViewModel> CaseCustomFields
        {
            get { return _caseCustomFields; }
            set
            {
                _caseCustomFields = value;
                OnPropertyChanged();
            }
        }


        private List<CommunicationItem> _caseCommunicationItems = new List<CommunicationItem>();
        public List<CommunicationItem> CaseCommunicationItems
        {
            get { return _caseCommunicationItems; }
            set
            {
                _caseCommunicationItems = value;
                OnPropertyChanged();
            }
        }

        private List<Note> _caseNotes = new List<Note>();
        public List<Note> CaseNotes
        {
            get { return _caseNotes; }
            set
            {
                _caseNotes = value;
                OnPropertyChanged();
            }
        }

        private List<Note> _contactNotes = new List<Note>();
        public List<Note> ContactNotes
        {
            get { return _contactNotes; }
            set
            {
                _contactNotes = value;
                OnPropertyChanged();
            }
        }


        public bool IsReadOnlyActive
        {
            get
            {
                return InnerItem == null || InnerItem.Status != CaseStatus.Resolved.ToString();
            }
        }

        #endregion

        #region ICustomersDetailViewModel Members

        public Case InnerItem
        {
            get { return _innerItem; }
            private set
            {
                _innerItem = value;
                OnPropertyChanged();
                OnPropertyChanged("IsReadOnlyActive");
            }
        }

        public Contact CurrentCustomer
        {
            get { return _innerContact; }
            internal set
            {
                _innerContact = value;
                OnPropertyChanged();
            }
        }

        public Case OriginalItem
        {
            get { return _originalItem; }
            set
            {
                _originalItem = value;
                OnPropertyChanged();
            }
        }

        public Contact OriginalContact
        {
            get { return _originalContact; }
            set
            {
                _originalContact = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Case Properties

        public string Title
        {
            get { return (_originalItem.Title != null) ? _originalItem.Title : string.Empty; }
        }


        public CaseStatus? Status
        {
            get
            {
                CaseStatus result;
                if (Enum.TryParse(_originalItem.Status, out result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }


        public DateTime? LastModified
        {
            get { return _originalItem.LastModified; }
            set
            {
                _originalItem.LastModified = value;
                OnPropertyChanged("LastModified");
            }
        }


        public CaseChannel? Channel
        {
            get
            {
                CaseChannel result;
                if (Enum.TryParse(_originalItem.Channel, out result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Contact Meembers

        public string FullName
        {
            get { return _innerContact.FullName; }
        }

        #endregion

        #region IClosable Members

        public event EventHandler CloseViewRequestedEvent;

        private NavigationItem _navigationData;
        public NavigationItem NavigationData
        {
            get
            {
                return _navigationData ?? (_navigationData = new NavigationItem(InnerItem.CaseId, NavigationNames.HomeName,
                                                                                NavigationNames.MenuName, this));
            }
        }

        #endregion

        #region IMinimizable Command

        public event EventHandler MinimizableViewRequestedEvent = delegate { };

        public DelegateCommand MinimizeCommand { get; private set; }

        #endregion

        #region IOpenTracking Members

        public DelegateCommand OpenItemCommand { get; private set; }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private bool _isActive;

        #endregion

        #region ISupportAcceptChanges Members

        public DelegateCommand<object> SaveChangesCommand { get; set; }
        public DelegateCommand<object> CancelCommand { get; set; }
        public DelegateCommand<string> ShowImageCommand { get; set; }
        public DelegateCommand<string> CopyImageCommand { get; private set; }

        public InteractionRequest<Confirmation> CancelConfirmRequest { get; private set; }
        public InteractionRequest<Confirmation> SaveConfirmRequest { get; private set; }


        private async Task DoSaveChanges()
        {
            try
            {
                ShowLoadingAnimation = true;
                await Task.Run(() =>
                    {
                        DoSaveChangesAsync();
                        Repository.UnitOfWork.Commit();
                        UpdateCaseAndContactCommunicationItems();
                        UpdateCaseAndContactLabels();
                    });

                _originalContact = CurrentCustomer;
                _originalItem = InnerItem;

                _oldCasePropertyValues =
                                    Repository.CasePropertyValues.Where(x => x.CaseId == InnerItem.CaseId)
                                               .ToList();

                if (CaseActionState != CaseActionState.None)
                {
                    CaseActionState = CaseActionState.Open;
                }

                ContactActionState = ContactActionState.Open;

                OnUIThread(() =>
                    {
                        OnPropertyChanged("OriginalItem");
                        OnPropertyChanged("OriginalContact");
                        OnPropertyChanged("Status");
                        IsModified = false;
                        if (CaseActionState != CaseActionState.None)
                        {
                            _parentViewModel.Refresh();
                        }
                    });
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex, string.Format("An error occurred when trying to save changes: {0}".Localize(null, LocalizationScope.DefaultCategory), ex.InnerException.Message));
            }
            finally
            {
                ShowLoadingAnimation = false;
            }

        }



        private void DoSaveChangesAsync()
        {
            //case save actions
            UpdateCaseProxyCollection();

            //contact save actions
            UpdateContactProxyCollection();

            if (CaseActionState == CaseActionState.New)
            {
                InnerItem.Channel = CaseChannel.CommerceManager.ToString();
                Repository.Add(InnerItem);

            }
            if (CaseActionState != CaseActionState.None)
            {
                InnerItem.ContactId = CurrentCustomer.MemberId;
            }

            switch (ContactActionState)
            {
                case ContactActionState.New:
                    Repository.Add(CurrentCustomer);
                    break;
                case ContactActionState.Open:
                    Repository.Update(CurrentCustomer);
                    break;
            }
        }

        private void UpdateCaseProxyCollection()
        {
            if (CaseDetailViewModel == null || CaseActionState == CaseActionState.None)
                return;

            OnUIThread(() =>
                {
                    UpdateCaseCCs();
                    UpdateCasePropertyValues();
                });
        }

        private void UpdateContactProxyCollection()
        {
            if (CustomerDetailViewModel == null)
                return;

            OnUIThread(() =>
                {
                    UpdateContactEmails();
                    UpdateContactAddresses();
                    UpdateContactPhones();
                });
        }

        private void UpdateCaseAndContactCommunicationItems()
        {
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                GetCommunicationItemsFromCaseCommunications(repository);
                repository.UnitOfWork.Commit();
            }

            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                GetNotesToCaseFromCaseCommunications(repository);
                repository.UnitOfWork.Commit();
            }

            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                GetNotesToCustomerFromCustomerNotes(repository);
                repository.UnitOfWork.Commit();
            }

        }

        private void UpdateCaseAndContactLabels()
        {
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                UpdateCaseLabels(repository);
                UpdateContactLabels(repository);
            }
        }

        #endregion

        #region ISupportDelayInitialization Members

        public async void InitializeForOpen()
        {
            if (!_isItemsInitialized)
            {
                ShowLoadingAnimation = true;

                Repository = _repositoryFactory.GetRepositoryInstance();

                await Task.Run(() =>
                    {
                        //loading or creating contact
                        switch (ContactActionState)
                        {
                            case ContactActionState.New:
                                CurrentCustomer = new Contact();
                                break;

                            case ContactActionState.Open:
                                _originalContact =
                                    Repository.Members.Where(m => m.MemberId == _innerContact.MemberId)
                                               .OfType<Contact>()
                                               .Expand(c => c.Addresses)
                                               .Expand(c => c.Emails)
                                               .Expand(c => c.Labels)
                                    /*.Expand(c => c.Notes)*/.Expand(c => c.Phones).SingleOrDefault();

                                var contactNotes = Repository.Notes.Where(n => n.MemberId == CurrentCustomer.MemberId).ToList();
                                OnUIThread(() =>
                                {
                                    CurrentCustomer = _originalContact;
                                    ContactNotes = contactNotes;
                                });


                                if (CaseActionState == CaseActionState.None)
                                {
                                    IsCaseOptionSelected = false;
                                    IsCustomerOptionSelected = true;
                                }
                                break;
                        }

                        //loading or creating case
                        switch (CaseActionState)
                        {
                            case CaseActionState.None:
                                IsCaseOptionSelected = false;
                                IsCustomerOptionSelected = true;
                                break;

                            case CaseActionState.New:
                                InnerItem = new Case();
                                InnerItem.Status = CaseStatus.Open.ToString();
                                InnerItem.Number = UniqueNumberGenerator.GetUniqueNumber();

                                IsCaseOptionSelected = true;
                                IsCustomerOptionSelected = false;

                                break;

                            case CaseActionState.Open:

                                _originalItem =
                                    Repository.Cases.Where(c => c.CaseId == _originalItem.CaseId)
                                    /*.Expand(c => c.Notes)*/
                                                .Expand(c => c.Labels)
                                               .Expand(c => c.CaseCc).SingleOrDefault();



                                InnerItem = _originalItem.DeepClone(_entityFactory as IKnownSerializationTypes);

                                CaseCommunicationItems = Repository.CommunicationItems.Where(ci => ci.CaseId == InnerItem.CaseId).ToList();
                                CaseNotes = Repository.Notes.Where(n => n.CaseId == InnerItem.CaseId).ToList();

                                _oldCasePropertyValues =
                                    Repository.CasePropertyValues.Where(x => x.CaseId == InnerItem.CaseId)
                                               .ToList();

                                IsCaseOptionSelected = true;
                                IsCustomerOptionSelected = false;
                                break;
                        }
                    });

                await Task.Run(() =>
                    {
                        InitializeViewModels();
                        InitCommunicationControl();
                    });

                OnPropertyChanged("CaseCommunications");
                OnPropertyChanged("CustomerNotes");

                if (CaseCommunications != null && !CaseCommunications.IsInitialized)
                {
                    RefreshCaseCommunicationItems();
                }
                if (CustomerNotes != null && !CustomerNotes.IsInitialized)
                {
                    RefreshCustomerNotesItems();
                }


                SaveChangesCommand.RaiseCanExecuteChanged();
                ResolveAndSaveCommand.RaiseCanExecuteChanged();
                ProcessAndSaveCommand.RaiseCanExecuteChanged();

                _isItemsInitialized = true;
                IsModified = false;

                OnPropertyChanged("IsValidForSave");
                OnPropertyChanged("IsValid");
                OnPropertyChanged("InnerItem");
                OnPropertyChanged("CurrentCustomer");


                ShowLoadingAnimation = false;

                OnCaseOrContactStateChanged();
            }
        }

        private void ShowCreateCustomerDialog()
        {
            var itemVm =
                _wizardCustomerVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item",
                                                                                        new Contact
                                                                                            ()));

            var confirmation = new ConditionalConfirmation { Title = "Enter customer details".Localize(), Content = itemVm };

            if (CreateCustomerInteractionRequest != null)
            {
                CreateCustomerInteractionRequest.Raise(confirmation,
                                                       (x) =>
                                                       {
                                                           if (x.Confirmed)
                                                           {
                                                               var contactToAdd =
                                                                   (x.Content as
                                                                    ICreateCustomerDialogViewModel)
                                                                       .InnerItem;
                                                               var emailToAdd =
                                                                   (x.Content as
                                                                    ICreateCustomerDialogViewModel)
                                                                       .EmailForUserInput;
                                                               var phoneToAdd =
                                                                   (x.Content as
                                                                    ICreateCustomerDialogViewModel)
                                                                       .PhoneForUserInput;

                                                               if (emailToAdd.Validate())
                                                               {
                                                                   emailToAdd.Type = EmailType.Primary.ToString();
                                                                   contactToAdd.Emails.Add(
                                                                       emailToAdd);
                                                                   emailToAdd.MemberId =
                                                                       contactToAdd.MemberId;
                                                               }
                                                               if (phoneToAdd.Validate() &&
                                                                   !string.IsNullOrEmpty(
                                                                       phoneToAdd.Number))
                                                               {
                                                                   contactToAdd.Phones.Add(
                                                                       phoneToAdd);
                                                                   phoneToAdd.MemberId =
                                                                       contactToAdd.MemberId;
                                                               }

                                                               CurrentCustomer = contactToAdd;
                                                               _isNewContactInitialized = true;
                                                           }
                                                       });
            }
        }

        #endregion

        #region ViewModelBase overrides

        public override string DisplayName
        {
            get
            {
                StringBuilder result = new StringBuilder();
                if (CaseActionState == CaseActionState.None)
                {
                    result.Append(CurrentCustomer.FullName);
                }
                else
                {
                    if (InnerItem != null)
                    {
                        result.Append(InnerItem.Number);
                    }
                }

                return result.ToString();
            }
        }

        public override string IconSource
        {
            get { return "Customer"; }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                SolidColorBrush result =
                    (SolidColorBrush)Application.Current.TryFindResource("CustomerDetailMenuItemBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        #endregion

        #region Communication control

        public ICommunicationControlViewModel CaseCommunications { set; get; }
        public ICommunicationControlViewModel CustomerNotes { set; get; }

        private void InitCommunicationControl()
        {
            CaseCommunications = new CaseCommunicationControlViewModel(null, _knowledgeBaseGroupVmFactory, _authContext, _authorId, _authorName, this);
            CustomerNotes = new CustomerCaseControlViewModel(null, _knowledgeBaseGroupVmFactory, _authContext, _authorId, _authorName, this);
        }

        /// <summary>
        /// refresh customer notes
        /// </summary>
        private async void RefreshCustomerNotesItems()
        {
            var adaptor = new CommunicationAdaptor();

            try
            {
                CustomerNotes.IsInitialized = false;
                var notes = await RefreshCustomerNotesItemAsync(adaptor);
                OnUIThread(() =>
                {
                    CustomerNotes.AppendCommunucationItems(notes);
                    CustomerNotes.IsInitialized = true;
                    ShowLoadingAnimation = false;
                    IsModified = false;
                });

            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex, string.Format("An error occurred when trying to refresh contact notes: {0}".Localize(), ex.InnerException.Message));
            }
        }

        private Task<List<CommunicationItemViewModel>> RefreshCustomerNotesItemAsync(CommunicationAdaptor adaptor)
        {
            Task<List<CommunicationItemViewModel>> taskResult = Task.Run(() =>
                {
                    var result = new List<CommunicationItemViewModel>();
                    if (CurrentCustomer != null && ContactNotes.Count > 0)
                    {
                        result =
                            result.Union(
                                ContactNotes.Select(
                                    n =>
                                    adaptor.Note2NoteCommunicationViewModel(
                                        n.DeepClone(_entityFactory as IKnownSerializationTypes)))).ToList();
                    }
                    return result;
                });

            return taskResult;
        }

        /// <summary>
        /// refresh case communication Items (Public Reply)
        /// refresh case communication Items (Notes)
        /// </summary>
        private async void RefreshCaseCommunicationItems()
        {
            var adaptor = new CommunicationAdaptor();

            try
            {
                CaseCommunications.IsInitialized = false;
                var communicationItems = await RefreshCaseCommunicationItemsAsync(adaptor);

                OnUIThread(() =>
                    {
                        CaseCommunications.AppendCommunucationItems(communicationItems);
                        CaseCommunications.IsInitialized = true;
                        IsModified = false;
                    });

            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex, string.Format("An error occurred when trying to refresh case notes: {0}".Localize(), ex.InnerException.Message));
            }
        }

        private Task<List<CommunicationItemViewModel>> RefreshCaseCommunicationItemsAsync(CommunicationAdaptor adaptor)
        {
            Task<List<CommunicationItemViewModel>> taskResult = Task.Run(() =>
                {
                    var result = new List<CommunicationItemViewModel>();

                    // Append publicReplyItems
                    if (CaseCommunicationItems.Count > 0)
                    {
                        result = result.Union(CaseCommunicationItems
                                                       .Where(item => item is PublicReplyItem)
                                                       .Select(
                                                           item =>
                                                           adaptor.PublicReply2PublicReplyViewModel(
                                                               item.DeepClone(
                                                                   _entityFactory as IKnownSerializationTypes)
                                                               as PublicReplyItem)))
                                       .ToList();
                    }
                    // Append Notes
                    if (CaseNotes.Count > 0)
                    {
                        result =
                            result.Union(
                                CaseNotes.Select(
                                    n =>
                                    adaptor.Note2NoteCommunicationViewModel(
                                        n.DeepClone(_entityFactory as IKnownSerializationTypes)))).ToList();
                    }

                    return result;
                });

            return taskResult;
        }


        /// <summary>
        /// Add communication item to CurrentCase
        /// </summary>
        /// <returns></returns>
        private void GetCommunicationItemsFromCaseCommunications(ICustomerRepository repository)
        {
            var adaptor = new CommunicationAdaptor();

            if (CaseCommunications != null && CaseCommunications.Items != null)
            {
                var result = new List<CommunicationItem>();

                result = result.Union(CaseCommunications.Items
                                                        .Where(item => item.Type == CommunicationItemType.PublicReply)
                                                        .Where(item => item.State != CommunicationItemState.Deleted)
                                                        .Select(
                                                            item =>
                                                            adaptor.PublicReplyViewModel2PublicReplyItem(
                                                                item as CommunicationItemPublicReplyViewModel))).ToList();

                foreach (var item in result)
                {
                    var itemForUpdate =
                        CaseCommunicationItems.SingleOrDefault(
                            comItem => comItem.CommunicationItemId == item.CommunicationItemId);
                    if (itemForUpdate == null)
                    {
                        item.CaseId = InnerItem.CaseId;
                        CaseCommunicationItems.Add(item);
                        repository.Add(item);
                    }
                }

                //if messsage was writed in textbox, but Enter was not pressed
                //then create new publicReplyItem and save it;
                if (!string.IsNullOrEmpty(CaseCommunications.NewBody))
                {
                    var activeCommand = CaseCommunications.ToolBarCommmands.SingleOrDefault(c => c.IsActive);

                    if ((CommunicationItemType)activeCommand.CommandParametr == CommunicationItemType.PublicReply)
                    {
                        PublicReplyItem publicReplyItem = new PublicReplyItem();
                        publicReplyItem.AuthorName = _authorName;
                        publicReplyItem.Body = CaseCommunications.NewBody;
                        publicReplyItem.Title = InnerItem.Title;
                        publicReplyItem.Created =
                            (DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local)).ToUniversalTime();
                        publicReplyItem.LastModified = publicReplyItem.Created;
                        publicReplyItem.CaseId = InnerItem.CaseId;
                        CaseCommunicationItems.Add(publicReplyItem);
                        repository.Add(publicReplyItem);
                    }

                    OnUIThread(() =>
                        {
                            if (activeCommand != null)
                            {
                                activeCommand.Command.Execute(CaseCommunications.NewBody);
                            }
                        });
                }
            }
        }

        /// <summary>
        /// add notes to current customer grom CustomerNotesViewModel
        /// </summary>
        private void GetNotesToCustomerFromCustomerNotes(ICustomerRepository repository)
        {
            var adaptor = new CommunicationAdaptor();
            if (CustomerNotes != null && CustomerNotes.Items != null)
            {
                var result = new List<Note>();

                result = result.Union(CustomerNotes.Items
                                                   .Where(n => n.State != CommunicationItemState.Deleted)
                                                   .Select(n => adaptor.NoteCommunicationViewModel2Note(
                                                       n as CommunicationItemNoteViewModel))).ToList();

                foreach (var note in result)
                {
                    var noteForUpdate =
                        ContactNotes.SingleOrDefault(custNote => custNote.NoteId == note.NoteId);
                    if (noteForUpdate == null)
                    {
                        note.MemberId = CurrentCustomer.MemberId;
                        ContactNotes.Add(note);
                        repository.Add(note);
                    }
                }

                //if messsage was writed in textbox, but Enter was not pressed
                //then create new noteItem and save it;
                if (!string.IsNullOrEmpty(CustomerNotes.NewBody))
                {
                    var activeCommand = CustomerNotes.ToolBarCommmands.SingleOrDefault(c => c.IsActive);

                    if ((CommunicationItemType)activeCommand.CommandParametr == CommunicationItemType.Note)
                    {
                        var note = new Note
                            {
                                AuthorName = _authorName,
                                Body = CustomerNotes.NewBody,
                                Title = InnerItem.Title,
                                Created = (DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local)).ToUniversalTime(),
                                MemberId = CurrentCustomer.MemberId
                            };
                        note.LastModified = note.Created;
                        ContactNotes.Add(note);
                        repository.Add(note);
                    }

                    OnUIThread(() =>
                    {
                        if (activeCommand != null)
                        {
                            activeCommand.Command.Execute(CustomerNotes.NewBody);
                        }
                    });



                }
            }
        }

        /// <summary>
        /// add notes to InnerItem from CaseCommunicationViewModel
        /// </summary>
        private void GetNotesToCaseFromCaseCommunications(ICustomerRepository repository)
        {
            var adaptor = new CommunicationAdaptor();
            if (CaseCommunications != null && CaseCommunications.Items != null)
            {
                var result = new List<Note>();

                result = result.Union(CaseCommunications.Items
                                                        .Where(n => n.Type == CommunicationItemType.Note)
                                                        .Where(
                                                            n =>
                                                            n.State !=
                                                            CommunicationItemState.Deleted)
                                                        .Select(
                                                            n =>
                                                            adaptor.NoteCommunicationViewModel2Note(
                                                                n as CommunicationItemNoteViewModel))).ToList();

                foreach (var note in result)
                {
                    var noteForUpdate = CaseNotes.SingleOrDefault(caseNote => caseNote.NoteId == note.NoteId);
                    if (noteForUpdate == null)
                    {
                        note.CaseId = InnerItem.CaseId;
                        CaseNotes.Add(note);
                        repository.Add(note);
                    }
                }

                //if messsage was writed in textbox, but Enter was not pressed
                //then create new noteItem and save it;
                if (!string.IsNullOrEmpty(CaseCommunications.NewBody))
                {
                    var activeCommand = CaseCommunications.ToolBarCommmands.SingleOrDefault(c => c.IsActive);

                    if ((CommunicationItemType)activeCommand.CommandParametr == CommunicationItemType.Note)
                    {
                        var note = new Note
                            {
                                AuthorName = _authorName,
                                Body = CaseCommunications.NewBody,
                                Title = InnerItem.Title,
                                Created = (DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local)).ToUniversalTime(),
                                CaseId = InnerItem.CaseId
                            };
                        note.LastModified = note.Created;
                        CaseNotes.Add(note);
                        repository.Add(note);
                    }

                    OnUIThread(() =>
                    {
                        if (activeCommand != null)
                        {
                            activeCommand.Command.Execute(CaseCommunications.NewBody);
                        }
                    });
                }
            }
        }

        #endregion

        #region customer cases history

        public List<Case> CustomerCaseList { get; private set; }
        public DelegateCommand<Case> OpenCaseCommand { get; private set; }

        private void RiseOpenCaseInteractionRequest(Case item)
        {

        }

        #endregion

        #region PrivateMethods

        private void UpdateCaseCCs()
        {
            //delete old values
            var originalItems = InnerItem.CaseCc.ToList();
            foreach (var ccItem in originalItems)
            {
                if (
                    (CaseDetailViewModel as ITagControlExtended<Account>).CollectionFromTagControl.All(
                        i => (i.InnerItem as Account).AccountId != ccItem.AccountId))
                {
                    InnerItem.CaseCc.Remove(ccItem);
                    Repository.Attach(ccItem);
                    Repository.Remove(ccItem);
                }
            }

            //add new values
            foreach (var ccItem in (CaseDetailViewModel as ITagControlExtended<Account>).CollectionFromTagControl)
            {
                if (_originalItem.CaseCc.All(a => a.AccountId != (ccItem.InnerItem as Account).AccountId))
                {
                    if (!string.IsNullOrWhiteSpace((ccItem.InnerItem as Account).AccountId))
                    {
                        CaseCC ccToAdd = new CaseCC();
                        ccToAdd.AccountId = (ccItem.InnerItem as Account).AccountId.ToString();
                        InnerItem.CaseCc.Add(ccToAdd);
                    }
                }
            }
        }

        private void UpdateCasePropertyValues()
        {
            if (CaseDetailViewModel != null && CaseDetailViewModel.CaseTemplateValues != null)
            {
                // add new values
                foreach (var item in CaseDetailViewModel.CaseTemplateValues)
                {
                    if (_oldCasePropertyValues.All(x => x.PropertyValueId != item.InnerItem.PropertyValueId))
                    {
                        Repository.Add(item.InnerItem);
                    }
                    else
                    {
                        Repository.Attach(item.InnerItem);
                        Repository.Update(item.InnerItem);
                    }
                }

                // delete old values
                foreach (var item in _oldCasePropertyValues)
                {
                    if (
                        CaseDetailViewModel.CaseTemplateValues.All(
                            x => x.InnerItem.PropertyValueId != item.PropertyValueId))
                    {
                        Repository.Attach(item);
                        Repository.Remove(item);
                    }
                }


            }
        }

        private void UpdateCaseLabels(ICustomerRepository repository)
        {
            //delete old values
            var originalLabels = InnerItem.Labels.ToList();
            foreach (var labelItem in originalLabels)
            {
                if (CaseDetailViewModel.CaseLabelList.All(l => l.LabelId != labelItem.LabelId))
                {
                    InnerItem.Labels.Remove(labelItem);
                    repository.Attach(labelItem);
                    repository.Remove(labelItem);
                }
            }

            //add new values
            foreach (var labelItem in CaseDetailViewModel.CaseLabelList)
            {
                if (_originalItem.Labels.All(l => l.LabelId != labelItem.LabelId))
                {
                    labelItem.CaseId = InnerItem.CaseId;
                    repository.Add(labelItem);
                    InnerItem.Labels.Add(labelItem);
                }
            }

            repository.UnitOfWork.Commit();
        }

        private void UpdateContactLabels(ICustomerRepository repository)
        {
            //delete old values
            var originalValues = CurrentCustomer.Labels.ToList();
            foreach (var labelItem in originalValues)
            {
                if (CustomerDetailViewModel.ContactLabels.All(l => l.LabelId != labelItem.LabelId))
                {
                    if (CurrentCustomer == null)
                        break;
                    CurrentCustomer.Labels.Remove(labelItem);
                    repository.Attach(labelItem);
                    repository.Remove(labelItem);
                }
            }

            //add new values
            foreach (var labelItem in CustomerDetailViewModel.ContactLabels)
            {
                if (_originalContact.Labels.All(l => l.LabelId != labelItem.LabelId))
                {
                    if (CurrentCustomer == null)
                        break;

                    labelItem.MemberId = CurrentCustomer.MemberId;
                    repository.Add(labelItem);
                    CurrentCustomer.Labels.Add(labelItem);
                }
            }

            repository.UnitOfWork.Commit();
        }

        private void UpdateContactEmails()
        {
            //delete old values
            var originalValues = CurrentCustomer.Emails.ToList();
            foreach (var emailItem in originalValues)
            {
                if (CustomerDetailViewModel.ContactEmails.All(e => e.EmailId != emailItem.EmailId))
                {
                    CurrentCustomer.Emails.Remove(emailItem);
                    Repository.Attach(emailItem);
                    Repository.Remove(emailItem);
                }
            }

            //add new values
            foreach (var emailItem in CustomerDetailViewModel.ContactEmails)
            {
                if (_originalContact.Emails.All(e => e.EmailId != emailItem.EmailId))
                {
                    CurrentCustomer.Emails.Add(emailItem);
                }
            }
        }

        private void UpdateContactAddresses()
        {
            //delete old values
            var originalValues = CurrentCustomer.Addresses.ToList();
            foreach (var addressItem in originalValues)
            {
                if (CustomerDetailViewModel.ContactAddresses.All(a => a.AddressId != addressItem.AddressId))
                {
                    CurrentCustomer.Addresses.Remove(addressItem);
                    Repository.Attach(addressItem);
                    Repository.Remove(addressItem);
                }
            }

            //add new values
            foreach (var addressItem in CustomerDetailViewModel.ContactAddresses)
            {
                if (_originalContact.Addresses.All(a => a.AddressId != addressItem.AddressId))
                {
                    CurrentCustomer.Addresses.Add(addressItem);
                }
            }
        }

        private void UpdateContactPhones()
        {
            //delete old values
            var originalValues = CurrentCustomer.Phones.ToList();
            foreach (var phoneItem in originalValues)
            {
                if (CustomerDetailViewModel.ContactPhones.All(p => p.PhoneId != phoneItem.PhoneId))
                {
                    CurrentCustomer.Phones.Remove(phoneItem);
                    Repository.Attach(phoneItem);
                    Repository.Remove(phoneItem);
                }
            }

            //add new values
            foreach (var phoneItem in CustomerDetailViewModel.ContactPhones)
            {
                if (_originalContact.Phones.All(p => p.PhoneId != phoneItem.PhoneId))
                {
                    CurrentCustomer.Phones.Add(phoneItem);
                }
            }
        }

        private void OnCaseOrContactStateChanged()
        {
            if (Repository == null)
                Repository = _repositoryFactory.GetRepositoryInstance();

            switch (CaseActionState)
            {
                case CaseActionState.None:
                    break;
                case CaseActionState.New:
                    break;
                case CaseActionState.Open:
                    if (!Repository.IsAttachedTo(InnerItem))
                        Repository.Attach(InnerItem);
                    break;
            }

            switch (ContactActionState)
            {
                case ContactActionState.None:
                    break;
                case ContactActionState.New:
                    break;
                case ContactActionState.Open:
                    if (!Repository.IsAttachedTo(CurrentCustomer))
                        Repository.Attach(CurrentCustomer);
                    break;
            }
        }


        private string GetPropertyWithoutType(string fieldName)
        {
            int indexOfDot = fieldName.IndexOf(".");
            string result;

            if (indexOfDot != -1)
            {
                result = fieldName.Substring(indexOfDot + 1);
            }
            else
            {
                result = fieldName;
            }

            return result;
        }

        #endregion

    }


}