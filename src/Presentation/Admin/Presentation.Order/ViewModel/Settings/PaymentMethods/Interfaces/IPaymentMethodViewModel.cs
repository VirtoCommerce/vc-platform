using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Interfaces
{
    public interface IPaymentMethodViewModel : IViewModelDetailBase
    {
        PaymentMethod InnerItem { get; }

        IGeneralLanguagesStepViewModel LanguagesStepViewModel { get; set; }

        void InitializeLanguagesForSave();
        void InitializeGatewayValuesForSave();
	    void GetShippingItemToInnerItem();
    }
}
