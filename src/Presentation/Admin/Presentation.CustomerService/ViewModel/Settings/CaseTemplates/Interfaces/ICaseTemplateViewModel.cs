using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Interfaces
{
    public interface ICaseTemplateViewModel : IViewModel
    {
        CaseTemplate InnerItem { get; }

        DelegateCommand ItemAddCommand { get; }
        DelegateCommand<CaseTemplateProperty> ItemEditCommand { get; }
        DelegateCommand<CaseTemplateProperty> ItemDeleteCommand { get; }
    }
}
