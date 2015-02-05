using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;

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
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);

			return retVal;
		}

		public static LineItemEntity ToEntity(this LineItem lineItem)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new LineItemEntity();
			retVal.InjectFrom(lineItem);
			retVal.Currency = lineItem.Currency.ToString();
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

			//Simply properties patch
			target.Quantity = source.Quantity;
			target.SalePrice = source.SalePrice;
			target.PlacedPrice = source.PlacedPrice;
			target.ListPrice = source.ListPrice;
		}

	}
}
