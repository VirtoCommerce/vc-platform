using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class OrderOperation : Operation
	{
		public DateTime DeilveryPlanedDate { get; set; }
	}
}
