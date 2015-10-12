using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
	public static class AddressConverter
	{
		public static coreModel.Address ToCoreModel(this dataModel.AddressEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new coreModel.Address();
			retVal.InjectFrom(entity);
			retVal.AddressType = (coreModel.AddressType)Enum.Parse(typeof(coreModel.AddressType), entity.AddressType);

			return retVal;
		}

		public static dataModel.AddressEntity ToDataModel(this coreModel.Address address)
		{
			if (address == null)
				throw new ArgumentNullException("address");

			var retVal = new dataModel.AddressEntity();
			retVal.InjectFrom(address);

			retVal.AddressType = address.AddressType.ToString();
			return retVal;
		}

	}

	public class AddressComparer : IEqualityComparer<dataModel.AddressEntity>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(dataModel.AddressEntity x, dataModel.AddressEntity y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(dataModel.AddressEntity obj)
		{
			var result = String.Join(":", obj.Organization, obj.City, obj.CountryCode, obj.CountryName,
										  obj.Email, obj.FirstName, obj.LastName, obj.Line1, obj.Line2, obj.Phone, obj.PostalCode, obj.RegionId, obj.RegionName);
			return result.GetHashCode();
		}


		#endregion
	}
}
