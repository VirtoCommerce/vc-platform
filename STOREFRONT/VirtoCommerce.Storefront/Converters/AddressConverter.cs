using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class AddressConverter
    {
        public static Address ToWebModel(this VirtoCommerceCustomerModuleWebModelAddress address)
        {
            var customerAddress = new Address();
            customerAddress.InjectFrom(address);
            return customerAddress;
        }

        public static Address ToWebModel(this VirtoCommerceCartModuleWebModelAddress address)
        {
            var addressWebModel = new Address();

            addressWebModel.InjectFrom(address);

            return addressWebModel;
        }

        public static VirtoCommerceCartModuleWebModelAddress ToServiceModel(this Address address)
        {
            var addressServiceModel = new VirtoCommerceCartModuleWebModelAddress();

            addressServiceModel.InjectFrom(address);

            return addressServiceModel;
        }
    }
}