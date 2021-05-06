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
            var stopWatch = Stopwatch.StartNew();
            var result = Handler(@event, cancellationToken);
            stopWatch.Stop();

            Logger.LogInformation($"event:{EventName} module:{HandlerModuleName} overall_elapsed:{stopWatch.ElapsedMilliseconds}");

            return result;
        }
    }
}
