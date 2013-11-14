using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces
{
    public interface ISecurityMainViewModel : IViewModel
    {
        List<ItemTypeHomeTab> SubItems { get; }
    }
}
