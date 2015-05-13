using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class DynamicContentPublication : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public bool IsActive { get; set; }

		public string PredicateSerialized { get; set; }
		public string PredicateVisualTreeSerialized { get; set; }

		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public ICollection<DynamicContentItem> ContentItems { get; set; }
		public ICollection<DynamicContentPlace> ContentPlaces { get; set; }
	}
}
