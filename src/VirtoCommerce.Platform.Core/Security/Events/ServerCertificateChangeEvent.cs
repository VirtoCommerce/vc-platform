using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class ServerCertificateChangeEvent : GenericChangedEntryEvent<ServerCertificate>
    {
        public ServerCertificateChangeEvent(IEnumerable<GenericChangedEntry<ServerCertificate>> changedEntries) : base(changedEntries)
        {
        }
    }
}
