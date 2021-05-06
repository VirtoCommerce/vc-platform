using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    public sealed class HandlerWrapper
    {
        public string EventName { get; set; }

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

                Logger.LogInformation($"event:{EventName} module:{HandlerModuleName} overall_elapsed:{stopWatch.ElapsedMilliseconds}");
            });
        }
    }
}
