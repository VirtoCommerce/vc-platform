using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;

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

			//Simply properties patch
			if (source.Comment != null)
				target.Comment = source.Comment;
			if (source.Currency != null)
				target.Currency = source.Currency;
			if (source.Number != null)
				target.Number = source.Number;
			if (source.SourceAgentId != null)
				target.SourceAgentId = source.SourceAgentId;
			if (source.SourceStoreId != null)
				target.SourceStoreId = source.SourceStoreId;
			if (source.Status != null)
				target.Status = source.Status;
			if (source.TargetAgentId != null)
				target.TargetAgentId = source.TargetAgentId;
			if (source.TargetStoreId != null)
				target.TargetStoreId = source.TargetStoreId;

			target.Tax = source.Tax;
			target.TaxIncluded = source.TaxIncluded;
			target.IsApproved = source.IsApproved;
		}
	}

	public class OperationComparer : IEqualityComparer<OperationEntity>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(OperationEntity x, OperationEntity y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(OperationEntity obj)
		{
			return obj.Id.GetHashCode();
		}

		#endregion
	}
}
