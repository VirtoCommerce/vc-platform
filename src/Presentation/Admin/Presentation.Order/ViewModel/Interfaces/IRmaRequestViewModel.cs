using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
    public interface IRmaRequestViewModel : IViewModel
    {
        DelegateCommand RmaRequestCancelCommand { get; }
        DelegateCommand RmaRequestCompleteCommand { get; }
        
        DelegateCommand ExchangeOrderCreateCommand { get; }
        DelegateCommand ExchangeOrderViewCommand { get; }
        
        RmaRequest CurrentRmaRequest { get; }
		bool IsExchangeOrderCreateShow { get; }
		string CurrentStatusText { get; }
    }
}
