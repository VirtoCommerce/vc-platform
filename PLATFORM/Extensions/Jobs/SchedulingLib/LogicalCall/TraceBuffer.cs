using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    public class TraceBuffer
    {
        private readonly Action<string, TraceEventType, DateTime> log;
        public TraceBuffer(Action<string, TraceEventType, DateTime> log)
        {
            this.log = log;
        }

        class TraceMessage
        {
            public TraceMessage(Func<string> getMessage, TraceEventType traceEventType, StackTrace stackTrace)
            {
                GetMessage = getMessage;
                TraceEventType = traceEventType;
                StackTrace = stackTrace;
                DateTime = DateTime.Now;
            }
            public DateTime DateTime { get; private set; }
            public TraceEventType TraceEventType { get; private set; }
            public Func<string> GetMessage { get; private set; }
            public StackTrace StackTrace { get; private set; }
        }

        private readonly Queue<TraceMessage> traceMessages = new Queue<TraceMessage>();

        private const int MaxBuffer = 20;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Trace(string message, TraceEventType traceEventType)
        {
            if (traceMessages.Count > MaxBuffer)
                traceMessages.Dequeue();
            traceMessages.Enqueue(new TraceMessage(() => message, traceEventType, new StackTrace(1, true)));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Trace(Func<string> func, TraceEventType traceEventType)
        {
            if (traceMessages.Count > MaxBuffer)
                traceMessages.Dequeue();
            traceMessages.Enqueue(new TraceMessage(func, traceEventType, new StackTrace(1, true)));
        }

        /// <summary>
        /// Note: Queue enumerating is not thread safe operation but a) session is per call living object (that mean all calls should be from one thread) b) Session.Trace method is marked as synchronized "just in case"
        /// </summary>
        public void Flash()
        {
            int i = 0;
            var count = traceMessages.Count;
            foreach (var m in traceMessages)
            {
                var stringBuilder = new StringBuilder(1024);
                i++;
                stringBuilder.Append("Trace item ").Append(i).Append(" of ")
                             .Append(count).Append(Environment.NewLine)
                             .Append(m.GetMessage()).Append(Environment.NewLine)
                             .Append("Stack Trace: ").Append(m.StackTrace);

                log(stringBuilder.ToString(), m.TraceEventType, m.DateTime);
            }
        }
    }
}