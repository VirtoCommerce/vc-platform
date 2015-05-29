using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

		public abstract ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context);

		public abstract PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context);

		public abstract ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString);

		public string GetSetting(string settingName)
		{
			var setting = Settings.FirstOrDefault(s => s.Name == settingName);

			if (setting == null && setting.Value is string && string.IsNullOrEmpty((string)setting.Value))
				throw new NullReferenceException(string.Format("{0} setting is not exist or null"));

			return (string)setting.Value;
		}
	}
}
