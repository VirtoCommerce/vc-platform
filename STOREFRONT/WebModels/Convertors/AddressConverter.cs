using VirtoCommerce.Web.Models;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Convertors
{
    public static class AddressConverter
    {
        public static CustomerAddress ToViewModel(this DataContracts.Address address)
        {
            var addressModel = new CustomerAddress();

            addressModel.Address1 = address.Line1;
            addressModel.Address2 = address.Line2;
            addressModel.City = address.City;
            addressModel.Company = address.Organization;
            addressModel.Country = address.CountryName;
            addressModel.CountryCode = address.CountryCode;
            addressModel.FirstName = address.FirstName;
            addressModel.LastName = address.LastName;
            addressModel.Phone = address.Phone;
            addressModel.Province = address.RegionName;
            addressModel.ProvinceCode = address.RegionId;
            addressModel.Zip = address.PostalCode;

            return addressModel;
        }

        public static DataContracts.Address ToServiceModel(this CustomerAddress addressModel, string email = null)
        {
            var address = new DataContracts.Address();

            address.City = addressModel.City;
            address.CountryCode = addressModel.CountryCode;
            address.CountryName = addressModel.Country;
            address.Email = email;
            address.FirstName = addressModel.FirstName;
            address.LastName = addressModel.LastName;
            address.Line1 = addressModel.Address1;
            address.Line2 = addressModel.Address2;
            address.Organization = addressModel.Company;
            address.Phone = addressModel.Phone;
            address.PostalCode = addressModel.Zip;
            address.RegionId = addressModel.ProvinceCode;
            address.RegionName = addressModel.Province;

            return address;
        }
    }
}