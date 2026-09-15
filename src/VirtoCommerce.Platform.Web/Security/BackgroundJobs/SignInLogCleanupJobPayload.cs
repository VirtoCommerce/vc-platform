namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    /// <summary>
    /// Payload for the recurring sign-in log cleanup job. Carries no data — the schedule and the
    /// retention setting are the only inputs — but exists so the job runs through the
    /// engine-agnostic message-based background-job pipeline.
    /// </summary>
    public sealed class SignInLogCleanupJobPayload
    {
    }
}
