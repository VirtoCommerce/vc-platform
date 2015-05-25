using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Domain.Payment.Model
{
	public abstract class PaymentMethod : Entity, IHaveSettings
	{
		public PaymentMethod(string code)
		{
			Id = Guid.NewGuid().ToString("N");
			Code = code;
		}

		/// <summary>
		/// Method identity property (system name)
		/// </summary>
		public string Code { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public string LogoUrl { get; set; }
		public bool IsActive { get; set; }
		public int Priority { get; set; }


		#region IHaveSettings Members

		public ICollection<SettingEntry> Settings { get; set; }

		#endregion

		public abstract PaymentMethodType PaymentMethodType { get; }

		public abstract ProcessPaymentResult ProcessPayment(IEvaluationContext context);

		public abstract PostProcessPaymentResult PostProcessPayment(IEvaluationContext context);
	}
}
