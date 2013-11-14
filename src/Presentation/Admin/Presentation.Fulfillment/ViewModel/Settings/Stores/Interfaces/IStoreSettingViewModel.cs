using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces
{
    public interface IStoreSettingViewModel : IViewModel
    {
		StoreSetting InnerItem { get; set; }
    }
}
