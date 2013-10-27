using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    public class TraceContext : ITraceContext
    {
        private readonly TraceContextConfiguration configuration;
        private readonly string header;
        private readonly TraceBuffer traceBuffer;
        private readonly TraceSource traceSource;
        public TraceContext(
            TraceContextConfiguration configuration, 
            ContextName contextName,
            Guid corellationToken,
            TraceSource traceSource)
        {
            this.configuration = configuration;
            this.header = contextName.Value + "#" + corellationToken.ToString("D");
            this.traceBuffer = new TraceBuffer(
                (message, eventType, date) => traceSource.TraceEvent(
                    eventType, 0, header + " Trace ("+date.ToString(CultureInfo.InvariantCulture)+"):"+message)
                );
            this.traceSource = traceSource;
            if (this.configuration.BufferizeCatchExceptionAndFlash)
                this.trace = (m,e) => traceBuffer.Trace(m, TraceEventType.Information);
            else
                this.trace = (m, e) => traceSource.TraceEvent(e, 0, header + " Trace " + m);
                
        }

        public TraceContext(string traceSourceName)
        {
            this.traceSource = new TraceSource(traceSourceName);
            this.configuration = new TraceContextConfiguration
            {
                Activity = true,
                Trace = false,
                BufferizeCatchExceptionAndFlash = false,
                Configs = new Dictionary<string, string>()
            };
            this.header = "DefaultContext";
            this.traceBuffer = new TraceBuffer(
                (message, eventType, date) => traceSource.TraceEvent(
                    eventType, 0, header + " Trace (" + date.ToString(CultureInfo.InvariantCulture) + "):" + message)
                );
            if (!this.configuration.BufferizeCatchExceptionAndFlash)
                this.trace = (m,e) => traceSource.TraceEvent(e, 0, header + " Trace " + m);
            else
                this.trace = (m,e) => traceBuffer.Trace(m, e);
        }

        private DateTime startDateTime;

        public void FlashTraceBuffer()
        {
            this.traceBuffer.Flash();
        }

        public void ActivityStart()
        {
            if (configuration.Activity)
            {
                startDateTime = DateTime.Now;
                traceSource.TraceEvent(TraceEventType.Start, 0, header + " " + "Started at " + startDateTime.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void ActivityFinish(bool success)
        {
            if (configuration.Activity)
            {
                var duration = (DateTime.Now - startDateTime);
                var durationText = Math.Floor(duration.TotalMinutes) + "m." + duration.Seconds + "s." + duration.Milliseconds;
                if (success)
                    traceSource.TraceEvent(TraceEventType.Stop, 0, header + " " + "Finished! Duration=" + durationText);
                else
                    traceSource.TraceEvent(TraceEventType.Stop, 0, header + " " + "Finished with Error! Duration=" + durationText);
            }
        }

        private readonly Action<string, TraceEventType> trace;
        public bool IsTraceEnabled
        {
            get { return trace != null; }
        }

        public void Trace(string message)
        {
            if (configuration.Trace)
                trace(message, TraceEventType.Information);
        }

        public void Error(string message)
        {
            //if (configuration.Trace)
            trace(message,TraceEventType.Error);
        }

        public Action<T> PerformanceCounter<T>(string name)
        {
            return null;
        }

        public T ResolveConfig<T>() where T : IResolvableConfig, new()
        {
            string propertiesText;
            var @value = new T();
            bool success = configuration.Configs.TryGetValue(typeof(T).Name, out propertiesText);
            if (success)
            {
                Dictionary<string, string> propertiesDictionary = PairsParser.Parse(propertiesText);
                @value.Initialize(propertiesDictionary);
            }
            return @value;
        }
    }
}