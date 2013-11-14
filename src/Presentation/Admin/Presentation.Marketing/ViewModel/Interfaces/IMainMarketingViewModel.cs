using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces
{
    public interface IMainMarketingViewModel : IViewModel
    {
        List<ItemTypeHomeTab> SubItems { get; }
    }
}
