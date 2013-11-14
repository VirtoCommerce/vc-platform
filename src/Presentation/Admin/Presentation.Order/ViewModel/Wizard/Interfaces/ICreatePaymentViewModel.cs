using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces
{
    public interface ICreatePaymentViewModel : IViewModel
    {
        PaymentMethod[] PaymentMethods { get; }
        PaymentMethod PaymentMethod { get; set; }

        decimal Amount { get; set; }
    }

    public interface IPaymentMethodStepViewModel : IWizardStep
    {
    }

    public interface IPaymentDetailsStepViewModel : IWizardStep
    {
    }
}
