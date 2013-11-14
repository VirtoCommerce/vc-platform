using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Model.Taxes;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces
{
    public interface ITaxValueViewModel : IViewModel
    {
        TaxValue InnerItem { get; }
    }
}