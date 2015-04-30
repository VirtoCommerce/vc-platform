using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentPublishingGroup : AuditableEntity
	{
		public DynamicContentPublishingGroup()
		{
			ContentItems = new NullCollection<PublishingGroupContentItem>();
			ContentPlaces = new NullCollection<PublishingGroupContentPlace>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		public int Priority { get; set; }

		public bool IsActive { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string ConditionExpression { get; set; }

		public string PredicateVisualTreeSerialized { get; set; }

		#region Navigation Properties

		public virtual ICollection<PublishingGroupContentItem> ContentItems { get; set; }

		public virtual ICollection<PublishingGroupContentPlace> ContentPlaces { get; set; }
		#endregion
	}
}
