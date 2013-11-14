using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Implementations
{
	public class CreateTaxCategoryViewModel : WizardContainerStepsViewModel, ICreateTaxCategoryViewModel
    {
        public CreateTaxCategoryViewModel(IViewModelsFactory<ITaxCategoryOverviewStepViewModel> overviewVmFactory, TaxCategory item)
        {
            var parameters = new[] {new KeyValuePair<string,object>("item", item), new KeyValuePair<string, object>("isWizardMode", true)};
            RegisterStep(overviewVmFactory.GetViewModelInstance(parameters));
        }

    }

    public class TaxCategoryOverviewStepViewModel : TaxCategoryViewModel, ITaxCategoryOverviewStepViewModel
    {
        public TaxCategoryOverviewStepViewModel(
			IRepositoryFactory<ICatalogRepository> repositoryFactory, 
			IOrderEntityFactory entityFactory, 
			TaxCategory item)
            : base(repositoryFactory, entityFactory, item)
        {
        }
    }
}
