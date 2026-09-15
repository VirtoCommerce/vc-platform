using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security.Search;

/// <summary>
/// Aggregates for the sign-in log blade. Every member is produced by a SQL aggregate against an
/// indexed column - never by materialising rows, which at ~18M rows a year would be a table scan.
/// </summary>
public class UserSignInLogStats
{
    public int TotalCount { get; set; }

    public int FailedCount { get; set; }

    public int DistinctUserCount { get; set; }

    public int ImpersonationCount { get; set; }

    public IList<UserSignInLogStatsEntry> TopFailedIpAddresses { get; set; } = [];

    public IList<UserSignInLogStatsEntry> TopFailedUserNames { get; set; } = [];

    public IList<UserSignInLogStatsEntry> FailureReasonBreakdown { get; set; } = [];

    /// <summary>
    /// Covers attempts against existing accounts only: an unknown user name has no organization.
    /// </summary>
    public IList<UserSignInLogStatsEntry> SignInsByOrganization { get; set; } = [];
}

public class UserSignInLogStatsEntry
{
    public string Key { get; set; }

    public int Count { get; set; }
}
