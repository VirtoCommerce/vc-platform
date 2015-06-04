using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Slab;
using VirtoCommerce.Slab.Constants;
using VirtoCommerce.Slab.Contexts;

namespace VirtoCommerce.PaymentGateways
{
	[EventSource(Name = VCEventSources.Base)]
	public class PaymentGatewaysEventSource : EventSource
	{
		public class Keywords
		{
			public const EventKeywords Page = (EventKeywords)VCKeywords.Web;
			public const EventKeywords DataBase = (EventKeywords)VCKeywords.DataBase;
			public const EventKeywords Diagnostic = (EventKeywords)VCKeywords.Diagnostic;
			public const EventKeywords Performance = (EventKeywords)VCKeywords.Performance;
		}

		public class Tasks
		{
			public const EventTask Page = (EventTask)1;
			public const EventTask DBQuery = (EventTask)2;
		}

		public class EventCodes
		{
			public const int PaymentOperationError = 11001;
			public const int PaymentOperationInfo = 11000;
			public const int KlarnaPaymentActivationCompleted = 11100;
			public const int KlarnaPaymentCancellationCompleted = 11200;
			public const int KlarnaPaymentReservationCompleted = 11300;
			public const int AuthorizeNetPaymentActivationCompleted = 11400;
			public const int AuthorizeNetPaymentCancellationCompleted = 11500;
			public const int AuthorizeNetPaymentReservationCompleted = 11600;
		}

		public class PaymentGatewaysContext: BaseSlabContext
		{
			public string orderId { get; set; }
			public string orderTN { get; set; }
			public string message { get; set; }
			public string paymentStatus { get; set; }
			public string billingAddress { get; set; }
			public string reservation { get; set; }
			public string statusDesc { get; set; }
			public string task { get; set; }
		}

		private static readonly PaymentGatewaysEventSource _log = new PaymentGatewaysEventSource();
		public static PaymentGatewaysEventSource Log { get { return _log; } }

		[Event(EventCodes.PaymentOperationError, Message = "Payment gateway failure: {3}, task: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
		public void PaymentOperationError(string task, string orderId, string orderTN, string message, string exception)
		{
			this.WriteEvent(EventCodes.PaymentOperationError, task, orderId, orderTN, message, exception);
		}

		[Event(EventCodes.PaymentOperationInfo, Message = "{2} - Payment gateway success: {3}, task: {0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
		public void PaymentOperationInfo(string task, string orderTN, string message)
		{
			this.WriteEvent(EventCodes.PaymentOperationInfo, task, orderTN, message);
		}
		
		[Event(EventCodes.KlarnaPaymentActivationCompleted, Message = "Klarna payment for order {1} activated.", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
		public void KlarnaPaymentActivationCompleted(string orderId, string orderTN, string paymentStatus, string statusDesc, string startTime, string endTime, double duration)
		{
			this.WriteEvent(EventCodes.KlarnaPaymentActivationCompleted, orderId, orderTN, paymentStatus, statusDesc, startTime, endTime, duration);
		}

		[Event(EventCodes.KlarnaPaymentCancellationCompleted, Message = "Klarna payment cancelled for order {1} with result: {3}", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
		public void KlarnaPaymentCancellationCompleted(string orderId, string orderTN, string reservation, bool result, string startTime, string endTime, double duration)
		{
			this.WriteEvent(EventCodes.KlarnaPaymentCancellationCompleted, orderId, orderTN, reservation, result, startTime, endTime, duration);
		}

		[Event(EventCodes.KlarnaPaymentReservationCompleted, Message = "Klarna payment reserved for order {1} with result: {3}", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
		public void KlarnaPaymentReservationCompleted(string orderId, string orderTN, string reservation, string status, string billingAddress, string startTime, string endTime, double duration)
		{
			this.WriteEvent(EventCodes.KlarnaPaymentReservationCompleted, orderId, orderTN, reservation, status, billingAddress, startTime, endTime, duration);
		}
	}
}
