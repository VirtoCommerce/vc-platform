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
	
			if (entity.Discount != null)
			{
				retVal.Discount = entity.Discount.ToCoreModel();
			}
		
			return retVal;
		}

		public static LineItemEntity ToEntity(this LineItem lineItem)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new LineItemEntity();
			retVal.InjectFrom(lineItem);

			if (retVal.IsTransient())
			{
				retVal.Id = Guid.NewGuid().ToString();
			}

			if(lineItem.Discount != null)
			{
				retVal.Discount = lineItem.Discount.ToEntity();
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
			
		}

	}

	public class LineItemComparer : IEqualityComparer<LineItemEntity>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(LineItemEntity x, LineItemEntity y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(LineItemEntity obj)
		{
			return obj.Id.GetHashCode();
		}

		#endregion
	}
}
