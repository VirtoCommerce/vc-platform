using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class AddressConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this Address source, Address target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			//Simply properties patch
			if (source.City != null)
				target.City = source.City;

			if (source.CountryCode != null)
				target.CountryCode = source.CountryCode;
			if (source.CountryName != null)
				target.CountryName = source.CountryName;
			if (source.Email != null)
				target.Email = source.Email;
			if (source.FirstName != null)
				target.FirstName = source.FirstName;
			if (source.LastName != null)
				target.LastName = source.LastName;
			if (source.Line1 != null)
				target.Line1 = source.Line1;
			if (source.Line2 != null)
				target.Line2 = source.Line2;
			if (source.MiddleName != null)
				target.MiddleName = source.MiddleName;
			if (source.Organization != null)
				target.Organization = source.Organization;
			if (source.Phone != null)
				target.Phone = source.Phone;
			if (source.PostalCode != null)
				target.PostalCode = source.PostalCode;
			if (source.Zip != null)
				target.Zip = source.Zip;

		}

	}

	public class AddressComparer : IEqualityComparer<Address>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(Address x, Address y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(Address obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}
}
