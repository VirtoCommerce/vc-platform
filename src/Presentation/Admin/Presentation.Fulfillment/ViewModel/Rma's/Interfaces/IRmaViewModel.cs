using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Interfaces
{
	public interface IRmaViewModel : IViewModel
	{
		RmaRequest InnerItem { get; set; }
	}
}
