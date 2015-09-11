using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Data.Model
{
	public class TierPriceEntity : Entity
	{
		[Column(TypeName = "Money")]
		public decimal Price { get; set; }

		public long Quantity { get; set; }

		#region Navigation Properties

		public virtual QuoteItemEntity QuoteItem { get; set; }
		public string QuoteItemId { get; set; }

		#endregion
	}
}
