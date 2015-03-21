using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Pricing.Model
{
	public class Price : Entity, IAuditable
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion
		public string PricelistId { get; set; }
		public CurrencyCodes Currency { get; set; }
		public string ProductId { get; set; }
		public decimal? Sale { get; set; }
		public decimal List { get; set; }
		public int MinQuantity { get; set; }
	}
}
