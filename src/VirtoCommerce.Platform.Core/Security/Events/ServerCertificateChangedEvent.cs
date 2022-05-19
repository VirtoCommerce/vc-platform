using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class ServerCertificateChangedEvent : GenericChangedEntryEvent<ServerCertificate>
    {
        public ServerCertificateChangedEvent(IEnumerable<GenericChangedEntry<ServerCertificate>> changedEntries) : base(changedEntries)
        {
        }
    }
}
