using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Catalogs.Services
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
	}
}
