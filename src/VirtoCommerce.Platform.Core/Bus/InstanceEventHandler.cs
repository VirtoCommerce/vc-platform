using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Bus
{
    // A handler instance registered directly through IEventHandlerRegistrar, subscribed to TEvent.
    internal sealed class InstanceEventHandler<TEvent> : EventHandlerRegistration
        where TEvent : IEvent
    {
        private readonly IEventHandler<TEvent> _handler;

        public InstanceEventHandler(IEventHandler<TEvent> handler)
        {
            _handler = handler;
        }

        public override Type EventType => typeof(TEvent);

        public override Type HandlerType => _handler.GetType();

        public override Task Handle(IEvent @event, CancellationToken cancellationToken)
        {
            return _handler.Handle((TEvent)@event);
        }
    }
}
