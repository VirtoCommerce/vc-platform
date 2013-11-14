using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces
{
    public interface IImportJobViewModel : IViewModel
	{
		ImportJob InnerItem { get; }
        void Delete();
		ImportEntityType[] EntityImporters { get; }
	}
}
