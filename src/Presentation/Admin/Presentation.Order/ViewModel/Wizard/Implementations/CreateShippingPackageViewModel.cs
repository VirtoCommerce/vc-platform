using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
    public class CreateShippingPackageViewModel : WizardViewModelBare, ICreateShippingPackageViewModel
    {
        public CreateShippingPackageViewModel(IViewModelsFactory<IShippingPackageOverviewStepViewModel> overviewVmFactory, ShippingPackage item)
        {
            var itemParameter = new KeyValuePair<string, object>("item", item);
            RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
        }
    }
}
