using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces
{
    public interface IKnowledgeGroupViewModel : IViewModel
    {
        IViewModel Parent { get; set; }
        KnowledgeBaseGroup InnerItem { get; }
        KnowledgeBaseGroup OriginalItem { get; }
        bool IsValid { get; }
    }
}
