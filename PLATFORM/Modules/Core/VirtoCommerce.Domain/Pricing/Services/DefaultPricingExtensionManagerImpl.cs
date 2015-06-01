using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Pricing.Services
{
	public class DefaultPricingExtensionManagerImpl : IPricingExtensionManager
	{
		#region IPricingExtensionManager Members
		public virtual ConditionExpressionTree ConditionExpressionTree { get; set; }
		#endregion
	}
}
