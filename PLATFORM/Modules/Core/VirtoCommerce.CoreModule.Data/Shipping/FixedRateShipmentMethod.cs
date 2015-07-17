using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CoreModule.Data.Shipping
{
	public class FixedRateShippingMethod : ShippingMethod
	{
		public FixedRateShippingMethod()
			: base("FixedRate")
		{
		}

		public FixedRateShippingMethod(ICollection<SettingEntry> settings)
			: base("FixedRate")
		{
			Settings = settings;
		}

		private decimal Rate
		{
			get
			{
				decimal retVal = 0;
                var settingRate = Settings.Where(x => x.Name == "Rate").FirstOrDefault();
				if(settingRate != null)
				{
					retVal = Decimal.Parse(settingRate.Value, CultureInfo.InvariantCulture);
				}
				return retVal;
			}
		}

		public override IEnumerable<ShippingRate> CalculateRates(Domain.Common.IEvaluationContext context)
		{
			var shippingEvalContext = context as ShippingEvaluationContext;
			if(shippingEvalContext == null)
			{
				throw new NullReferenceException("shippingEvalContext");
			}

			return new ShippingRate[] { new ShippingRate { Rate = Rate, Currency = shippingEvalContext.ShoppingCart.Currency, ShippingMethod = this } };
		}
	}
}
