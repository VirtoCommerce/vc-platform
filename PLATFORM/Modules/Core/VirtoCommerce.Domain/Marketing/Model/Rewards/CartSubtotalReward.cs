using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Marketing.Model
{
	
	public class CartSubtotalReward : PromotionReward
	{

		public decimal Amount { get; set; }
	}
}
