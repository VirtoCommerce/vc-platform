using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Security.Model;
using VirtoCommerce.ManagementClient.Security.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Interfaces;


namespace VirtoCommerce.ManagementClient.Security.ViewModel.Implementations
{
    public class RoleViewModel : ViewModelDetailAndWizardBase<Role>, IRoleViewModel
    {
        #region Dependencies

        private readonly INavigationManager _navManager;
        private readonly IRepositoryFactory<ISecurityRepository> _repositoryFactory;

        #endregion

        static readonly StringDictionary predefinedPermissionGroupNames = new StringDictionary();

        static RoleViewModel()
        {
            predefinedPermissionGroupNames.Add("catalog", "Catalog");
            predefinedPermissionGroupNames.Add("customers", "Customer Service");
            predefinedPermissionGroupNames.Add("orders", "Orders");
            predefinedPermissionGroupNames.Add("pricing", "Pricing");
            predefinedPermissionGroupNames.Add("marketing", "Marketing");
            predefinedPermissionGroupNames.Add("fulfillment", "Fulfillment");
            predefinedPermissionGroupNames.Add("security", "Users");
            predefinedPermissionGroupNames.Add("reporting", "Reporting");
            predefinedPermissionGroupNames.Add("stores", "Shopper");
        }

        #region Constructor

        public RoleViewModel(IRepositoryFactory<ISecurityRepository> repositoryFactory, ISecurityEntityFactory entityFactory, INavigationManager navManager, Role item)
            : base(entityFactory, item, false)
        {
            _repositoryFactory = repositoryFactory;
            _navManager = navManager;
            ViewTitle = new ViewTitleBase()
                {
                    Title = "Role",
                    SubTitle =
                        (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
                };
            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
            InitCommands();
        }

        protected RoleViewModel(IRepositoryFactory<ISecurityRepository> repositoryFactory, ISecurityEntityFactory entityFactory, Role item)
            : base(entityFactory, item, true)
        {
            _repositoryFactory = repositoryFactory;
            InitCommands();
        }

        #endregion

        #region ViewModelBase members

        public override sealed string DisplayName
        {
            get
            {
                return InnerItem == null ? "" : InnerItem.Name;
            }
        }

        public override sealed string IconSource
        {
            get
            {
                return "";
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result =
                    (SolidColorBrush)Application.Current.TryFindResource("SecurityDetailMenuItemBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.RoleId),
                                                            NavigationNames.HomeName, NavigationNames.MenuName, this));
            }
        }


        #endregion

        #region ViewModelDetailAndWizardBase Members

        public override string ExceptionContextIdentity { get { return string.Format("Role ({0})".Localize(), DisplayName); } }

        protected override void GetRepository()
        {
            Repository = _repositoryFactory.GetRepositoryInstance();
        }

        protected override bool HasPermission()
        {
            return true; //todo
        }

        protected override bool IsValidForSave()
        {
            InnerItem.Validate();
            return !InnerItem.Errors.Any();
        }

        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes \'{0}\'?".Localize(null, LocalizationScope.DefaultCategory), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void InitializePropertiesForViewing()
        {
            if (this is IRoleOverviewStepViewModel || !IsWizardMode)
            {
                if (AllAvailablePermissionGroupViewModels == null)
                {
                    AllAvailablePermissionGroupViewModels = GetAllAvailablePermissionGroupViewModels();
                    foreach (var originalItemPermission in InnerItem.RolePermissions)
                    {
                        var permVM = (from permGroupVm in AllAvailablePermissionGroupViewModels
                                      from perm in permGroupVm.Permissions
                                      where perm.InnerItem.PermissionId == originalItemPermission.PermissionId
                                      select perm).SingleOrDefault();
                        if (permVM != null)
                        {
                            permVM.IsVisible = false;
                        }
                    }

                    OnPropertyChanged("AllAvailablePermissionGroupViewModels");
                }

                if (CurrentPermissionGroupViewModels == null)
                {
                    CurrentPermissionGroupViewModels = GetAllAvailablePermissionGroupViewModels();

                    foreach (var permissionGroupViewModel in CurrentPermissionGroupViewModels)
                    {
                        foreach (var permission in permissionGroupViewModel.Permissions)
                        {
                            permission.IsVisible = false;
                        }
                    }

                    foreach (var originalItemPermission in InnerItem.RolePermissions)
                    {
                        var permVM = (from permGroupVm in CurrentPermissionGroupViewModels
                                      from perm in permGroupVm.Permissions
                                      where perm.InnerItem.PermissionId == originalItemPermission.PermissionId
                                      select perm).SingleOrDefault();

                        if (permVM != null)
                        {
                            permVM.IsVisible = true;
                        }
                    }

                    OnPropertyChanged("CurrentPermissionGroupViewModels");
                }
            }
        }

