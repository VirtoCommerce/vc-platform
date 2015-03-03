using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Implementations;

namespace CommerceFoundation.UI.FunctionalTests.Taxes
{
	public class AuthenticationContext : IAuthenticationContext
	{
		public bool IsUserAuthenticated { get; private set; }
		public bool IsAdminUser { get; private set; }
		public string CurrentUserName { get; private set; }
		public string CurrentUserId { get; private set; }
		public string Token { get; private set; }
		public void UpdateToken()
		{
		}

		public bool Login(string userName, string password, string baseUrl)
		{
			return true;
		}

		public bool CheckPermission(string permission)
		{
			return true;
		}
	}

	public class RegionManager:IRegionManager
	{
		public IRegionManager CreateRegionManager()
		{
			throw new NotImplementedException();
		}

		public IRegionCollection Regions { get; private set; }
	}
	
	public class ViewModelTestScenarios : TaxesTestBase
	{
		public ViewModelTestScenarios()
		{
			Container.RegisterType<IAuthenticationContext, AuthenticationContext>();
			Container.RegisterType<ICreatePicklistWizardViewModel, CreatePicklistWizardViewModel>();
			
			Container.RegisterType<ICreatePicklistStepViewModel, CreatePicklistStepViewModel>();
			Container.RegisterType<IHomeSettingsViewModel, TaxSettingsViewModel>();

			Container.RegisterType<IRegionManager, RegionManager>();
			
			
			Container.RegisterType(typeof(IRepositoryFactory<>), typeof(UnityRepositoryFactory<>), new ContainerControlledLifetimeManager());
			
		}
	}
}
