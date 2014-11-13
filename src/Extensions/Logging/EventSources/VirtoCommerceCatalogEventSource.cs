using System.Diagnostics.Tracing;
using VirtoCommerce.Slab.Constants;

namespace VirtoCommerce.Slab.EventSources
{
	[EventSource(Name = VCEventSources.Base + "-Catalog")]
	public class VirtoCommerceCatalogEventSource : EventSource
	{
        private static readonly VirtoCommerceCatalogEventSource _log = new VirtoCommerceCatalogEventSource();
        public static VirtoCommerceCatalogEventSource Log { get { return _log; } }

        public class Keywords
        {
            public const EventKeywords Page = (EventKeywords)1;
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
			public const int Error = 1;
			public const int Init = 2;
		}

		[Event(EventCodes.Error, Message = "Application Failure: {0}",
			Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
		public void Error(string message)
		{
			if (this.IsEnabled())
			{
				this.WriteEvent(EventCodes.Error, message);
			}
		}

		[Event(EventCodes.Init, Message = "Starting up.", Keywords = Keywords.Performance, Level = EventLevel.Informational)]
		public void Init()
		{
			this.WriteEvent(EventCodes.Init);
		}
	}
}
