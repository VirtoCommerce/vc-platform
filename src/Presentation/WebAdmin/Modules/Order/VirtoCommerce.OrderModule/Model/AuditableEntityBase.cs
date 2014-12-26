using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Model
{
	public abstract class AuditableEntityBase<T> : EntityBase<T, string>, IAuditable  where T : EntityBase<T, string>
	{
		#region IAuditable Members

		public DateTime CreatedDate	{ get; set;	}

		public string CreatedBy	{ get; set;	}

		public DateTime ModifiedDate { get; set; }

		public string ModifiedBy { get; set; }

		#endregion
	}
}
