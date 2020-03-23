using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface ISupportSecurityScopes
    {
        IEnumerable<string> Scopes { get; set; }
    }
}
