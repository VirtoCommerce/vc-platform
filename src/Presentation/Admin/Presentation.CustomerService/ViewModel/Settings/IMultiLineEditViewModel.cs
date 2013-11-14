using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings
{
	public interface IMultiLineEditViewModel : IViewModel
    {
		string InnerItem { get; set; }
    }
}
