using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Interfaces
{
    public interface ICaseTemplatePropertyViewModel : IViewModel
    {
        CaseTemplateProperty InnerItem { get; }
    }
}
