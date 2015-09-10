using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Commerce.Model
{
	public interface ISupportCancellation
	{
		bool IsCancelled { get; set; }
		DateTime? CancelledDate { get; set; }
		string CancelReason { get; set; }
	}
}
