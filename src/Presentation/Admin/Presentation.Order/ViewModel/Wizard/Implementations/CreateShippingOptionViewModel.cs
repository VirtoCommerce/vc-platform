using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
	public class CreateShippingOptionViewModel : WizardViewModelBare, ICreateShippingOptionViewModel
	{
		public CreateShippingOptionViewModel(
			IViewModelsFactory<IShippingOptionOverviewStepViewModel> overviewVmFactory,
			IViewModelsFactory<IShippingOptionPackagesStepViewModel> packagesVmFactory,
			ShippingOption item)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
			RegisterStep(packagesVmFactory.GetViewModelInstance(itemParameter));
		}

	}

	public class ShippingOptionOverviewStepViewModel : ShippingOptionViewModel, IShippingOptionOverviewStepViewModel
	{
		public ShippingOptionOverviewStepViewModel(
			IRepositoryFactory<IShippingRepository> repositoryFactory,
			IOrderEntityFactory entityFactory,
			ShippingOption item)
			: base(null, repositoryFactory, entityFactory, null, item)
		{
		}
	}

	public class ShippingOptionPackagesStepViewModel : ShippingOptionViewModel, IShippingOptionPackagesStepViewModel
	{
		public ShippingOptionPackagesStepViewModel(
			IViewModelsFactory<IShippingOptionAddShippingPackageViewModel> addPackageVmFactory,
			IRepositoryFactory<IShippingRepository> repositoryFactory,
			IOrderEntityFactory entityFactory,
			ShippingOption item,
			ICatalogRepository catalogRepository)
			: base(addPackageVmFactory, repositoryFactory, entityFactory, catalogRepository, item)
		{
		}

		public override string Description
		{
			get { return "Enter available Packages.".Localize(); }
		}
	}
}
