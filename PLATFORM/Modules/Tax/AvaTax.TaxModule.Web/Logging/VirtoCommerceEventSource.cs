using System.Diagnostics.Tracing;

namespace AvaTax.TaxModule.Web.Logging
{
    [EventSource(Name = VCEventSources.Base)]
    public class VirtoCommerceEventSource : EventSource
    {
        #region Context

        public class TaxRequestContext : BaseSlabContext
        {
            public string docCode { get; set; }
            public string docType { get; set; }
            public string customerCode { get; set; }
            public double amount { get; set; }
        }

        #endregion

        public class Keywords
        {
            public const EventKeywords Page = VCKeywords.Web;
            public const EventKeywords DataBase = VCKeywords.DataBase;
            public const EventKeywords Diagnostic = VCKeywords.Diagnostic;
            public const EventKeywords Performance = VCKeywords.Performance;
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
            public const int ApplicationError = 1001;
            public const int GetTaxRequestData = 2001;
            public const int TaxCalculationError = 2100;
            public const int GetTaxRequestTime = 2000;
        }

        private static readonly VirtoCommerceEventSource _log = new VirtoCommerceEventSource();
        public static VirtoCommerceEventSource Log { get { return _log; } }

        [Event(EventCodes.TaskFailure, Message = "Task Failure: {0}, task: {1}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
        public void TaskFailure(string message, string taskName)
        {
            this.WriteEvent(EventCodes.TaskFailure, message, taskName);
        }

        [Event(EventCodes.Startup, Message = "Starting up.", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
        public void Startup()
        {
            this.WriteEvent(EventCodes.Startup);
        }

        [Event(EventCodes.ApplicationError, Message = "Application Failure: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
        public void ApplicationError(string error)
        {
            this.WriteEvent(EventCodes.ApplicationError, error);
        }

        [Event(EventCodes.GetTaxRequestData, Message = "DocCode - {0}, DocType - {1}, CustomerCode - {2}, Total - {3}", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void GetTaxRequestData(string docCode, string docType, string customerCode, double amount)
        {
            this.WriteEvent(EventCodes.GetTaxRequestData, docCode, docType, customerCode, amount);
        }

        [Event(EventCodes.TaxCalculationError, Message = "{0} - {1}. Error message: {3}", Level = EventLevel.Error, Keywords = Keywords.Diagnostic)]
        public void TaxCalculationError(string docCode, string docType, double amount, string message)
        {
            this.WriteEvent(EventCodes.TaxCalculationError, docCode, docType, amount, message);
        }

        [Event(EventCodes.GetTaxRequestTime, Message = "{0} - {1}. AvaTax Get tax executed successfully. Duration {4} ms.", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void GetTaxRequestTime(string docCode, string docType, string startTime, string endTime, double duration)
        {
            this.WriteEvent(EventCodes.GetTaxRequestTime, docCode, docType, startTime, endTime, duration);
        }

        
    }
}