using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    public class SecurityOptions : ISecurityOptions
    {
        private readonly string[] _nonEditableUsers;

        public SecurityOptions(string userNames)
        {
            if (!string.IsNullOrEmpty(userNames))
                _nonEditableUsers = userNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public IEnumerable<string> NonEditableUsers
        {
            get
            {
                return _nonEditableUsers;
            }
        }
    }
}
