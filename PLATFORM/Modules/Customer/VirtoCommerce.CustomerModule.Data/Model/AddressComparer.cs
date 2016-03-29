using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Data.Model;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class AddressComparer : IEqualityComparer<Address>
    {
        #region IEqualityComparer<Discount> Members

        public bool Equals(Address x, Address y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(Address obj)
        {
            var result = String.Join(":", obj.Organization, obj.City, obj.CountryCode, obj.CountryName, obj.FaxNumber, obj.Name, obj.RegionName,
                                          obj.RegionId, obj.StateProvince, obj.Email, obj.FirstName, obj.LastName, obj.Line1, obj.Line2,
                                          obj.DaytimePhoneNumber, obj.PostalCode, obj.DaytimePhoneNumber, obj.EveningPhoneNumber,
                                          obj.Type);
            return result.GetHashCode();
        }


        #endregion
    }
}
