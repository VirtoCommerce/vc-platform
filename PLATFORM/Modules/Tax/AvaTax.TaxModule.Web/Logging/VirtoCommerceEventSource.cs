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
            public bool isCommit { get; set; }
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
            public const int Startup = 2;
            public const int Ping = 3;
            public const int ValidateAddress = 4;

            public const int ApplicationError = 1001;
            public const int TaxCalculationError = 1000;
            public const int TaxPingError = 1002;
            public const int AddressValidationError = 1003;

            public const int GetTaxRequestTime = 2000;
            public const int GetSalesInvoiceRequestTime = 2001;
        }

        private static readonly VirtoCommerceEventSource _log = new VirtoCommerceEventSource();
        public static VirtoCommerceEventSource Log { get { return _log; } }
        
        [Event(EventCodes.Startup, Message = "Starting up.", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
        public void Startup()
        {
            this.WriteEvent(EventCodes.Startup);
        }

        [Event(EventCodes.Ping, Message = "Test connection passed. Duration {2} ms.", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
        public void Ping(string startTime, string endTime, double duration)
        {
            this.WriteEvent(EventCodes.Ping, startTime, endTime, duration);
        }

        [Event(EventCodes.ValidateAddress, Message = "Address validated successfully. Duration {2} ms.", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
        public void ValidateAddress(string startTime, string endTime, double duration)
        {
            this.WriteEvent(EventCodes.ValidateAddress, startTime, endTime, duration);
        }

        [Event(EventCodes.ApplicationError, Message = "Application Failure: {0}", Level = EventLevel.Critical, Keywords = Keywords.Diagnostic)]
        public void ApplicationError(string error)
        {
            this.WriteEvent(EventCodes.ApplicationError, error);
        }
        
        [Event(EventCodes.TaxCalculationError, Message = "{0} - {1}. Error message: {2}", Level = EventLevel.Error, Keywords = Keywords.Diagnostic)]
        public void TaxCalculationError(string docCode, string docType, string error)
        {
            this.WriteEvent(EventCodes.TaxCalculationError, docCode, docType, error);
        }

        [Event(EventCodes.TaxPingError, Message = "AvaTax ping failed. Error message: {0}", Level = EventLevel.Error, Keywords = Keywords.Diagnostic)]
        public void TaxPingError(string error)
        {
            this.WriteEvent(EventCodes.TaxPingError, error);
        }

        [Event(EventCodes.AddressValidationError, Message = "Address validation failed. Error message: {0}", Level = EventLevel.Error, Keywords = Keywords.Diagnostic)]
        public void AddressValidationError(string error)
        {
            this.WriteEvent(EventCodes.AddressValidationError, error);
        }

        [Event(EventCodes.GetTaxRequestTime, Message = "{0} - {1}. Duration {4} ms. AvaTax tax request executed successfully.", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void GetTaxRequestTime(string docCode, string docType, string startTime, string endTime, double duration)
        {
            this.WriteEvent(EventCodes.GetTaxRequestTime, docCode, docType, startTime, endTime, duration);
        }

        [Event(EventCodes.GetSalesInvoiceRequestTime, Message = "{0} - {1}. Commit - {5}. Duration {4} ms. AvaTax tax request executed successfully.", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void GetSalesInvoiceRequestTime(string docCode, string docType, bool isCommit, string startTime, string endTime, double duration)
        {
            this.WriteEvent(EventCodes.GetSalesInvoiceRequestTime, docCode, docType, startTime, endTime, duration, isCommit);
        }
    }
}