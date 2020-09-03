using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Events
{
    public interface IEvent : IEntity, IMessage
    {
        int Version { get; set; }
        DateTimeOffset TimeStamp { get; set; }
    }
}
