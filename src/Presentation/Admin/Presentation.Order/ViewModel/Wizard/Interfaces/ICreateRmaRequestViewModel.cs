using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces
{
    public interface ICreateRmaRequestViewModel : IViewModel
    {
        InteractionRequest<Confirmation> ReturnItemConfirmRequest { get; }

        RmaRequest GetRmaRequest();

        bool IsPhysicalReturnRequired { get; set; }
        string RefundOption { get; set; }
    }

    public interface IRmaRequestReturnItemsStepViewModel : IWizardStep
    {
    }

    public interface IRmaRequestRefundStepViewModel : IWizardStep
    {
    }
}
