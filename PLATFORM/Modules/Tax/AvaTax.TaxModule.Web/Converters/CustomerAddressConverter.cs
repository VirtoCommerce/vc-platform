using AvaTaxCalcREST;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class CustomerAddressConverter
    {
        public static Address ToValidateAddressRequest(this VirtoCommerce.Domain.Customer.Model.Address address, string companyCode)
        {
            var retVal = new Address()
            {
                Country = address.CountryName,
                City = address.City,
                Line1 = address.Line1,
                Line2 = address.Line2,
                Region = address.RegionName,
                PostalCode = address.PostalCode
            };

            return retVal;
        }
    }
}