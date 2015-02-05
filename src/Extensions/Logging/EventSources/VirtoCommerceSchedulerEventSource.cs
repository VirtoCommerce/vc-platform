using System.Diagnostics.Tracing;
using VirtoCommerce.Slab.Constants;

namespace VirtoCommerce.Slab.EventSources
{
    [EventSource(Name = VCEventSources.Base+"-scheduler")]
    public class VirtoCommerceSchedulerEventSource : EventSource
    {
        public class Keywords
        {
            public const EventKeywords Workflow = VCKeywords.Workflow;
            public const EventKeywords Jobs = VCKeywords.Jobs;

            public const EventKeywords Diagnostic = VCKeywords.Diagnostic;
            public const EventKeywords Performance = VCKeywords.Performance;
        }

        public class Tasks
        {
            public const EventTask DbQuery = VCTasks.DbQuery;
            public const EventTask Remote = VCTasks.Remote;
            public const EventTask IOAction = VCTasks.IOAction;
        }

		public class EventCodes
		{
			public const int Startup = 1;
			public const int Stop = 2;
			public const int UnexpectedFailure = 3;
			public const int SystemJobExecute = 4;
			public const int SchedulerBootstrapperInitialize = 5;
			public const int QueueFailure = 6;
			public const int LeaseReleaseFailure = 7;
			public const int GenerateSearchIndexJobCompleted = 8;
			public const int ProcessSearchIndexJobCompleted = 9;
			public const int OrderStatusChanged = 10;
			public const int WorkflowJobExecutes = 11;
		}

        private static readonly VirtoCommerceSchedulerEventSource _log = new VirtoCommerceSchedulerEventSource();
        public static VirtoCommerceSchedulerEventSource Log { get { return _log; } }

		[Event(EventCodes.Startup, Message = "Starting up.", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void Startup()
        {
            this.WriteEvent(EventCodes.Startup);
        }

		[Event(EventCodes.Stop, Message = "Stop.", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
		public void Stop()
		{
			this.WriteEvent(EventCodes.Stop);
		}

		[Event(EventCodes.UnexpectedFailure, Message = "Scheduler unexpected failure: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
        public void UnexpectedFailure(string message)
        {
			this.WriteEvent(EventCodes.UnexpectedFailure, message);
        }

		[Event(EventCodes.SystemJobExecute, Message = "System job executes: {0}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
		public void SystemJobExecute(string name)
		{
			this.WriteEvent(EventCodes.SystemJobExecute, name);
		}

		[Event(EventCodes.SchedulerBootstrapperInitialize, Message = "Scheduler bootstrapper initialize", Level = EventLevel.Verbose, Keywords = Keywords.Diagnostic)]
		public void SchedulerBootstrapperInitialize()
		{
			this.WriteEvent(EventCodes.SchedulerBootstrapperInitialize);
		}

		[Event(EventCodes.QueueFailure, Message = "Scheduler failure creating queue: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
		public void QueueFailure(string message)
		{
			this.WriteEvent(EventCodes.QueueFailure, message);
		}

		[Event(EventCodes.LeaseReleaseFailure, Message = "Scheduler lease release failure: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
		public void LeaseReleaseFailure(string message)
		{
			this.WriteEvent(EventCodes.LeaseReleaseFailure, message);
		}

		[Event(EventCodes.GenerateSearchIndexJobCompleted, Message = "Generate search index job completed, scope: {0}", Level = EventLevel.Verbose, Keywords = Keywords.Jobs)]
		public void GenerateSearchIndexJobCompleted(string name)
		{
			this.WriteEvent(EventCodes.GenerateSearchIndexJobCompleted, name);
		}


		[Event(EventCodes.ProcessSearchIndexJobCompleted, Message = "Process search index job completed, scope: {0}", Level = EventLevel.Verbose, Keywords = Keywords.Jobs)]
		public void ProcessSearchIndexJobCompleted(string name)
		{
			this.WriteEvent(EventCodes.ProcessSearchIndexJobCompleted, name);
		}

		[Event(EventCodes.OrderStatusChanged, Message = "Order status changed to 'InProgress', order TN: {0}", Level = EventLevel.Verbose, Keywords = Keywords.Jobs)]
		public void OrderStatusChanged(string orderTrackingNumber)
		{
			this.WriteEvent(EventCodes.OrderStatusChanged, orderTrackingNumber);
		}

		[Event(EventCodes.WorkflowJobExecutes, Message = "Workflow job executes, workflow name: {0}", Level = EventLevel.Verbose, Keywords = Keywords.Jobs)]
		public void WorkflowJobExecutes(string workflow)
		{
			this.WriteEvent(EventCodes.WorkflowJobExecutes, workflow);
		}
    }
}