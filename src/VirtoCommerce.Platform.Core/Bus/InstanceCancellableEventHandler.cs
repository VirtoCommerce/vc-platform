using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Bus
{
    // A cancellable handler instance registered directly through IEventHandlerRegistrar, subscribed to TEvent.
    internal sealed class InstanceCancellableEventHandler<TEvent> : EventHandlerRegistration
        where TEvent : IEvent
    {
        private readonly ICancellableEventHandler<TEvent> _handler;

        public InstanceCancellableEventHandler(ICancellableEventHandler<TEvent> handler)
        {
            _handler = handler;
        }

        public override Type EventType => typeof(TEvent);

        public override Type HandlerType => _handler.GetType();

        public override Task Handle(IEvent @event, CancellationToken cancellationToken)
        {
            return _handler.Handle((TEvent)@event, cancellationToken);
        }
    }
}
