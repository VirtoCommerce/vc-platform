using System;
using System.Linq;
using AuthorizeNet;
using VirtoCommerce.Foundation.Orders.Model;
using Order = VirtoCommerce.Foundation.Orders.Model.Order;

namespace VirtoCommerce.PaymentGateways
{
    public class AuthorizeNetPaymentGateway : PaymentGatewayBase
    {
        public override bool ProcessPayment(Payment payment, ref string message)
        {
            var info = payment as CreditCardPayment;

            if (ReferenceEquals(info, null))
            {
                payment.Status = PaymentStatus.Failed.ToString();
                message = "AuthorizeNet gateway supports only CreditCardPayment";
                return false;
            }

            string[] validateSettings = { "MerchantLogin", "MerchantPassword" };

            foreach (var validateSetting in validateSettings)
            {
                if (!Settings.ContainsKey(validateSetting) || string.IsNullOrWhiteSpace(Settings[validateSetting]))
                {
                    payment.Status = PaymentStatus.Failed.ToString();
                    message = string.Format("{0} not configured", validateSetting);
                    return false;
                }
            }

            var transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), info.TransactionType);
            payment.Status = PaymentStatus.Processing.ToString();

            var gateway = new Gateway(Settings["MerchantLogin"], Settings["MerchantPassword"]);

            bool isTestMode;
            if (Settings.ContainsKey("TestMode") && bool.TryParse(Settings["TestMode"], out isTestMode))
            {
                gateway.TestMode = isTestMode;
            }

            var description = string.Format("{0} transaction for order id {1}", transactionType, info.OrderForm.OrderGroupId);
            IGatewayRequest request = null;

            switch (transactionType)
            {
                case TransactionType.Authorization:
                case TransactionType.Sale:
                    request = new AuthorizationRequest(info.CreditCardNumber,
                        string.Format("{0}{1}", info.CreditCardExpirationMonth, info.CreditCardExpirationYear),
                        info.Amount,
                        description,
                        transactionType == TransactionType.Sale);
                    break;
                case TransactionType.Capture:
                    request = new PriorAuthCaptureRequest(info.Amount, info.ValidationCode);
                    break;
                case TransactionType.Credit:
                    request = new CreditRequest(info.ValidationCode, info.Amount, info.CreditCardNumber);
                    break;
                case TransactionType.Void:
                    request = new VoidRequest(info.ValidationCode);
                    break;
            }

            if (request == null)
            {
                payment.Status = PaymentStatus.Failed.ToString();
                message = string.Format("Unsupported transation type {0}", transactionType);
                return false;
            }

            request.AddCardCode(info.CreditCardSecurityCode);

            var invoice = info.OrderForm.OrderGroupId;

            var order = info.OrderForm.OrderGroup as Order;
            if (order != null)
            {
                invoice = order.TrackingNumber;
            }

            request.AddInvoice(invoice);
            request.AddTax(info.OrderForm.TaxTotal);

            // Find the address
            var address = info.OrderForm.OrderGroup.OrderAddresses
                .FirstOrDefault(a => String.Compare(a.OrderAddressId, info.BillingAddressId, StringComparison.OrdinalIgnoreCase) == 0);

            if (address != null)
            {
                request.AddCustomer(address.Email, address.FirstName, address.LastName,
                    address.Line1 + " " + address.Line2, address.StateProvince, address.PostalCode);
            }

            //foreach (var lineItem in info.OrderForm.LineItems)
            //{
            //    request.AddLineItem(lineItem.LineItemId, lineItem.DisplayName, lineItem.Description,
            //        (int)lineItem.Quantity, lineItem.PlacedPrice, false);
            //}


            try
            {
                var response = gateway.Send(request, description);

                if (!response.Approved)
                {
                    payment.Status = PaymentStatus.Denied.ToString();
                    message = "Transaction Declined: " + response.Message;
                    return false;
                }
                info.StatusCode = response.ResponseCode;
                info.StatusDesc = response.Message;
                info.ValidationCode = response.TransactionID;
                info.AuthorizationCode = response.AuthorizationCode;

                // transaction is marked as completed every time the payment operation succeeds even if it is void transaction type
                payment.Status = PaymentStatus.Completed.ToString();          
            }
            catch (Exception ex)
            {
                payment.Status = PaymentStatus.Failed.ToString();
                throw new ApplicationException(ex.Message);
            }

            return true;

        }
    }
}
