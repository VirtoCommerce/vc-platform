using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces
{
    public interface ICreateExchangeViewModel : ICreateRmaRequestViewModel
    {
        object ShippingAddress { get; set; }
        VirtoCommerce.Foundation.Orders.Model.ShippingMethod.ShippingMethod ShippingMethod { get; set; }
    }

    public interface IExchangeOrderStepViewModel : IWizardStep
    {
    }
}
