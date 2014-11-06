using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Implementations
{
	public class AccountViewModel : ViewModelDetailAndWizardBase<Account>, IAccountViewModel, IMultiSelectControlCommands
	{
		#region Dependencies

		private readonly IRepositoryFactory<ISecurityRepository> _repositoryFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly INavigationManager _navManager;
		private readonly IViewModelsFactory<IPasswordChangeViewModel> _passwordVmFactory;
		private readonly ISecurityService _securityService;

		#endregion

		#region Constructor

		/// <summary>
		/// public. For viewing
		/// </summary>
		public AccountViewModel(
			IRepositoryFactory<ISecurityRepository> repositoryFactory,
			ISecurityEntityFactory entityFactory,
			IAuthenticationContext authContext,
			INavigationManager navManager,
			IViewModelsFactory<IPasswordChangeViewModel> passwordVmFactory,
			Account item,
			ISecurityService securityService)
			: base(entityFactory, item, false)
		{
			_securityService = securityService;
			_repositoryFactory = repositoryFactory;
			_authContext = authContext;
			_navManager = navManager;
			_passwordVmFactory = passwordVmFactory;
			ViewTitle = new ViewTitleBase()
				{
                    Title = "Account",
					SubTitle = (item != null && !string.IsNullOrEmpty(item.UserName)) ? item.UserName.ToUpper(CultureInfo.InvariantCulture) : ""
				};

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			InitCommands();
		}

		/// <summary>
		/// protected. For a step
		/// </summary>
		protected AccountViewModel(IRepositoryFactory<ISecurityRepository> repositoryFactory, ISecurityEntityFactory entityFactory, Account item)
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
				return OriginalItem == null ? string.Empty : OriginalItem.UserName;
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.AccountId),
															NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}


		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Account ({0})", DisplayName); } }

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
				Content = string.Format("Save changes to \'{0}\'?".Localize(null, LocalizationScope.DefaultCategory), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void InitializePropertiesForViewing()
		{
			if (!IsWizardMode)
			{
				InitializeAvailableRoles();
			}
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as ISecurityRepository).Accounts.Where(x => x.AccountId == OriginalItem.AccountId)
												   .Expand("RoleAssignments/Role")
												   .SingleOrDefault();
			OnUIThread(() => InnerItem = item);
		}

		protected override void AfterSaveChangesUI()
		{
			OriginalItem.InjectFrom<CloneInjection>(InnerItem);
		}

		protected override void SetSubscriptionUI()
		{
			InnerItem.RoleAssignments.CollectionChanged += ViewModel_PropertyChanged;
		}

		protected override void CloseSubscriptionUI()
		{
			InnerItem.RoleAssignments.CollectionChanged -= ViewModel_PropertyChanged;
		}

		#endregion

		#region IMultiSelectControlCommands

		public void SelectItem(object selectedObj)
		{
			var selectedItem = (Role)selectedObj;
			var item = (RoleAssignment)EntityFactory.CreateEntityForType(typeof(RoleAssignment));
			item.AccountId = InnerItem.AccountId;
			item.Role = selectedItem;
			item.RoleId = selectedItem.RoleId;

			OnUIThread(() => InnerItem.RoleAssignments.Add(item));
		}

		public void SelectAllItems(ICollectionView availableItemsCollectionView)
		{
			var itemsList = availableItemsCollectionView.Cast<Role>().ToList();
			itemsList.ForEach(SelectItem);
		}

		public void UnSelectItem(object selectedObj)
		{
			var selectedItem = selectedObj as RoleAssignment;
			var item = InnerItem.RoleAssignments.First(x => x.RoleId == selectedItem.RoleId);
			OnUIThread(() => InnerItem.RoleAssignments.Remove(item));
		}

		public void UnSelectAllItems(System.Collections.IList currentListItems)
		{
			InnerItem.RoleAssignments.ToList().ForEach(UnSelectItem);
		}
		#endregion

		#region IWizardStep Members

		public override bool IsLast
		{
			get { return false; }
		}

		public override string Description
		{
			get
			{
				return "Enter User general information.".Localize();
			}
		}
		#endregion

		#region Public Properties

		public bool IsAdministrator
		{
			get { return InnerItem.RegisterType == (int)RegisterType.Administrator; }
			set { InnerItem.RegisterType = value ? (int)RegisterType.Administrator : (int)RegisterType.SiteAdministrator; }
		}

		public bool IsAdministratorCheckable
		{
			get
			{
				return _authContext != null && InnerItem.MemberId != _authContext.CurrentUserId;
			}
		}

		public DelegateCommand ApproveCommand { get; private set; }
		public DelegateCommand RejectCommand { get; private set; }
		public DelegateCommand PasswordChangeCommand { get; private set; }
		public DelegateCommand PasswordResetCommand { get; private set; }

		public Role[] AllAvailableRoles { get; private set; }

		#endregion

		#region private members

		private void InitCommands()
		{
			ApproveCommand = new DelegateCommand(RaiseApproveRequest, () => InnerItem.AccountState != (int)AccountState.Approved);
			RejectCommand = new DelegateCommand(RaiseRejectRequest, () => (InnerItem.AccountState == (int)AccountState.Approved || InnerItem.AccountState == (int)AccountState.PendingApproval)
																			&& InnerItem.MemberId != _authContext.CurrentUserId);
			PasswordChangeCommand = new DelegateCommand(RaisePasswordChangeRequest, () => InnerItem.MemberId == _authContext.CurrentUserId);
			PasswordResetCommand = new DelegateCommand(RaisePasswordResetRequest, () => _authContext.IsAdminUser && InnerItem.MemberId != _authContext.CurrentUserId);
		}

		private bool FilterRoles(object item)
		{
			var result = false;
			if (item is Role)
			{
				var itemTyped = (Role)item;
				result = InnerItem.RoleAssignments.All(x => x.RoleId != itemTyped.RoleId);
			}
			return result;
		}

		private void RaiseApproveRequest()
		{
			InnerItem.AccountState = (int)AccountState.Approved;

			ApproveCommand.RaiseCanExecuteChanged();
			RejectCommand.RaiseCanExecuteChanged();
		}

		private void RaiseRejectRequest()
		{
			InnerItem.AccountState = (int)AccountState.Rejected;

			ApproveCommand.RaiseCanExecuteChanged();
			RejectCommand.RaiseCanExecuteChanged();
		}

		private void RaisePasswordChangeRequest()
		{
			RaisePasswordChangeRequest("Change Password".Localize(), true);
		}

		private void RaisePasswordResetRequest()
		{
			RaisePasswordChangeRequest("Reset Password".Localize(), false);
		}

		private void RaisePasswordChangeRequest(string title, bool isPasswordChanging)
		{
			var itemVM = _passwordVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("isPasswordChanging", isPasswordChanging)
				);
			var confirmation = new ConditionalConfirmation { Title = title, Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					try
					{

						if (isPasswordChanging)
							_securityService.ChangePasswordAsync(InnerItem.UserName, itemVM.OldPassword, itemVM.Password);
						else
							_securityService.ResetPasswordAsync(InnerItem.UserName, itemVM.Password);
					}
					catch (Exception ex)
					{
						var message = string.Format(isPasswordChanging ?
							"An error occurred when trying to change password: {0}".Localize() :
							"An error occurred when trying to reset password: {0}".Localize(), ex.InnerException.Message);

						ShowErrorDialog(ex, message);
					}

				}
			});
		}

		#endregion

		#region Initialize and Update

		protected void InitializeAvailableRoles()
		{
			AllAvailableRoles = (Repository as ISecurityRepository).Roles.ToArray();
			OnUIThread(() =>
			{
				var defaultView = CollectionViewSource.GetDefaultView(AllAvailableRoles);
				defaultView.Filter = FilterRoles;
				OnPropertyChanged("AllAvailableRoles");
			});
		}

		#endregion
	}
}
