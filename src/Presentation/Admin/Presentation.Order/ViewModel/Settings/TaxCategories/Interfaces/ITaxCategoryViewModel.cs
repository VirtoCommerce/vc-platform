using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Interfaces
{
    public interface ITaxCategoryViewModel : IViewModel
    {
        TaxCategory InnerItem { get; }
    }
}
