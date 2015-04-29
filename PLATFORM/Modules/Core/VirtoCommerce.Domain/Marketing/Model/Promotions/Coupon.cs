using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Marketing.Model
{
	public class Coupon : AuditableEntity
	{

		public string CustomerId { get; set; }

		public string CustomerName { get; set; }
	
		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public DateTime? UsedDate { get; set; }

		public string Code { get; set; }

		public decimal Amount { get; set; }

		public string Description { get; set; }

		public decimal? MinOrderAmount { get; set; }

		public bool IsActive
		{
			get
			{
				var retVal = UsedDate == null;
				if (retVal)
				{
					var now = DateTime.Now;
					retVal = (StartDate == null || now >= StartDate) && (EndDate == null || EndDate >= now);
				}
				return retVal;
			}

		}

		public string RemainderDays { get; set; }
	}
}
