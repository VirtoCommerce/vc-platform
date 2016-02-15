using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Pricing.Model
{
    /// <summary>
    /// Used to assign pricelist to specific catalog by using conditional expression 
    /// </summary>
	public class PricelistAssignment : AuditableEntity
	{
		public string CatalogId { get; set; }
		public string PricelistId { get; set; }
        public Pricelist Pricelist { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
        /// <summary>
        /// If two PricelistAssignments satisfies the conditions and rules, will use one with the greater priority
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Start of period when Prices Assignment is valid. Null value means no limit
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// End of period when Prices Assignment is valid. Null value means no limit
        /// </summary>
		public DateTime? EndDate { get; set; }
        /// <summary>
        /// Serialized condition expression used to evaluate current assignment availability 
        /// </summary>
		public string ConditionExpression { get; set; }
        /// <summary>
        /// Serialized condition expression visual tree used in UI
        /// </summary>
		public string  PredicateVisualTreeSerialized { get; set; }
        /// <summary>
        /// Deserialized conditional expression  used to evaluate current assignment availability 
        /// </summary>
        public Func<IEvaluationContext, bool> Condition { get; set; }
    }
}
