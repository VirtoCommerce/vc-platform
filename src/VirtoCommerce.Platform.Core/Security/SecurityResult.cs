using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security
{
    public class SecurityResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
