using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Security.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Wizard.Implementations
{
	public class CreateAccountViewModel : WizardViewModelBare, ICreateAccountViewModel
	{
		public CreateAccountViewModel(IViewModelsFactory<IAccountOverviewStepViewModel> overviewVmFactory, IViewModelsFactory<IAccountRolesStepViewModel> rolesVmFactory, Account item)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
			RegisterStep(rolesVmFactory.GetViewModelInstance(itemParameter));
		}

		#region ICreateAccountViewModel Members

		public string Password
		{
			get { return AllRegisteredSteps.OfType<IAccountOverviewStepViewModel>().First().Password; }
		}

		#endregion
	}

	public class AccountOverviewStepViewModel : AccountViewModel, IAccountOverviewStepViewModel
	{
		public AccountOverviewStepViewModel(IRepositoryFactory<ISecurityRepository> repositoryFactory, ISecurityEntityFactory entityFactory, Account item)
			: base(repositoryFactory, entityFactory, item)
		{
		}

		#region WizardViewModelBase overrides

		private string _Password;
		public string Password
		{
			get { return _Password; }
			set { _Password = value; OnPropertyChanged(); OnIsValidChanged(); }
		}

		private string _Password1;
		public string Password1
		{
			get { return _Password1; }
			set { _Password1 = value; OnPropertyChanged(); OnIsValidChanged(); }
		}

		public override string Description
		{
			get { return "Enter Account details".Localize(); }
		}

		public override bool IsValid
		{
			get
			{
				bool retval;
				bool doNotifyChanges = false;
				InnerItem.Validate(doNotifyChanges);
				retval = InnerItem.Errors.Count == 0;
				InnerItem.Errors.Clear();

				retval = retval
					&& !string.IsNullOrWhiteSpace(Password) && Password == Password1;
				return retval;
			}
		}
		#endregion
	}

	public class AccountRolesStepViewModel : AccountViewModel, IAccountRolesStepViewModel
	{
		public AccountRolesStepViewModel(IRepositoryFactory<ISecurityRepository> repositoryFactory, ISecurityEntityFactory entityFactory, Account item)
			: base(repositoryFactory, entityFactory, item)
		{
		}

		#region WizardViewModelBase overrides

		public override string Description
		{
			get { return "Set Roles".Localize(); }
		}

		public override bool IsValid
		{
			get { return true; }
		}

		public override bool IsLast
		{
			get { return true; }
		}
		#endregion

		#region ViewModelDetailBase

		// Do custom initialize for step instead initialize all properties for detail ViewModel
		protected override void InitializePropertiesForViewing()
		{
			InitializeAvailableRoles();
		}

		#endregion

	}
}
