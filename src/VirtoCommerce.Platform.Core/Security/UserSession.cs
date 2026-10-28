using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security;

public class UserSession : Entity
{
    public string SessionGroupId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string IpAddress { get; set; }

    public string UserAgent { get; set; }

    /// <summary>True when this session was created by the login-on-behalf grant.</summary>
    public bool IsImpersonated { get; set; }

    /// <summary>The operator acting on behalf of the user, when <see cref="IsImpersonated"/>.</summary>
    public string OperatorUserId { get; set; }

    public string OperatorUserName { get; set; }
}
