using System;
using System.Collections.Generic;
using System.Linq;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.PaymentGateways
{
    public class PaypalPaymentGateway : PaymentGatewayBase
    {
        /// <summary>
        /// Processes the payment. Can be used for both positive and negative transactions.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public override bool ProcessPayment(Payment payment, ref string message)
        {
            try
            {
                payment.Status = PaymentStatus.Processing.ToString();

                //ContractId - used as paypal PayerID
                var payerId = payment.ContractId;
                //ValidationCode - used as paypal token
                var token = payment.AuthorizationCode;

                if (string.IsNullOrEmpty(payerId) || string.IsNullOrEmpty(token))
                {
                    return false;
                }

                // Create the PayPalAPIInterfaceServiceService service object to make the API call
                var service = new PayPalAPIInterfaceServiceService((Dictionary<string, string>)Settings);

                var getECWrapper = new GetExpressCheckoutDetailsReq();
                getECWrapper.GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType(token);
                var getECResponse = service.GetExpressCheckoutDetails(getECWrapper);

                var request = new DoExpressCheckoutPaymentRequestType();
                var requestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
                request.DoExpressCheckoutPaymentRequestDetails = requestDetails;

                requestDetails.PaymentDetails = getECResponse.GetExpressCheckoutDetailsResponseDetails.PaymentDetails;
                requestDetails.Token = token;
                requestDetails.PayerID = payerId;
                requestDetails.PaymentAction = PaymentActionCodeType.SALE;

                // Invoke the API
                var wrapper = new DoExpressCheckoutPaymentReq();
                wrapper.DoExpressCheckoutPaymentRequest = request;

                var doECResponse = service.DoExpressCheckoutPayment(wrapper);

                if (doECResponse.Ack.Equals(AckCodeType.FAILURE) || (doECResponse.Errors != null && doECResponse.Errors.Count > 0))
                {
                    return false;
                }
                else
                {
                    switch (doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].PaymentStatus)
                    {
                        case PaymentStatusCodeType.COMPLETED:
                            payment.Status = PaymentStatus.Completed.ToString();
                            break;
                        case PaymentStatusCodeType.INPROGRESS:
                            payment.Status = PaymentStatus.Processing.ToString();
                            break;
                        case PaymentStatusCodeType.DENIED:
                            payment.Status = PaymentStatus.Denied.ToString();
                            break;
                        case PaymentStatusCodeType.FAILED:
                            payment.Status = PaymentStatus.Failed.ToString();
                            break;
                        default:
                            payment.Status =
                                doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].PaymentStatus
                                    .ToString();
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }


            return true;
        }
    }
}
