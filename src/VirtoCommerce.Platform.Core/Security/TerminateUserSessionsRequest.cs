using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security;

public class TerminateUserSessionsRequest
{
    public string UserId { get; set; }

    public IList<string> ExcludedSessionGroupIds { get; set; }
}
