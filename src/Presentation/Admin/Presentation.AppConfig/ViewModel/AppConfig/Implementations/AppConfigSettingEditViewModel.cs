using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.AppConfig.Infrastructure.Enumerations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations
{
    public class AppConfigSettingEditViewModel : ViewModelDetailAndWizardBase<Setting>, IAppConfigSettingEditViewModel
    {
        #region const

        public const string textShortText = "ShortText",
            textLongText = "LongText",
            textInteger = "Integer",
            textDecimal = "Decimal",
            textBoolean = "Boolean",
            textDateTime = "DateTime";

        #endregion

        #region Dependencies

        private readonly INavigationManager _navManager;
        private readonly IHomeSettingsViewModel _parent;
        private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;

        #endregion

        #region Constructor

        public AppConfigSettingEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory,
                                             IHomeSettingsViewModel parent, INavigationManager navManager, Setting item)
            : base(entityFactory, item, false)
        {
            _repositoryFactory = repositoryFactory;
            _navManager = navManager;
            _parent = parent;
            ViewTitle = new ViewTitleBase() { Title = "Setting", SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory) };
            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
            CommandInit();
        }

        protected AppConfigSettingEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, Setting item)
            : base(entityFactory, item, true)
        {
            _repositoryFactory = repositoryFactory;
            CommandInit();
        }

        private void CommandInit()
        {
            ItemAddCommand = new DelegateCommand(AddSettingValue, () => HasPermission() && CanAddSettingValue());
            ItemEditCommand = new DelegateCommand<SettingValue>(EditSettingValue, (x) => HasPermission() && SelectedSettingValue != null);
            ItemDeleteCommand = new DelegateCommand<SettingValue>(RemoveSettingValue, (x) => HasPermission() && SelectedSettingValue != null);

            RemoveConfirmRequest = new InteractionRequest<ConditionalConfirmation>();
        }

        #endregion

        #region ViewModelBase Members

        public override string IconSource
        {
            get
            {
                return "Icon_SystemJob";
            }
        }

        public override string DisplayName
        {
            get
            {
                return InnerItem != null ? InnerItem.Name : string.Empty;
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result =
                    (SolidColorBrush)Application.Current.TryFindResource("SettingsDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(OriginalItem.SettingId,
                                                            Configuration.NavigationNames.HomeName,
                                                            Configuration.NavigationNames.MenuName, this));
            }
        }

        #endregion

        #region ViewModelDetailAndWizardBase Members

        public override string ExceptionContextIdentity { get { return string.Format("Application Setting ({0})", DisplayName); } }

        protected override void GetRepository()
        {
            Repository = _repositoryFactory.GetRepositoryInstance();
        }

        protected override bool IsValidForSave()
        {
            return InnerItem.Validate();
        }

        /// <summary>
        /// Return RefusedConfirmation for Cancel Confirm dialog
        /// </summary>
        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes to Setting '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void LoadInnerItem()
        {
            var item =
                (Repository as IAppConfigRepository).Settings.Where(x => x.SettingId == OriginalItem.SettingId)
                    .Expand(s => s.SettingValues)
                    .SingleOrDefault();
            OnUIThread(() => { InnerItem = item; });
        }

        protected override void InitializePropertiesForViewing()
        {
            if (AvailableValueTypes == null)
            {
                OnUIThread(() =>
                    {
                        AvailableValueTypes = new string[] { textShortText, textLongText, textInteger, textDecimal, textBoolean, textDateTime };
                        IsValueTypeChangeable = AvailableValueTypes.All(x => x != InnerItem.SettingValueType);
                        UpdateInputControlsVisibility();
                        OnPropertyChanged("AvailableValueTypes");
                    });
            }
        }

        protected override void AfterSaveChangesUI()
        {
            if (_parent != null)
            {
                OriginalItem.InjectFrom<CloneInjection>(InnerItem);
                _parent.RefreshItem(OriginalItem);
            }
        }

        protected override void SetSubscriptionUI()
        {
            if (InnerItem.SettingValues != null)
            {
                InnerItem.SettingValues.CollectionChanged += ViewModel_PropertyChanged;
                InnerItem.SettingValues.ToList().ForEach(param => param.PropertyChanged += ViewModel_PropertyChanged);
            }
        }

        protected override void CloseSubscriptionUI()
        {
            if (InnerItem.SettingValues != null)
            {
                InnerItem.SettingValues.CollectionChanged -= ViewModel_PropertyChanged;
                InnerItem.SettingValues.ToList().ForEach(param => param.PropertyChanged -= ViewModel_PropertyChanged);
            }
        }

        protected override void OnViewModelPropertyChangedUI(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelPropertyChangedUI(sender, e);
            CanChangeValueType();
        }

        protected override void OnViewModelCollectionChangedUI(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnViewModelCollectionChangedUI(sender, e);
            if (ItemAddCommand != null)
            {
                ItemAddCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region IWizardStep Members

        public override bool IsLast
        {
            get
            {
                return this is IAppConfigSettingOverviewStepViewModel;
            }
        }

        #endregion

        #region IAppConfigSettingEditViewModel members

        public DelegateCommand ItemAddCommand { get; private set; }
        public DelegateCommand<SettingValue> ItemEditCommand { get; private set; }
        public DelegateCommand<SettingValue> ItemDeleteCommand { get; private set; }

        public InteractionRequest<ConditionalConfirmation> RemoveConfirmRequest { get; private set; }

        public bool IsSystemSetting
        {
            get
            {
                return !InnerItem.IsSystem || IsWizardMode;
            }
        }

        private bool _isValueTypeChangeable;
        public bool IsValueTypeChangeable
        {
            get { return _isValueTypeChangeable; }
            private set
            {
                _isValueTypeChangeable = value;
                OnPropertyChanged();
                if (ItemAddCommand != null)
                    ItemAddCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanEnterValue { get { return AvailableValueTypes.Any(x => x == InnerItem.SettingValueType); } }
        public string[] AvailableValueTypes { get; private set; }

        bool _isShortStringValue;
        public bool IsShortStringValue
        {
            get { return _isShortStringValue; }
            set
            {
                _isShortStringValue = value;
                OnPropertyChanged();
                if (value)
                    ValueType = Infrastructure.Enumerations.ValueType.ShortText;
            }
        }

        bool _isLongStringValue;
        public bool IsLongStringValue
        {
            get { return _isLongStringValue; }
            set
            {
                _isLongStringValue = value;
                OnPropertyChanged();
                if (value)
                    ValueType = Infrastructure.Enumerations.ValueType.LongText;
            }
        }

        bool _isDecimalValue;
        public bool IsDecimalValue
        {
            get { return _isDecimalValue; }
            set
            {
                _isDecimalValue = value;
                OnPropertyChanged();
                if (value)
                    ValueType = Infrastructure.Enumerations.ValueType.Decimal;
            }
        }

        bool _isIntegerValue;
        public bool IsIntegerValue
        {
            get { return _isIntegerValue; }
            set
            {
                _isIntegerValue = value;
                OnPropertyChanged();
                if (value)
                    ValueType = Infrastructure.Enumerations.ValueType.Integer;
            }
        }

        bool _isBooleanValue;
        public bool IsBooleanValue
        {
            get { return _isBooleanValue; }
            set
            {
                _isBooleanValue = value;
                OnPropertyChanged();
                if (value)
                    ValueType = Infrastructure.Enumerations.ValueType.Boolean;
            }
        }

        bool _isDateTimeValue;
        public bool IsDateTimeValue
        {
            get { return _isDateTimeValue; }
            set
            {
                _isDateTimeValue = value;
                OnPropertyChanged();
                if (value)
                    ValueType = Infrastructure.Enumerations.ValueType.DataTime;
            }
        }


        public Infrastructure.Enumerations.ValueType ValueType { get; private set; }


        public SettingValue _selectedSettingValue;
        public SettingValue SelectedSettingValue
        {
            get { return _selectedSettingValue; }
            set
            {
                _selectedSettingValue = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Public methods

        public void RaiseCanExecuteChanged()
        {
            ItemAddCommand.RaiseCanExecuteChanged();
            ItemEditCommand.RaiseCanExecuteChanged();
            ItemDeleteCommand.RaiseCanExecuteChanged();
        }

        public void UpdateInputControlsVisibility()
        {
            IsShortStringValue = InnerItem.SettingValueType == textShortText;
            IsLongStringValue = InnerItem.SettingValueType == textLongText;
            IsDecimalValue = InnerItem.SettingValueType == textDecimal;
            IsIntegerValue = InnerItem.SettingValueType == textInteger;
            IsBooleanValue = InnerItem.SettingValueType == textBoolean;
            IsDateTimeValue = InnerItem.SettingValueType == textDateTime;
            OnPropertyChanged("CanEnterValue");
        }
        #endregion

        #region Command Implementation

        private void AddSettingValue()
        {
            RaiseAddSettingRequest(OperationType.Add);
        }

        private bool CanAddSettingValue()
        {
            bool result = true;

            if (InnerItem.IsMultiValue == false)
            {
                result = InnerItem.SettingValues.Count != 1;
            }

            return result;
        }

        private void EditSettingValue(SettingValue item)
        {
            RaiseAddSettingRequest(OperationType.Edit);
        }

        private void RemoveSettingValue(SettingValue item)
        {
            RaiseRemoveSettingRequest();
        }

        private void RaiseAddSettingRequest(OperationType operationType)
        {
            if (CommonConfirmRequest != null)
            {
                var operType = operationType;
                var itemVM = new AddSettingValueViewModel(ValueType, operationType);
                ConditionalConfirmation confirmation = new ConditionalConfirmation();

                if (operationType == OperationType.Edit)
                {
                    switch (ValueType)
                    {
                        case Infrastructure.Enumerations.ValueType.Boolean:
                            itemVM.BooleanValue = SelectedSettingValue.BooleanValue;
                            break;
                        case Infrastructure.Enumerations.ValueType.DataTime:
                            itemVM.DateTimeValue = SelectedSettingValue.DateTimeValue;
                            break;
                        case Infrastructure.Enumerations.ValueType.Decimal:
                            itemVM.DecimalValue = SelectedSettingValue.DecimalValue;
                            break;
                        case Infrastructure.Enumerations.ValueType.Integer:
                            itemVM.IntegerValue = SelectedSettingValue.IntegerValue;
                            break;
                        case Infrastructure.Enumerations.ValueType.LongText:
                            itemVM.LongTextValue = SelectedSettingValue.LongTextValue;
                            break;
                        case Infrastructure.Enumerations.ValueType.ShortText:
                            itemVM.ShortTextValue = SelectedSettingValue.ShortTextValue;
                            break;
                    }
                    confirmation.Title = "Edit setting value".Localize();
                }
                else
                {
                    confirmation.Title = "Add setting value".Localize();
                }

                confirmation.Content = itemVM;

                CommonConfirmRequest.Raise(confirmation,
                    (x) =>
                    {
                        if (x.Confirmed)
                        {
                            var vm = x.Content as AddSettingValueViewModel;

                            if (operType == OperationType.Add)
                            {
                                SettingValue settingToAdd = new SettingValue();

                                GetValueFromAddEditDialog(vm, ref settingToAdd);

                                this.InnerItem.SettingValues.Add(settingToAdd);
                            }
                            else
                            {
                                SettingValue settingToEdit = SelectedSettingValue;
                                GetValueFromAddEditDialog(vm, ref settingToEdit);
                            }

                            OnPropertyChanged("IsValid");
                            ItemAddCommand.RaiseCanExecuteChanged();
                            OnIsValidChanged();
                        }
                    });

            }
        }

        private void GetValueFromAddEditDialog(AddSettingValueViewModel vm, ref SettingValue settingValue)
        {
            switch (vm.ValueType)
            {
                case Infrastructure.Enumerations.ValueType.Boolean:
                    settingValue.BooleanValue = vm.BooleanValue;
                    settingValue.ValueType = textBoolean;
                    break;
                case Infrastructure.Enumerations.ValueType.DataTime:
                    settingValue.DateTimeValue = vm.DateTimeValue;
                    settingValue.ValueType = textDateTime;
                    break;
                case Infrastructure.Enumerations.ValueType.Decimal:
                    settingValue.DecimalValue = vm.DecimalValue;
                    settingValue.ValueType = textDecimal;
                    break;
                case Infrastructure.Enumerations.ValueType.Integer:
                    settingValue.IntegerValue = vm.IntegerValue;
                    settingValue.ValueType = textInteger;
                    break;
                case Infrastructure.Enumerations.ValueType.LongText:
                    settingValue.LongTextValue = vm.LongTextValue;
                    settingValue.ValueType = textLongText;
                    break;
                case Infrastructure.Enumerations.ValueType.ShortText:
                    settingValue.ShortTextValue = vm.ShortTextValue;
                    settingValue.ValueType = textShortText;
                    break;
            }

        }

        private void RaiseRemoveSettingRequest()
        {
            var settingToRemove = SelectedSettingValue;

            if (RemoveConfirmRequest != null)
            {
                var confirmation = new ConditionalConfirmation();
                confirmation.Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory);
                confirmation.Content = "Remove this setting value?".Localize();

                RemoveConfirmRequest.Raise(confirmation,
                    (x) =>
                    {
                        if (x.Confirmed)
                        {
                            var settingFromInnerItem =
                                InnerItem.SettingValues.SingleOrDefault(
                                    s => s.SettingValueId == settingToRemove.SettingValueId);
                            if (settingFromInnerItem != null)
                            {
                                settingFromInnerItem.PropertyChanged -= ViewModel_PropertyChanged;
                                InnerItem.SettingValues.Remove(settingFromInnerItem);
                            }
                        }
                        OnPropertyChanged("IsValid");
                        ItemAddCommand.RaiseCanExecuteChanged();
                    });
            }
        }

        #endregion

        #region Private Methods

        private void CanChangeValueType()
        {
            if (InnerItem != null)
            {
                if (InnerItem.SettingValues.Count > 0)
                {
                    IsValueTypeChangeable = false;
                }
                else
                {
                    IsValueTypeChangeable = true;
                }
            }

        }

        private bool IsSettingValuesFilled()
        {
            bool result = false;

            if (InnerItem.IsMultiValue)
            {
                if (InnerItem.SettingValues.Count > 0)
                {
                    result = true;
                }
            }
            else
            {
                if (InnerItem.SettingValues.Count == 1)
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

    }
}
