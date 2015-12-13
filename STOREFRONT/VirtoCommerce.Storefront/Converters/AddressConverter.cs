using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.Storefront.Model;
using shopifyModel = VirtoCommerce.LiquidThemeEngine.Objects;

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

        public static VirtoCommerceCustomerModuleWebModelAddress ToServiceModel(this shopifyModel.Address address, Country[] countries)
        {
            var result = new VirtoCommerceCustomerModuleWebModelAddress();
            result.CopyFrom(address, countries);
            return result;
        }

        public static VirtoCommerceCustomerModuleWebModelAddress ToCustomerModel(this VirtoCommerceOrderModuleWebModelAddress orderAddress)
        {
            var customerAddress = new VirtoCommerceCustomerModuleWebModelAddress();

            customerAddress.InjectFrom(orderAddress);

            customerAddress.Name = string.Format("{0} {1}", orderAddress.FirstName, orderAddress.LastName);

            return customerAddress;
        }

        public static VirtoCommerceCustomerModuleWebModelAddress CopyFrom(this VirtoCommerceCustomerModuleWebModelAddress result, shopifyModel.Address address, Country[] countries)
        {
            result.InjectFrom<NullableAndEnumValueInjection>(address);

            result.Organization = address.Company;
            result.CountryName = address.Country;
            result.PostalCode = address.Zip;
            result.Line1 = address.Address1;
            result.Line2 = address.Address2;
            result.RegionName = address.Province;

            result.Name = string.Join(" ", result.FirstName, result.LastName).Trim();

            var country = countries.FirstOrDefault(c => string.Equals(c.Name, address.Country, StringComparison.OrdinalIgnoreCase));
            if (country != null)
            {
                result.CountryCode = country.Code3;

                if (address.Province != null && country.Regions != null)
                {
                    var region = country.Regions.FirstOrDefault(r => string.Equals(r.Name, address.Province, StringComparison.OrdinalIgnoreCase));

                    if (region != null)
                    {
                        result.RegionId = region.Code;
                    }
                }
            }

            return result;
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

        public static Address ToWebModel(this VirtoCommerceOrderModuleWebModelAddress address)
        {
            var addressWebModel = new Address();

            addressWebModel.InjectFrom(address);
            addressWebModel.Type = (AddressType)Enum.Parse(typeof(AddressType), address.AddressType, true);

            return addressWebModel;
        }
    }
}