using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces
{
	public interface IAddParameterViewModel: IViewModel
    {
		JobParameter InnerItem { get; set; }
    }
}
