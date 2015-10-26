using Omu.ValueInjecter;
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
    }
}
