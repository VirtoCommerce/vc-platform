using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	/// <summary>
	/// Represent dynamic content publication and link content and places together
	/// may contain conditional expressions applicability 
	/// </summary>
	public class DynamicContentPublication : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		/// <summary>
		/// Priority used for chose publication in combination
		/// </summary>
		public int Priority { get; set; }
		public bool IsActive { get; set; }
		/// <summary>
		/// Store where the publication is active 
		/// </summary>
		public string StoreId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public ICollection<DynamicContentItem> ContentItems { get; set; }
		public ICollection<DynamicContentPlace> ContentPlaces { get; set; }

		/// <summary>
		/// Dynamic conditions tree determine the applicability of this publication
		/// </summary>
		public ConditionExpressionTree DynamicExpression { get; set; }
	}
}
