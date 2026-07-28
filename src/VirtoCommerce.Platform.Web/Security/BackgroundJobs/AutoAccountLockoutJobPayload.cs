namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    /// <summary>
    /// Payload for the recurring "auto account lockout" job. Carries no data — the schedule is the only input —
    /// but exists so the job runs through the engine-agnostic message-based background-job pipeline.
    /// </summary>
    public sealed class AutoAccountLockoutJobPayload
    {
    }
}
