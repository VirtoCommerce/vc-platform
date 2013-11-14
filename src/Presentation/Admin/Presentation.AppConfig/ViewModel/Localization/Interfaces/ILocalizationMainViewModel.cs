using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces
{
    public interface ILocalizationMainViewModel : IViewModel
    {
        List<ItemTypeHomeTab> SubItems { get; }
    }
}