        protected override void LoadInnerItem()
        {
            try
            {
                var item =
                    (Repository as ISecurityRepository).Roles.Where(r => r.RoleId == OriginalItem.RoleId)
                        .Expand(r => r.RolePermissions)
                        .SingleOrDefault();
                ;
                OnUIThread(() => InnerItem = item);
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex, string.Format("An error occurred when trying to load {0}".Localize(null, LocalizationScope.DefaultCategory), ExceptionContextIdentity));
            }
        }

        protected override void BeforeSaveChanges()
        {
            var rolePermWithoutPermissions =
                (Repository as ISecurityRepository).RolePermissions.Where(
                    rp => rp.RoleId == null && rp.PermissionId == null);

            foreach (var rolePermWithoutPermission in rolePermWithoutPermissions)
            {
                if (!Repository.IsAttachedTo(rolePermWithoutPermission))
                {
                    Repository.Attach(rolePermWithoutPermission);
                }
                Repository.Remove(rolePermWithoutPermission);
            }
        }

        protected override void AfterSaveChangesUI()
        {
            OriginalItem.InjectFrom<CloneInjection>(InnerItem);
        }

        protected override void SetSubscriptionUI()
        {
            InnerItem.RolePermissions.CollectionChanged += ViewModel_PropertyChanged;
            InnerItem.RoleAssignments.CollectionChanged += ViewModel_PropertyChanged;
        }

        protected override void CloseSubscriptionUI()
        {
            InnerItem.RolePermissions.CollectionChanged -= ViewModel_PropertyChanged;
            InnerItem.RoleAssignments.CollectionChanged -= ViewModel_PropertyChanged;
        }

        #endregion

        #region IMultiSelectControlCommands

        public void SelectItem(object selectedObj)
        {
            if (selectedObj != null)
            {
                if (selectedObj is PermissionGroupViewModel)
                {
                    var itemVM = (PermissionGroupViewModel)selectedObj;
                    itemVM.Permissions.ForEach(x => SelectPermissionViewModel(x));
                }
                else
                {
                    if (selectedObj is PermissionViewModel)
                    {
                        var itemVM = ((PermissionViewModel)selectedObj);
                        SelectPermissionViewModel(itemVM);
                    }

                }
            }
        }

        private void SelectPermissionViewModel(PermissionViewModel permVM)
        {
            permVM.IsVisible = false;

            var permVmFromCurrent = (from permGroupVm in CurrentPermissionGroupViewModels
                                     from permVm in permGroupVm.Permissions
                                     where permVm.InnerItem.PermissionId == permVM.InnerItem.PermissionId
                                     select permVm).SingleOrDefault();

            if (permVmFromCurrent != null)
            {
                permVmFromCurrent.IsVisible = true;
                permVmFromCurrent.Parent.RefreshIsVisibleProperty();
            }

            var item = permVM.InnerItem;
            AddPermissionToRole(item);
        }

        private void AddPermissionToRole(Permission permToAdd)
        {
            RolePermission perm = new RolePermission
            {
                PermissionId = permToAdd.PermissionId,
                RoleId = InnerItem.RoleId
            };

            InnerItem.RolePermissions.Add(perm);
            //			OnViewModelPropertyChangedUI(null, null);
        }

        public void SelectAllItems()
        {
            var itemVMList = AllAvailablePermissionGroupViewModels
                .SelectMany(x => x.Permissions).ToList();

            itemVMList.ForEach(x => SelectItem(x));
        }

        public void UnSelectItem(object selectedObj)
        {
            if (selectedObj != null)
            {
                if (selectedObj is PermissionGroupViewModel)
                {
                    var itemVM = (PermissionGroupViewModel)selectedObj;

                    itemVM.Permissions.ForEach(x => UnSelectPermissionViewModel(x));
                }
                else
                    if (selectedObj is PermissionViewModel)
                    {
                        var itemVM = ((PermissionViewModel)selectedObj);

                        UnSelectPermissionViewModel(itemVM);
                    }
            }
        }

        private void UnSelectPermissionViewModel(PermissionViewModel permVM)
        {
            permVM.IsVisible = false;

            var permVmFromAvail = (from permGroupVm in AllAvailablePermissionGroupViewModels
                                   from permVm in permGroupVm.Permissions
                                   where permVm.InnerItem.PermissionId == permVM.InnerItem.PermissionId
                                   select permVm).SingleOrDefault();

            if (permVmFromAvail != null)
            {
                permVmFromAvail.IsVisible = true;
            }

            var item = permVM.InnerItem;
            RemovePermissionFromRole(item);
        }

