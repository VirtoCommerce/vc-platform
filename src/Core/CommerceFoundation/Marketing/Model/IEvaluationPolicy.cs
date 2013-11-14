using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public interface IEvaluationPolicy
	{
        /// <summary>
        /// Gets or sets the priority the policies are executed by. The highest priority is ran first.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
		int Priority { get; set; }

        /// <summary>
        /// Gets or sets the group. The group can be global, catalog or shipping
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        string Group { get; set; }
        
        /// <summary>
        /// Filters the promotions.
        /// </summary>
        /// <param name="evaluationContext">The evaluation context.</param>
        /// <param name="records">The records, must be sorted in the order they are applied.</param>
        /// <returns></returns>
		PromotionRecord[] FilterPromotions(IPromotionEvaluationContext evaluationContext, PromotionRecord[] records);
	}
}
