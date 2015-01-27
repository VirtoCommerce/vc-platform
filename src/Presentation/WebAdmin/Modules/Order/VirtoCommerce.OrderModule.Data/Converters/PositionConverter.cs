using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class PositionConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this Position source, Position target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			target.BasePrice = source.BasePrice;
			target.Price = source.Price;
			target.Quantity = source.Quantity;
			target.StaticDiscount = source.StaticDiscount;
			target.Tax = source.Tax;
			
		}

	}

	public class PositionComparer : IEqualityComparer<Position>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(Position x, Position y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(Position obj)
		{
			return obj.Id.GetHashCode();
		}

		#endregion
	}
}
