using System;

namespace VirtoCommerce.Scheduling.LogicalCall
{
        /// <summary>
        /// Special structure that plays a role of Closure in any enterprise component that requires advanced tracing
        /// </summary>
        public interface ITraceContext
        {
            bool IsTraceEnabled { get; }
            void Trace(string message);  
            Action<T> PerformanceCounter<T>(string name);  // "support performance counters", T there is type of counter's value
            T ResolveConfig<T>() where T : IResolvableConfig, new(); // configuration could point additional trace parameters
        }
}
