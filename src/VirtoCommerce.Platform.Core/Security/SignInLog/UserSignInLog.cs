using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security.SignInLog;

/// <summary>
/// One sign-in attempt — successful or failed, including login-on-behalf.
/// Display names are denormalized on purpose: an audit row records what was true at the time,
/// so a later rename must not silently re-attribute history.
/// </summary>
public class UserSignInLog : Entity
{
    public DateTime CreatedDate { get; set; }

    /// <summary>User name exactly as submitted. The only identity field on unknown-user attempts.</summary>
    public string UserName { get; set; }

    /// <summary>Null when the account does not exist.</summary>
    public string UserId { get; set; }

    public bool Succeeded { get; set; }

    /// <summary>One of <see cref="SignInFailureReason"/>. Null when <see cref="Succeeded"/>.</summary>
    public string FailureReason { get; set; }

    /// <summary>One of <see cref="SignInLog.SignInType"/>.</summary>
    public string SignInType { get; set; }

    /// <summary>External identity provider name.</summary>
    public string Provider { get; set; }

    public string OperatorUserId { get; set; }

    public string OperatorUserName { get; set; }

    public string IpAddress { get; set; }

    public string UserAgent { get; set; }

    public string ClientId { get; set; }

    /// <summary>OpenIddict authorization id — joins this row to a live row in Active Sessions.</summary>
    public string SessionId { get; set; }

    public string StoreId { get; set; }

    public string StoreName { get; set; }

    public string MemberId { get; set; }

    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }
}