        private void RemovePermissionFromRole(Permission permToRemove)
        {
            var rolePermItem = InnerItem.RolePermissions.SingleOrDefault(x => x.PermissionId == permToRemove.PermissionId && x.RoleId == InnerItem.RoleId);

            if (rolePermItem != null)
            {

                InnerItem.RolePermissions.Remove(rolePermItem);
                rolePermItem.Permission = null;
                rolePermItem.PermissionId = null;
                rolePermItem.Role = null;
                rolePermItem.RoleId = null;
                IsModified = true;
            }
        }

        public void UnSelectAllItems()
        {
            CurrentPermissionGroupViewModels.ToList().ForEach(x => UnSelectItem(x));
        }


        #endregion

        #region IRoleViewModel

        public DelegateCommand<object> SelectItemCommand { get; private set; }
        public DelegateCommand SelectAllItemsCommand { get; private set; }
        public DelegateCommand<object> UnSelectItemCommand { get; private set; }
        public DelegateCommand UnSelectAllItemsCommand { get; private set; }

        public PermissionGroupViewModel[] AllAvailablePermissionGroupViewModels { get; private set; }

        public PermissionGroupViewModel[] CurrentPermissionGroupViewModels { get; private set; }

        protected override bool BeforeDelete()
        {
            var repository = Repository as ISecurityRepository;
            var rolePermissions = repository.RolePermissions.Where(rp => rp.RoleId == InnerItem.RoleId).ToArray();
            foreach (var rolePermission in rolePermissions)
            {
                repository.Attach(rolePermission);
                repository.Remove(rolePermission);
            }

            var roleAssignments = repository.RoleAssignments.Where(ra => ra.RoleId == InnerItem.RoleId).ToArray();
            foreach (var roleAssignment in roleAssignments)
            {
                repository.Attach(roleAssignment);
                repository.Remove(roleAssignment);
            }
            return true;
        }

        #endregion

        #region IWizardStep Members

        public override bool IsValid
        {
            get
            {
                var retval = true;
                if (this is IRoleOverviewStepViewModel)
                {
                    bool doNotifyChanges = false;
                    InnerItem.Validate(doNotifyChanges);
                    retval = InnerItem.Errors.Count == 0;
                    InnerItem.Errors.Clear();
                }

                return retval;
            }
        }

        public override bool IsLast
        {
            get
            {
                return this is IRoleOverviewStepViewModel;
            }
        }

        public override string Description
        {
            get
            {
                return "Enter Role information.".Localize();
            }
        }
        #endregion

        #region private members

        private void InitCommands()
        {
            SelectItemCommand = new DelegateCommand<object>(SelectItem);
            SelectAllItemsCommand = new DelegateCommand(SelectAllItems);
            UnSelectItemCommand = new DelegateCommand<object>(UnSelectItem);
            UnSelectAllItemsCommand = new DelegateCommand(UnSelectAllItems);
        }

        private PermissionGroupViewModel[] GetAllAvailablePermissionGroupViewModels()
        {
            PermissionGroupViewModel[] retVal = null;
            try
            {
                using (var repository = _repositoryFactory.GetRepositoryInstance())
                {
                    retVal = repository.Permissions.ToArray()
                        .GroupBy(x => GetGroupName(x.PermissionId))
                        .Select(x => CreatePermissionGroupViewModel(x.Key, x.ToArray()))
                        .OrderBy(x => x.InnerItem.DisplayName)
                        .ToArray();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex, string.Format("An error occurred when trying to load Permissions for: {0}".Localize(), ExceptionContextIdentity));
            }
            return retVal;
        }

        private string GetGroupName(string permissionId)
        {
            var result = permissionId;

            // all Permissions ending with ":config" goes to group "Settings"
            if (permissionId.EndsWith(":config"))
            {
                result = "Settings";
            }
            else
            {
                // group name is the begining of permissionId until ":"
                var idx = permissionId.IndexOf(':');
                if (idx > 0)
                {
                    result = permissionId.Substring(0, idx);
                    if (predefinedPermissionGroupNames.ContainsKey(result))
                    {
                        result = predefinedPermissionGroupNames[result];
                    }
                }
            }

            return result;
        }

        private PermissionGroupViewModel CreatePermissionGroupViewModel(string name, Permission[] permissions)
        {
            return new PermissionGroupViewModel(new PermissionGroup(name, permissions));
        }

        #endregion
    }
}
