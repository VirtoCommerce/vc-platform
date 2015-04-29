using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using foundationModel = VirtoCommerce.PricingModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.PricingModule.Data.Converters
{
	public static class PriceListAssignmentConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.PricelistAssignment ToCoreModel(this foundationModel.PricelistAssignment dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.PricelistAssignment();
			retVal.InjectFrom(dbEntity);
			return retVal;

		}


		public static foundationModel.PricelistAssignment ToFoundation(this coreModel.PricelistAssignment assignment)
		{
			if (assignment == null)
				throw new ArgumentNullException("assignment");

			var retVal = new foundationModel.PricelistAssignment();

			retVal.InjectFrom(assignment);

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.PricelistAssignment source, foundationModel.PricelistAssignment target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<foundationModel.PricelistAssignment>(x => x.Name, x => x.Description,
																						 x => x.StartDate, x => x.EndDate, x => x.CatalogId,
																						 x => x.PricelistId, x => x.Priority, x => x.ConditionExpression);
			target.InjectFrom(patchInjection, source);
		}


	}
}
