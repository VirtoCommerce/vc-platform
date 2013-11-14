using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces
{
    public interface ICreateShippingMethodViewModel : IViewModel
    {
    }

    public interface IShippingMethodOverviewStepViewModel : IWizardStep
    {
    }
    
    public interface IShippingMethodSettingsStepViewModel:IWizardStep
    {
    }

    public interface IShippingMethodParametersStepViewModel:IWizardStep
    {
    }
}
