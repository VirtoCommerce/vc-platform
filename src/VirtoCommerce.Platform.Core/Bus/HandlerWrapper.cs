using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    [DebuggerDisplay("{HandlerModuleName} {EventType.Name}")]
    public sealed class HandlerWrapper
    {
        public Type EventType { get; set; }

        // The type the handler was registered as; for instance registrations this is the runtime type.
        public Type HandlerType { get; set; }

        // The runtime type of the resolved handler, which differs from HandlerType when a derived type is registered for a base type.
        public Type ImplementationType { get; set; }

        public string HandlerModuleName { get; set; }

        public Func<IMessage, CancellationToken, Task> Handler { get; set; }

        public ILogger Logger { get; set; }

        public Task Handle(IEvent @event, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var stopWatch = Stopwatch.StartNew();
                Handler(@event, cancellationToken).GetAwaiter().GetResult();
                stopWatch.Stop();

                Logger.LogInformation("event:{Event} module:{Handler} overall_elapsed:{Elapsed}",
                    @event.GetType().Name, HandlerModuleName, stopWatch.ElapsedMilliseconds);
            }, cancellationToken);
        }
    }
}
