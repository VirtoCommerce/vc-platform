using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces
{
	public interface IStoreViewModel : IViewModelDetailBase
    {
		Store InnerItem { get; set; }
    }
}
