using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Interfaces
{
    public interface IJurisdictionGroupViewModel : IViewModel
    {
        JurisdictionGroup InnerItem { get; }
	    void UptadeOfPaymentShipping();
    }
}
