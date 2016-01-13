using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Pricing.Model
{
	public class Price : AuditableEntity, ICloneable
	{
		public string PricelistId { get; set; }
		public string Currency { get; set; }
		public string ProductId { get; set; }
		public decimal? Sale { get; set; }
		public decimal List { get; set; }
		public int MinQuantity { get; set; }

		public decimal EffectiveValue
		{
			get
			{
				return Sale ?? List;
			}
		}

		#region ICloneable Members

		public object Clone()
		{
			return new Price
			{
				CreatedBy = this.CreatedBy,
				CreatedDate = this.CreatedDate,
				ModifiedBy = this.ModifiedBy,
				ModifiedDate = this.ModifiedDate,
				PricelistId = this.PricelistId,
				Currency = this.Currency,
				ProductId = this.ProductId,
				Sale = this.Sale,
				List = this.List,
				MinQuantity = this.MinQuantity
			};
		}

		#endregion
	}
}
