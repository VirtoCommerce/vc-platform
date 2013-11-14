using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces
{
    public interface ITaxesMainSettingsViewModel : IViewModel
    {
        List<ItemTypeHomeTab> SubItems { get; }
    }
}
