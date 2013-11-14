using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Interfaces
{
    public interface IShippingMethodViewModel : IViewModel
    {
        ShippingMethod InnerItem { get; }

		void UpdateOfLanguages(ICollectionChange<GeneralLanguage> languagesVm);
		void UpdateOfPaymentItems();
	    void UpdateOfJurisdictionGroup();
    }
}
