using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Interfaces
{
	public interface IDynamicContentItemViewModel : IViewModel
	{
		void Duplicate(IDynamicContentRepository repository);
	}
}
