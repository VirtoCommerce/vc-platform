using System;

namespace VirtoCommerce.Platform.Core.Security.SignInLog;

/// <summary>
/// One bucket of the sign-in timeline. Buckets are contiguous and gap-filled, so a quiet
/// stretch is a run of zeroes rather than a hole the chart would silently close up.
/// </summary>
public class UserSignInLogTimelinePoint
{
    public DateTime Timestamp { get; set; }

    public int SucceededCount { get; set; }

    public int FailedCount { get; set; }
}

/// <summary>
/// Bucket size for the timeline. Chosen from the requested window rather than by the caller,
/// so a 30-minute view and a 30-day view both come back with a readable number of points.
/// </summary>
public static class TimelineGranularity
{
    public const string Minute = "Minute";
    public const string TenMinutes = "TenMinutes";
    public const string Hour = "Hour";
    public const string Day = "Day";
}
