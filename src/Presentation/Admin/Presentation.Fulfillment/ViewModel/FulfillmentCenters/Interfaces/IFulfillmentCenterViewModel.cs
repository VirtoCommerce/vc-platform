using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces
{
    public interface IFulfillmentCenterViewModel : IViewModel
    {
        FulfillmentCenter InnerItem { get; set; }

        object[] AllCountries { get; }

        Country SelectedCountry { get; }

        Region SelectedRegion { get; }
    }
}
