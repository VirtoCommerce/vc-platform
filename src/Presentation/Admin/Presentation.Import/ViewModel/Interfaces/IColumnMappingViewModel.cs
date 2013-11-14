using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces
{
	public interface IColumnMappingViewModel: IViewModel
	{
		bool Validate();
	}
}
