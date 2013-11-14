using System.Collections.Generic;
using VirtoCommerce.Scheduling.LogicalCall;

namespace VirtoCommerce.Scheduling
{
    public interface IJobContext
    {
        ITraceContext TraceContext { get; }
        IDictionary<string,string> Parameters { get; }
    }
    
    public class JobContext: IJobContext
    {

        public JobContext(ITraceContext traceContext, IDictionary<string, string> parameters)
        {
            Parameters = parameters;
            TraceContext = traceContext;
        }

        public ITraceContext TraceContext { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }
    }
}
