using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class ServerCertificateChangeEvent : GenericChangedEntryEvent<ServerCertificate>
    {
        public ServerCertificateChangeEvent(IEnumerable<GenericChangedEntry<ServerCertificate>> changedEntries) : base(changedEntries)
        {
        }
    }
}
