using System;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;

namespace VirtoCommerce.Foundation.Orders.Extensions
{
    public static class JurisdictionExtensions
    {

        public static bool CheckAllFieldsMatch(this Jurisdiction jurisdiction, string countryCode, string state, string postalCode, string regionCode, string district, string geoCode, string city)
        {

            if (!string.IsNullOrEmpty(jurisdiction.GeoCode) &&
                !string.Equals(jurisdiction.GeoCode, geoCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(jurisdiction.District) &&
                !string.Equals(jurisdiction.District, district, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(jurisdiction.City) &&
                !string.Equals(jurisdiction.City, city, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(jurisdiction.ZipPostalCodeEnd) && !string.IsNullOrEmpty(jurisdiction.ZipPostalCodeStart) &&
                !(string.CompareOrdinal(jurisdiction.ZipPostalCodeStart, postalCode) <= 0 &&
                  string.CompareOrdinal(jurisdiction.ZipPostalCodeEnd, postalCode) >= 0))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(jurisdiction.StateProvinceCode) &&
                !string.Equals(jurisdiction.StateProvinceCode, state, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(jurisdiction.CountryCode) &&
                !string.Equals(jurisdiction.CountryCode, countryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
