using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class AddressConverter
	{
		public static Address ToCoreModel(this AddressEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new Address();
			retVal.InjectFrom(entity);
			retVal.AddressType = (AddressType)Enum.Parse(typeof(AddressType), entity.AddressType);
			return retVal;
		}

		public static AddressEntity ToEntity(this Address address)
		{
			if (address == null)
				throw new ArgumentNullException("address");

			var retVal = new AddressEntity();
			retVal.InjectFrom(address);
	
			retVal.AddressType = address.AddressType.ToString();

			return retVal;
		}


		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this AddressEntity source, AddressEntity target)
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
			if (source.Organization != null)
				target.Organization = source.Organization;
			if (source.Phone != null)
				target.Phone = source.Phone;
			if (source.PostalCode != null)
				target.PostalCode = source.PostalCode;
		}

	}

	public class AddressComparer : IEqualityComparer<AddressEntity>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(AddressEntity x, AddressEntity y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(AddressEntity obj)
		{
			var result = String.Join(":", obj.Organization, obj.City, obj.CountryCode, obj.CountryName,
										  obj.Email, obj.FirstName, obj.LastName, obj.Line1, obj.Line2, obj.Phone, obj.PostalCode);
			return result.GetHashCode();
		}


		#endregion
	}
}
