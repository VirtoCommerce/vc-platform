using Omu.ValueInjecter;
using System;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

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
            addressWebModel.Type = (AddressType)Enum.Parse(typeof(AddressType), address.Type, true);

            return addressWebModel;
        }

        public static VirtoCommerceCartModuleWebModelAddress ToServiceModel(this Address address)
        {
            var addressServiceModel = new VirtoCommerceCartModuleWebModelAddress();

            addressServiceModel.InjectFrom(address);
            addressServiceModel.Type = address.Type.ToString();

            return addressServiceModel;
        }
    }
}