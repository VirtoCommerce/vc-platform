using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.Foundation.Money;

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

			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			if (entity.Discounts != null && entity.Discounts.Any())
			{
				retVal.Discount = entity.Discounts.First().ToCoreModel();
			}
		
			return retVal;
		}

		public static LineItemEntity ToEntity(this LineItem lineItem)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new LineItemEntity();
			retVal.InjectFrom(lineItem);
			retVal.Currency = lineItem.Currency.ToString();
			if(lineItem.Discount != null)
			{
				retVal.Discounts = new ObservableCollection<DiscountEntity>(new DiscountEntity[] { lineItem.Discount.ToEntity() });
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

			target.BasePrice = source.BasePrice;
			target.Price = source.Price;
			target.Quantity = source.Quantity;
			target.DiscountAmount = source.DiscountAmount;
			target.Tax = source.Tax;

			if (!source.Discounts.IsNullCollection())
			{
				source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}
		}

	}
}
