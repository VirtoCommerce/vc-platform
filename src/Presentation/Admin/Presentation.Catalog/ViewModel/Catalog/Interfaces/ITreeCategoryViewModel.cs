using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ITreeCategoryViewModel : IViewModel
	{
		IViewModel Parent { get; set; }
		CategoryBase InnerItem { get; }

		DelegateCommand<string> PriorityChangeCommand { get; }
		void RefreshUI();
	}
}
