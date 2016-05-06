using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using cartCoreModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.OrderModule.Data.Converters
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
			if (entity.Discounts != null && entity.Discounts.Any())
			{
				retVal.Discount = entity.Discounts.First().ToCoreModel();
			}

			retVal.TaxDetails = entity.TaxDetails.Select(x => x.ToCoreModel()).ToList();
		
			return retVal;
		}

		public static LineItem ToOrderCoreModel(this cartCoreModel.LineItem lineItem)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new LineItem();
			retVal.InjectFrom(lineItem);

			retVal.IsGift = lineItem.IsGift;
            retVal.BasePrice = lineItem.ListPrice;
			retVal.Price = lineItem.PlacedPrice;
            retVal.DiscountAmount = lineItem.DiscountTotal;
            retVal.Tax = lineItem.TaxTotal;
			retVal.FulfillmentLocationCode = lineItem.FulfillmentLocationCode;
            retVal.DynamicProperties = null; //to prevent copy dynamic properties from ShoppingCart LineItem to Order LineItem

            if (lineItem.Discounts != null)
			{
				retVal.Discount = lineItem.Discounts.Select(x => x.ToOrderCoreModel()).FirstOrDefault();
			}
			retVal.TaxDetails = lineItem.TaxDetails;
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
			if(lineItem.Discount != null)
			{
				retVal.Discounts = new ObservableCollection<DiscountEntity>(new DiscountEntity[] { lineItem.Discount.ToDataModel() });
			}
			if(lineItem.TaxDetails != null)
			{
				retVal.TaxDetails = new ObservableCollection<TaxDetailEntity>();
				retVal.TaxDetails.AddRange(lineItem.TaxDetails.Select(x=>x.ToDataModel()));
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


			var patchInjectionPolicy = new PatchInjection<LineItemEntity>(x => x.BasePrice, x => x.Price,
																		  x => x.Quantity, x => x.DiscountAmount, x => x.Tax,  x => x.Height, x => x.Length,
																		 x => x.Width, x => x.MeasureUnit, x => x.WeightUnit, x => x.Weight, x => x.TaxType, x => x.IsCancelled, x => x.CancelledDate, x => x.CancelReason);
			target.InjectFrom(patchInjectionPolicy, source);


			if (!source.Discounts.IsNullCollection())
			{
				source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}

			if (!source.TaxDetails.IsNullCollection())
			{
				var taxDetailComparer = AnonymousComparer.Create((TaxDetailEntity x) => x.Name);
				source.TaxDetails.Patch(target.TaxDetails, taxDetailComparer, (sourceTaxDetail, targetTaxDetail) => sourceTaxDetail.Patch(targetTaxDetail));
			}
		}

	}
}
