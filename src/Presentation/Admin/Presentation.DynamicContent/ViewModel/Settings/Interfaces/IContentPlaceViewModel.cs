using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Settings.Interfaces
{
	public interface IContentPlaceViewModel : IViewModel
	{
		DynamicContentPlace InnerItem { get; }
	}
}
