using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    internal sealed class EventHandlerRegistration
    {
        public EventHandlerRegistration(Type eventType, Type handlerType, object handler, Func<IMessage, CancellationToken, Task> invoke)
        {
            EventType = eventType;
            HandlerType = handlerType;
            Handler = handler;
            Invoke = invoke;
        }

        public Type EventType { get; }

        // The type the handler was registered as. For a scoped stand-in this is the type it resolves, not the stand-in itself.
        public Type HandlerType { get; }

        public object Handler { get; }

        public Func<IMessage, CancellationToken, Task> Invoke { get; }
    }
}
