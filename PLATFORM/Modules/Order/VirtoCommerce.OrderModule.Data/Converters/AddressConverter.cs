using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Cart.Model;

namespace VirtoCommerce.OrderModule.Data.Converters
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

		public static Address ToCoreModel(this coreModel.Address address)
		{
			if (address == null)
				throw new ArgumentNullException("entity");

			var retVal = new Address();
			retVal.InjectFrom(address);
			retVal.AddressType = (AddressType)(int)address.AddressType;
			return retVal;
		}

		public static AddressEntity ToDataModel(this Address address)
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
			//Nothing todo
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
										  obj.Email, obj.FirstName, obj.LastName, obj.Line1, obj.Line2, obj.Phone, obj.PostalCode, obj.RegionId, obj.RegionName);
			return result.GetHashCode();
		}


		#endregion
	}
}
