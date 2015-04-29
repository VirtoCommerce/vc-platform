using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class OperationConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this OperationEntity source, OperationEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<OperationEntity>(x => x.Comment, x => x.Currency,
																			   x => x.Number, x => x.Status, x => x.IsCancelled,
																			   x => x.CancelledDate, x => x.CancelReason, x => x.Tax,
																			   x => x.TaxIncluded, x => x.IsApproved, x => x.Sum);
			target.InjectFrom(patchInjectionPolicy, source);

		}
	}
}
