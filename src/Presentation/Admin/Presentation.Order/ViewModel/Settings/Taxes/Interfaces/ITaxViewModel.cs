using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.Foundation.Orders.Model.Taxes;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces
{
    public interface ITaxViewModel : IViewModel
    {
        Tax InnerItem { get; }

        void UpdateOfLanguages(ICollectionChange<GeneralLanguage> languagesVM);
    }
}
