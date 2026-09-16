using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security.SignInLog;

public class UserSignInLogSearchCriteria : SearchCriteriaBase
{
    public string UserId { get; set; }

    public bool? Succeeded { get; set; }

    public string[] SignInTypes { get; set; }

    public string[] FailureReasons { get; set; }

    public string IpAddress { get; set; }

    public string StoreId { get; set; }

    /// <summary>
    /// When true, matches only rows with no store at all, and <see cref="StoreId"/> is ignored.
    /// A store cannot be expressed as "no store" through <see cref="StoreId"/> itself, because an
    /// empty value there already means "any store" — which is how back-office sign-ins and failed
    /// attempts against unknown user names would otherwise be unfindable.
    /// </summary>
    public bool? WithoutStore { get; set; }

    public string OrganizationId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
