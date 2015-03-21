using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    public class TraceContext : ITraceContext
    {
        private readonly TraceContextConfiguration _configuration;
        private readonly string _header;
        private readonly TraceBuffer _traceBuffer;
        private readonly TraceSource _traceSource;
        private readonly Action<string, TraceEventType> _trace;
        private DateTime _startDateTime;

        public TraceContext(
            TraceContextConfiguration configuration, 
            ContextName contextName,
            Guid correlationToken,
            TraceSource traceSource)
        {
            _configuration = configuration;
            _header = string.Format("{0}#{1}", contextName.Value, correlationToken.ToString("D"));
            _traceBuffer = new TraceBuffer((message, eventType, date) => traceSource.TraceEvent(eventType, 0, 
                string.Format("{0} Trace ({1}):{2}", _header, date.ToString(CultureInfo.InvariantCulture), message)));
            _traceSource = traceSource;

            if (_configuration.BufferizeCatchExceptionAndFlash)
            {
                _trace = (m, e) => _traceBuffer.Trace(m, TraceEventType.Information);
            }
            else
            {
                _trace = (m, e) => traceSource.TraceEvent(e, 0, string.Format("{0} Trace {1}", _header, m));
            }

        }

        public TraceContext(string traceSourceName)
        {
            _traceSource = new TraceSource(traceSourceName);
            _configuration = new TraceContextConfiguration
            {
                Activity = true,
                Trace = false,
                BufferizeCatchExceptionAndFlash = false,
                Configs = new Dictionary<string, string>()
            };
            _header = "DefaultContext";
            _traceBuffer = new TraceBuffer((message, eventType, date) => _traceSource.TraceEvent(eventType, 0, 
                string.Format("{0} Trace ({1}):{2}", _header, date.ToString(CultureInfo.InvariantCulture), message)));
            if (!_configuration.BufferizeCatchExceptionAndFlash)
            {
                _trace = (m, e) => _traceSource.TraceEvent(e, 0, string.Format("{0} Trace {1}", _header, m));
            }
            else
            {
                _trace = (m, e) => _traceBuffer.Trace(m, e);
            }
        }

        public void FlashTraceBuffer()
        {
            _traceBuffer.Flash();
        }

        public void ActivityStart()
        {
            if (_configuration.Activity)
            {
                _startDateTime = DateTime.Now;
                _traceSource.TraceEvent(TraceEventType.Start, 0, string.Format("{0} Started at {1}",_header,_startDateTime.ToString(CultureInfo.InvariantCulture)));
            }
        }

        public void ActivityFinish(bool success)
        {
            if (_configuration.Activity)
            {
                var duration = DateTime.Now - _startDateTime;
                var durationText = string.Format("{0}m. {1}s. {2}", Math.Floor(duration.TotalMinutes), duration.Seconds, duration.Milliseconds);
                _traceSource.TraceEvent(TraceEventType.Stop, 0, success
                                            ? string.Format("{0} Finished! Duration={1}", _header, durationText)
                                            : string.Format("{0} Finished with Error! Duration={1}", _header, durationText));
            }
        }

        public bool IsTraceEnabled
        {
            get { return _trace != null; }
        }

        public void Trace(string message)
        {
            if (_configuration.Trace)
                _trace(message, TraceEventType.Information);
        }

        public void Error(string message)
        {
            //if (configuration.Trace)
            _trace(message,TraceEventType.Error);
        }

        public Action<T> PerformanceCounter<T>(string name)
        {
            return null;
        }

        public T ResolveConfig<T>() where T : IResolvableConfig, new()
        {
            string propertiesText;
            var value = new T();
            var success = _configuration.Configs.TryGetValue(typeof(T).Name, out propertiesText);
            if (success)
            {
                var propertiesDictionary = PairsParser.Parse(propertiesText);
                value.Initialize(propertiesDictionary);
            }
            return value;
        }
    }
}