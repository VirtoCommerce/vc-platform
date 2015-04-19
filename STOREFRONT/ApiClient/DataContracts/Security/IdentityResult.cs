using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Security
{
    public class IdentityResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}