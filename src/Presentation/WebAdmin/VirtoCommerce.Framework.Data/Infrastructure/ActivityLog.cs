using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Framework.Core.Infrastructure;

namespace VirtoCommerce.Framework.Data.Infrastructure
{
	public class ActivityLog : Entity
	{
		public DateTime Created { get; set; }

		public string EntityType { get; set; }

		public string EntityIdentity { get; set; }

		public string UserName { get; set; }

		public string ActivityType { get; set; }

		public string FieldName { get; set; }

		public string OldValue { get; set; }

		public string NewValue { get; set; }

		public object Tag { get; set; }
	}
}