using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Domain.Shipping.Model
{
	public abstract class ShippingMethod : Entity, IHaveSettings
	{
		public ShippingMethod(string code)
		{
			Id = Guid.NewGuid().ToString("N");
			Code = code;
		}

		/// <summary>
		/// Method identity property
		/// </summary>
		public string Code { get; set; }
		public string Description { get; set; }
		public string LogoUrl { get; set; }
		public bool IsActive { get; set; }
		public int Priority { get; set; }


		#region IHaveSettings Members

		public ICollection<SettingEntry> Settings { get; set; }

		#endregion

		public abstract ShippingRate CalculateRate(IEvaluationContext context);
	}
}
