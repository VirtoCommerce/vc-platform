using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Events
{
    public class DomainEvent : Entity, IEvent
    {
        public DomainEvent()
        {
            Id = Guid.NewGuid().ToString();
            TimeStamp = DateTime.UtcNow;
        }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
