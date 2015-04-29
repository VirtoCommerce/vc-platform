using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

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
	
			var patchInjectionPolicy = new PatchInjection<AddressEntity>(x => x.City, x => x.CountryCode,
																							  x => x.CountryName, x => x.Phone,
																							  x => x.Email, x => x.FirstName, x => x.LastName, x => x.Line1,
																							  x => x.Line2, x => x.AddressType, x => x.Organization, x => x.PostalCode,
																							  x => x.RegionName, x => x.RegionId, x => x.Email);
			target.InjectFrom(patchInjectionPolicy, source);
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
			var result = String.Join(":", obj.Organization, obj.City, obj.CountryCode, obj.CountryName, obj.RegionId, obj.RegionName,
										  obj.Email, obj.FirstName, obj.LastName, obj.Line1, obj.Line2, obj.Phone, obj.PostalCode);
			return result.GetHashCode();
		}


		#endregion
	}
}
