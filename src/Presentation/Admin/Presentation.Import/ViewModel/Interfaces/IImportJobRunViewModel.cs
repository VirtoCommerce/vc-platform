using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces
{
	public interface IImportJobRunViewModel: IViewModel
	{
		bool Validate();
	}
}
