using Mes.Gateway;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Payment.Model;

namespace MeS.PaymentGatewaysModule.Web.Managers
{
	public class MesPaymentMethod : PaymentMethod
	{
		public MesPaymentMethod()
			: base("Mes")
		{
		}

		private static string MeSProfileIdStoreSetting = "Mes.PaymentGateway.Credentials.ProfileId";
		private static string MeSProfileKeyStoreSetting = "Mes.PaymentGateway.Credentials.ProfileKey";

		public override PaymentMethodType PaymentMethodType
		{
			get { return PaymentMethodType.Standard; }
		}

		public override PaymentMethodGroupType PaymentMethodGroupType
		{
			get { return PaymentMethodGroupType.BankCard; }
		}

		private string ProfileId
		{
			get
			{
				var retVal = GetSetting(MeSProfileIdStoreSetting);
				return retVal;
			}
		}

		private string ProfileKey
		{
			get
			{
				var retVal = GetSetting(MeSProfileKeyStoreSetting);
				return retVal;
			}
		}

		public override ProcessPaymentResult ProcessPayment(ProcessPaymentEvaluationContext context)
		{
			var retVal = new ProcessPaymentResult();

			GatewaySettings settings = new GatewaySettings();
			settings.setCredentials(ProfileId, ProfileKey)
				.setVerbose(true)
				.setHostUrl(GatewaySettings.URL_CERT);
			Gateway gateway = new Gateway(settings);

			GatewayRequest request = new GatewayRequest(GatewayRequest.TransactionType.SALE);
			if (string.IsNullOrEmpty(context.Payment.OuterId))
			{
				request.setCardData("4012888812348882", "1216");
			}
			else
			{
				request.setTokenData(context.Payment.OuterId, string.Empty);
			}
			request.setAmount("1.03");
			GatewayResponse response = gateway.run(request);

			var tranId = response.getTransactionId();

			var errorCode = response.getErrorCode();

			if(errorCode.Equals("000"))
			{
				retVal.OuterId = tranId;
				retVal.IsSuccess = true;
				retVal.NewPaymentStatus = PaymentStatus.Pending; //maybe
			}
			else
			{
				retVal.NewPaymentStatus = PaymentStatus.Voided;
				retVal.Error = string.Format("Mes error {0}", errorCode);
			}

			return retVal;
		}

		public override PostProcessPaymentResult PostProcessPayment(PostProcessPaymentEvaluationContext context)
		{
			var retVal = new PostProcessPaymentResult();

			return retVal;
		}

		public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection context)
		{
			throw new NotImplementedException();
		}
	}
}