using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using System.Collections.ObjectModel;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class LineItemConverter
	{
		public static LineItem ToCoreModel(this LineItemEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new LineItem();
			retVal.InjectFrom(entity);
			retVal.Currency = entity.Currency;
			retVal.TaxDetails = entity.TaxDetails.Select(x => x.ToCoreModel()).ToList();
            retVal.Discounts = entity.Discounts.Select(x => x.ToCoreModel()).ToList();
            return retVal;
		}

		public static LineItemEntity ToDataModel(this LineItem lineItem, PrimaryKeyResolvingMap pkMap)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new LineItemEntity();
            pkMap.AddPair(lineItem, retVal);

            retVal.InjectFrom(lineItem);
			retVal.Currency = lineItem.Currency.ToString();
			if (lineItem.TaxDetails != null)
			{
				retVal.TaxDetails = new ObservableCollection<TaxDetailEntity>();
				retVal.TaxDetails.AddRange(lineItem.TaxDetails.Select(x => x.ToDataModel()));
			}

            if (lineItem.Discounts != null)
            {
                retVal.Discounts = new ObservableCollection<DiscountEntity>();
                retVal.Discounts.AddRange(lineItem.Discounts.Select(x => x.ToDataModel(pkMap)));
            }
            return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this LineItemEntity source, LineItemEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<LineItemEntity>(x => x.Quantity, x => x.SalePrice, x => x.PlacedPrice, x => x.ListPrice, x => x.ExtendedPrice, x => x.TaxIncluded, x => x.DiscountTotal, x => x.TaxTotal, x => x.TaxType, x => x.ValidationType);
            target.InjectFrom(patchInjection, source);

            if (!source.TaxDetails.IsNullCollection())
			{
				var taxDetailComparer = AnonymousComparer.Create((TaxDetailEntity x) => x.Name);
				source.TaxDetails.Patch(target.TaxDetails, taxDetailComparer, (sourceTaxDetail, targetTaxDetail) => sourceTaxDetail.Patch(targetTaxDetail));
			}
            if (!source.Discounts.IsNullCollection())
            {
                source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
            }
        }

	}
}
