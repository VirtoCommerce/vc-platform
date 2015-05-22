using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CoreModule.Data.Shipping
{
	public class FixedRateShippingMethod : ShippingMethod
	{
		public FixedRateShippingMethod(ICollection<SettingEntry> settings)
			: base("FixedRate")
		{
			Settings = settings;
			Description = "Manual shipping method";
		}
		public decimal Rate { get; set; }

		public override ShippingRate CalculateRate(Domain.Common.IEvaluationContext context)
		{
			return new ShippingRate { Rate = Rate, ShippingMethod = this };
		}
	}
}
