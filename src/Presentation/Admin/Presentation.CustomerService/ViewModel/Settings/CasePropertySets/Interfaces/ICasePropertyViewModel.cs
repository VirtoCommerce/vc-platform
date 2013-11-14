using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces
{
    public interface ICasePropertyViewModel : IViewModel
    {
        CaseProperty InnerItem { get; }
    }
}
