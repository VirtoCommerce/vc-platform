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

    /// <summary>
    /// Same-length window immediately before the requested one, so the UI can show movement
    /// rather than a bare number. Null when the criteria have no start date ("all time"),
    /// because there is then no previous period to compare against.
    /// </summary>
    public int? PreviousTotalCount { get; set; }

    public int? PreviousFailedCount { get; set; }

    public int? PreviousDistinctUserCount { get; set; }

    public int? PreviousImpersonationCount { get; set; }

    /// <summary>
    /// Contiguous, gap-filled buckets across the requested window. Empty when the criteria carry
    /// no start date, because "all time" has no bounded window to bucket.
    /// </summary>
    public IList<UserSignInLogTimelinePoint> Timeline { get; set; } = [];

    /// <summary>One of <see cref="Search.TimelineGranularity"/>. Null when the timeline is empty.</summary>
    public string TimelineGranularity { get; set; }

    public IList<UserSignInLogStatsEntry> TopFailedIpAddresses { get; set; } = [];

    public IList<UserSignInLogStatsEntry> TopFailedUserNames { get; set; } = [];

    public IList<UserSignInLogStatsEntry> FailureReasonBreakdown { get; set; } = [];

    /// <summary>
    /// Covers attempts against existing accounts only: an unknown user name has no organization.
    /// </summary>
    public IList<UserSignInLogStatsEntry> SignInsByOrganization { get; set; } = [];

    /// <summary>
    /// Stores present in the log, so the filter can offer real values. The platform has no store
    /// catalogue of its own, so the log is the only source for this list.
    /// </summary>
    public IList<UserSignInLogStatsEntry> SignInsByStore { get; set; } = [];
}

public class UserSignInLogStatsEntry
{
    public string Key { get; set; }

    public int Count { get; set; }
}
