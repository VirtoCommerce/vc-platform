using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.CartModule.Data.Model;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class TaxDetailConverter
	{
		public static TaxDetailEntity ToDataModel(this TaxDetail taxDetail)
		{
			if (taxDetail == null)
				throw new ArgumentNullException("taxDetail");

			var retVal = new TaxDetailEntity();
			retVal.InjectFrom(taxDetail);
			return retVal;
		}

		public static TaxDetail ToCoreModel(this TaxDetailEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new TaxDetail();
			retVal.InjectFrom(entity);

			return retVal;
		}

			/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this TaxDetailEntity source, TaxDetailEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			var patchInjectionPolicy = new PatchInjection<TaxDetailEntity>(x => x.Rate, x => x.Amount);
			target.InjectFrom(patchInjectionPolicy, source);

		}
	}

}
