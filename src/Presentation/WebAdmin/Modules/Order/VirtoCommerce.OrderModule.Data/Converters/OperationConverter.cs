using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class OperationConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this Operation source, Operation target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply properties patch
			if (source.Comment != null)
				target.Comment = source.Comment;
			if (source.Currency != null)
				target.Currency = source.Currency;
			if (source.IsApproved != null)
				target.IsApproved = source.IsApproved;
			if (source.Number != null)
				target.Number = source.Number;
			if (source.SourceAgentId != null)
				target.SourceAgentId = source.SourceAgentId;
			if (source.SourceStoreId != null)
				target.SourceStoreId = source.SourceStoreId;
			if(source.StatusId != null)
				target.StatusId = source.StatusId;
			if (source.TargetAgentId != null)
				target.TargetAgentId = source.TargetAgentId;
			if (source.TargetStoreId != null)
				target.TargetStoreId = source.TargetStoreId;
			target.Tax = source.Tax;
			if (source.TaxIncluded != null)
				target.TaxIncluded = source.TaxIncluded;
		}
	}

	public class OperationComparer : IEqualityComparer<Operation>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(Operation x, Operation y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(Operation obj)
		{
			return obj.Id.GetHashCode();
		}

		#endregion
	}
}
