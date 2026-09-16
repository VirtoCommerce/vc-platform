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

    public string OrganizationId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
