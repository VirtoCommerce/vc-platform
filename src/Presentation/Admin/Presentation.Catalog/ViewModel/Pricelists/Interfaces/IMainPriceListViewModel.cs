using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces
{
    public interface IMainPriceListViewModel : IViewModel
    {
        List<ItemTypeHomeTab> SubItems { get; }
    }
}
