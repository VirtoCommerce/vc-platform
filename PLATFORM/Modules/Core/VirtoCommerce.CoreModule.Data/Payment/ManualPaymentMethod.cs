using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CoreModule.Data.Payment
{
	public class ManualPaymentMethod : PaymentMethod
	{
		public ManualPaymentMethod(ICollection<SettingEntry> settings)
			: base("TemporaryPaymentMethod")
		{
			Settings = settings;
		}


		public override ProcessPaymentResult ProcessPayment(Domain.Common.IEvaluationContext context)
		{
			throw new System.NotImplementedException();
		}

		public override PostProcessPaymentResult PostProcessPayment(Domain.Common.IEvaluationContext context)
		{
			throw new NotImplementedException();
		}

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.Standard; }
		}
	}
}
