using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
