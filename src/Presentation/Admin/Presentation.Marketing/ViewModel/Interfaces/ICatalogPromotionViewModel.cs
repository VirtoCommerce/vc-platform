using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;
using System.Collections.Generic;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces
{
    public interface ICatalogPromotionViewModel : IPromotionViewModelBase
    {
        List<catalogModel.CatalogBase> AvailableCatalogs { get; }
    }
}
