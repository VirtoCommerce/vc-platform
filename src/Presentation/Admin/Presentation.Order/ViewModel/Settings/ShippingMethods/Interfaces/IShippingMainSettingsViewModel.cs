using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Interfaces
{
    public interface IShippingMainSettingsViewModel : IViewModel
    {
        List<ItemTypeHomeTab> SubItems { get; }
    }
}
