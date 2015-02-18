using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.Foundation.Customers.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
	public static class AddressConverter
	{
		public static coreModel.Address ToCoreModel(this foundationModel.Address entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new coreModel.Address();
			retVal.InjectFrom(entity);

			return retVal;
		}

		public static foundationModel.Address ToFoundation(this coreModel.Address address)
		{
			if (address == null)
				throw new ArgumentNullException("address");

			var retVal = new foundationModel.Address();
			retVal.InjectFrom(address);
			return retVal;
		}


		/// <summary>
		/// Patch 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.Address source, foundationModel.Address target)
		{
			var patchInjectionPolicy = new PatchInjection<foundationModel.Address>(x => x.City, x => x.CountryCode,
																					  x => x.CountryName, x => x.DaytimePhoneNumber,
																					  x => x.Email, x => x.EveningPhoneNumber, x => x.FaxNumber,
																					  x => x.FirstName, x => x.LastName, x => x.Line1,
																					  x => x.Line2, x => x.Name, x => x.Organization, x => x.PostalCode,
																					  x => x.RegionName, x => x.RegionId, x => x.StateProvince, x => x.Type);
			target.InjectFrom(patchInjectionPolicy, source);
		}

	}

	public class AddressComparer : IEqualityComparer<foundationModel.Address>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(foundationModel.Address x, foundationModel.Address y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(foundationModel.Address obj)
		{
			var result = String.Join(":", obj.Organization, obj.City, obj.CountryCode, obj.CountryName, obj.FaxNumber, obj.Name, obj.RegionName, obj.RegionId, obj.StateProvince,
										  obj.Email, obj.FirstName, obj.LastName, obj.Line1, obj.Line2, obj.DaytimePhoneNumber, obj.PostalCode);
			return result.GetHashCode();
		}


		#endregion
	}
}
