using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.PlatformTools
{
    public class VirtoCommerceTraceSource : ILogger
    {
        readonly public TraceSource[] TraceSources;
        public VirtoCommerceTraceSource(params string[] traceSourceNames)
        {
            var length = traceSourceNames == null ? 0 : traceSourceNames.Length;
            TraceSources = new TraceSource[length + 1];
            TraceSources[0] = new TraceSource("VirtoCommerce");
            if (traceSourceNames != null)
            {
                int i = 1;
                foreach (var sourceName in traceSourceNames)
                {
                    TraceSources[i] = new TraceSource(sourceName);
	                i++;
                }
            }
        }

        public void TraceEvent(TraceEventType eventType, string message)
        {
            Array.ForEach(TraceSources, ts => ts.TraceEvent(eventType, 0, message));
        }

        public void Error(string message)
        {
            Array.ForEach(TraceSources, ts => ts.TraceEvent(TraceEventType.Error, 0, message));
        }

        public void Info(string message)
        {
            Array.ForEach(TraceSources, ts => ts.TraceEvent(TraceEventType.Information, 0, message));
        }

        public void Warning(string message)
        {
            Array.ForEach(TraceSources, ts => ts.TraceEvent(TraceEventType.Warning, 0, message));
        }
    }

    public class TraceContext : ILogger
    {
        readonly VirtoCommerceTraceSource traceSource;
        readonly string header;
        public TraceContext(VirtoCommerceTraceSource traceSource, string logicalCallName)
        {
            this.header = logicalCallName+"#"+Guid.NewGuid().ToString("N");
            this.traceSource = traceSource;
        }

        DateTime startDateTime;
        public void Start()
        {
            startDateTime = DateTime.Now;
            traceSource.TraceEvent(TraceEventType.Start, header+" START");
        }

        public void Stop(bool errorFlag)
        {
            var duration = (DateTime.Now - startDateTime);
            var durationText = Math.Floor(duration.TotalMinutes) + "m" + duration.Seconds + "s" + duration.Milliseconds+"ms";
            traceSource.TraceEvent(TraceEventType.Stop, header + " STOP after" + durationText + ((errorFlag) ? " ON ERROR" : ""));
        }

        public void TraceEvent(TraceEventType eventType, string message)
        {
            traceSource.TraceEvent(eventType, message);
        }

        public void Error(string message)
        {
            traceSource.Error(message);
        }

        public void Info(string message)
        {
            traceSource.Info(message);
        }

        public void Warning(string message)
        {
            traceSource.Warning(message);
        }
    }
}
