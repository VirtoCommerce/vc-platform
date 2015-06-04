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
			public const int Startup = 100;
			public const int ApplicationError = 10000;
			public const int TaskFailure = 10100;
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

        [Event(EventCodes.ApplicationError, Message = "Application Failure: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
        public void ApplicationError(string error)
        {
            this.WriteEvent(EventCodes.ApplicationError, error);
        }
	}
}
