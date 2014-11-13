using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using VirtoCommerce.Slab.Constants;
using VirtoCommerce.Slab.Contexts;

namespace VirtoCommerce.Slab.EventSources
{
	[EventSource(Name = VCEventSources.Base)]
	public class VirtoCommerceEventSource : EventSource
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
			public const int TaskFailure = 1;
			public const int Startup = 2;
			public const int PaymentTaskFailure = 1000;
			public const int ApplicationError = 1001;
			public const int KlarnaPaymentActivationCompleted = 1010;
			public const int KlarnaPaymentCancellationCompleted = 1020;
			public const int KlarnaPaymentReservationCompleted = 1030;
		}

        private static readonly VirtoCommerceEventSource _log  = new VirtoCommerceEventSource();
		public static VirtoCommerceEventSource Log { get { return _log; } }

		[Event(EventCodes.TaskFailure, Message = "Task Failure: {0}, task: {1}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
		public void TaskFailure(string message, string taskName)
		{
			this.WriteEvent(EventCodes.TaskFailure, message, taskName);
		}

		[Event(EventCodes.Startup, Message = "Starting up.", Keywords = Keywords.Diagnostic,	Level = EventLevel.Informational)]
		public void Startup()
		{
			this.WriteEvent(EventCodes.Startup);
		}

        [Event(EventCodes.PaymentTaskFailure, Message = "Payment gateway failure: {3}, task: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
        public void PaymentTaskFailure(string task, string orderId, string orderTN, string message, string exception)
        {
            this.WriteEvent(EventCodes.PaymentTaskFailure, task, orderId, orderTN, message, exception);
        }

        [Event(EventCodes.ApplicationError, Message = "Application Failure: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
        public void ApplicationError(string error)
        {
            this.WriteEvent(EventCodes.ApplicationError, error);
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
