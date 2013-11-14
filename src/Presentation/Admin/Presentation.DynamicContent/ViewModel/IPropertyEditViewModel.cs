using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel
{
    public interface IPropertyEditViewModel : IViewModel
    {
		DynamicContentItemProperty InnerItem { get; }

        bool Validate();
    }
}
