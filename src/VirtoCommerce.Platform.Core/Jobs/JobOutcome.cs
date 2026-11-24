namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// How a background job ended, as reported through <see cref="IJobProgress.Complete"/>.
/// </summary>
/// <remarks>
/// The values start at 1 so that an uninitialized <see cref="JobOutcome"/> is not mistaken for a success.
/// </remarks>
public enum JobOutcome
{
    Succeeded = 1,
    Failed = 2,
    Canceled = 3,
}
