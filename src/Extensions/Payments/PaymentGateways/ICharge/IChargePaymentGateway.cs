using System.Globalization;
using System.Linq;
using nsoftware.InPay;
using System;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.PaymentGateways
{
	public class IchargePaymentGateway : PaymentGatewayBase
	{
		Icharge _icharge;

		/// <summary>
		/// Processes the payment. Can be used for both positive and negative transactions.
		/// </summary>
		/// <param name="payment">The payment.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public override bool ProcessPayment(Payment payment, ref string message)
		{
			var info = payment as CreditCardPayment;

			if (ReferenceEquals(info, null))
			{
				payment.Status = PaymentStatus.Failed.ToString();
				message = "ICharge gateway supports only CreditCardPayment";
				return false;
			}
			var transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), info.TransactionType);
			payment.Status = PaymentStatus.Processing.ToString();
			var invoiceNr = info.OrderForm.OrderGroupId;
			_icharge = new Icharge
			{
				InvoiceNumber = invoiceNr,
				TransactionId = invoiceNr
			};

			try
			{
				_icharge.MerchantLogin = Settings["MerchantLogin"];
				_icharge.MerchantPassword = Settings["MerchantPassword"];
				_icharge.Gateway = (IchargeGateways)Enum.Parse(typeof(IchargeGateways), Settings["Gateway"]);
				bool isTestMode;
				if (Settings.ContainsKey("TestMode") && bool.TryParse(Settings["TestMode"], out isTestMode) && isTestMode)
				{
					_icharge.TestMode = true;
				}
			}
			catch
			{
				payment.Status = PaymentStatus.Failed.ToString();
				message = "ICharge gateway is not configured properly";
				return false;
			}

			if (!String.IsNullOrEmpty(Settings["GatewayURL"]))
			{
				_icharge.GatewayURL = Settings["GatewayURL"];
			}

			var transactionId = payment.ValidationCode;
			_icharge.AuthCode = payment.AuthorizationCode;

			_icharge.Card.ExpMonth = info.CreditCardExpirationMonth;
			_icharge.Card.ExpYear = info.CreditCardExpirationYear;
			_icharge.Card.Number = info.CreditCardNumber ?? "";
			_icharge.Card.CVVData = info.CreditCardSecurityCode ?? "";

			// Find the address
			var address = info.OrderForm.OrderGroup.OrderAddresses
				.FirstOrDefault(a => String.Compare(a.OrderAddressId, info.BillingAddressId, StringComparison.OrdinalIgnoreCase) == 0);

			if (address != null)
			{
				_icharge.Customer.Address = address.Line1;
				_icharge.Customer.City = address.City;
				_icharge.Customer.Country = address.CountryCode;
				_icharge.Customer.Email = address.Email;
				_icharge.Customer.FirstName = address.FirstName;
				_icharge.Customer.LastName = address.LastName;
				_icharge.Customer.Phone = address.DaytimePhoneNumber;
				_icharge.Customer.State = address.StateProvince;
				_icharge.Customer.Zip = address.PostalCode;
			}

			var transactionAmount = (double)info.Amount;

			//The following gateways require the TransactionAmount to be represented as cents without a decimal point. 
			//For example, a dollar value of "1.00" would equate to "100" for these gateways.

			if (_icharge.Gateway == IchargeGateways.gw3DSI ||
				_icharge.Gateway == IchargeGateways.gwTrustCommerce ||
				_icharge.Gateway == IchargeGateways.gwPayFuse ||
				_icharge.Gateway == IchargeGateways.gwOrbital ||
				_icharge.Gateway == IchargeGateways.gwOgone ||
				//_icharge.Gateway == IchargeGateways.gwOptimal ||
				_icharge.Gateway == IchargeGateways.gwWorldPayXML ||
				_icharge.Gateway == IchargeGateways.gwProPay ||
				_icharge.Gateway == IchargeGateways.gwLitle ||
				_icharge.Gateway == IchargeGateways.gwJetPay ||
				_icharge.Gateway == IchargeGateways.gwHSBC ||
				_icharge.Gateway == IchargeGateways.gwAdyen ||
				_icharge.Gateway == IchargeGateways.gwBarclay ||
				_icharge.Gateway == IchargeGateways.gwCyberbit ||
				_icharge.Gateway == IchargeGateways.gwGoToBilling ||
				_icharge.Gateway == IchargeGateways.gwGlobalIris)
			{
				_icharge.TransactionAmount = (transactionAmount * 100).ToString("F0", CultureInfo.InvariantCulture);
			}
			else
			{
				_icharge.TransactionAmount = transactionAmount.ToString("F2", CultureInfo.InvariantCulture);
			}
			try
			{
				switch (_icharge.Gateway)
				{
					case IchargeGateways.gwAuthorizeNet:
						if (_icharge.TestMode)
						{
							_icharge.AddSpecialField("x_tran_key", _icharge.MerchantPassword);
						}
						AddConfigField("AIMHashSecret");
						break;
					case IchargeGateways.gwPlanetPayment:
					case IchargeGateways.gwMPCS:
					case IchargeGateways.gwRTWare:
					case IchargeGateways.gwECX:
						//AddSpecialField(info, " x_tran_key");
						AddConfigField("AIMHashSecret");
						break;
                    //case IchargeGateways.gwViaKlix:
                    //    AddSpecialField("ssl_user_id");
                    //    break;
					case IchargeGateways.gwBankOfAmerica:
						_icharge.AddSpecialField("ecom_payment_card_name", info.CreditCardCustomerName);
						AddConfigField("referer");
						break;
					case IchargeGateways.gwInnovative:
						AddSpecialField("test_override_errors");
						break;
					case IchargeGateways.gwTrustCommerce:
					case IchargeGateways.gw3DSI:
						_icharge.TransactionAmount = _icharge.TransactionAmount.Replace(".", "");
						break;
					case IchargeGateways.gwPayFuse:
						AddConfigField("MerchantAlias");
						_icharge.TransactionAmount = _icharge.TransactionAmount.Replace(".", "");
						break;
					case IchargeGateways.gwYourPay:
					case IchargeGateways.gwFirstData:
					case IchargeGateways.gwLinkPoint:
						_icharge.SSLCert.Store = Settings["SSLCertStore"];
						_icharge.SSLCert.Subject = Settings["SSLCertSubject"];
						_icharge.SSLCert.Encoded = Settings["SSLCertEncoded"];
						break;
					case IchargeGateways.gwPRIGate:
						_icharge.MerchantPassword = Settings["MerchantPassword"];
						break;
                    //case IchargeGateways.gwProtx:
                    //    AddSpecialField("RelatedSecurityKey");
                    //    AddSpecialField("RelatedVendorTXCode");
                    //    AddSpecialField("RelatedTXAuthNo");
                    //    break;
                    //case IchargeGateways.gwOptimal:
                    //    _icharge.MerchantPassword = Settings["MerchantPassword"];
                    //    AddSpecialField("account");
                    //    break;
                    //case IchargeGateways.gwEFSNet:
                    //    _icharge.AddSpecialField("OriginalTransactionAmount", _icharge.TransactionAmount);
                    //    break;
                    //case IchargeGateways.gwPayStream:
                    //    AddSpecialField("CustomerID");
                    //    AddSpecialField("ZoneID");
                    //    AddSpecialField("Username");
                    //    break;
					case IchargeGateways.gwPayFlowPro:
						// for testing purpose uncomment line below   
						//_icharge.GatewayURL = "test-payflow.verisign.com";
						_icharge.AddSpecialField("user", Settings["MerchantLogin"]);
						break;
					case IchargeGateways.gwMoneris:
						// for testing purpose uncomment line below
						//_icharge.GatewayURL = "https://esqa.moneris.com/HPPDP/index.php";
						_icharge.TransactionAmount = transactionAmount.ToString("##0.00");
						break;
					case IchargeGateways.gwBeanstream:
						break;
				}

				_icharge.TransactionDesc = String.Format("Order Number {0}", _icharge.TransactionId);
				_icharge.OnSSLServerAuthentication += PaymentGateway_OnSSLAuthentication;


				switch (transactionType)
				{
					case TransactionType.Authorization:
						_icharge.AuthOnly();
						break;
					case TransactionType.Capture:
						_icharge.Capture(transactionId, _icharge.TransactionAmount);
						break;
					case TransactionType.Credit:
                        _icharge.Refund(transactionId, _icharge.TransactionAmount);
						break;
					case TransactionType.Sale:
						_icharge.Sale();
						break;
					case TransactionType.Void:
						_icharge.VoidTransaction(transactionId);
						break;
				}

				//_icharge.Sale();

				var approved = _icharge.Response.Approved;
				if (!approved)
				{
					payment.Status = PaymentStatus.Denied.ToString();
					message = "Transaction Declined: " + _icharge.Response.Text;
					return false;
				}

			}
			catch (Exception ex)
			{
				payment.Status = PaymentStatus.Failed.ToString();
				throw new ApplicationException(ex.Message);
			}

			info.StatusCode = _icharge.Response.Code;
			info.StatusDesc = _icharge.Response.Text;
			info.ValidationCode = _icharge.Response.TransactionId;
            info.AuthorizationCode = _icharge.Response.ApprovalCode;

			// transaction is marked as completed every time the payment operation succeeds even if it is void transaction type
			if (_icharge.Response.Approved)
			{
				payment.Status = PaymentStatus.Completed.ToString();
			}

			return true;
		}

		/// <summary>
		/// Adds the special field.
		/// </summary>
		/// <param name="name">The name.</param>
		private void AddSpecialField(string name)
		{
			if (!Settings.ContainsKey(name))
			{
				return;
			}
			var val = Settings[name];
			if (!string.IsNullOrEmpty(val))
				_icharge.AddSpecialField(name, val);
		}

		/// <summary>
		/// Adds the config field.
		/// </summary>
		/// <param name="name">The name.</param>
		private void AddConfigField(string name)
		{
			if (!Settings.ContainsKey(name))
			{
				return;
			}
			var val = Settings[name];

			if (!string.IsNullOrEmpty(val))
			{
				_icharge.Config(String.Format("{0}={1}", name, val));
			}
		}

		/// <summary>
		/// Handles the OnSSLAuthentication event of the PaymentGateway control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="IchargeSSLServerAuthenticationEventArgs"/> instance containing the event data.</param>
		private void PaymentGateway_OnSSLAuthentication(object sender, IchargeSSLServerAuthenticationEventArgs e)
		{
			e.Accept = true;
		}
	}
}
