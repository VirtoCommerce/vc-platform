using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Pricing.Services
{
	public interface IPricingExtensionManager
	{
		ConditionExpressionTree ConditionExpressionTree { get; set; }
	}
}
