namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Cancellation token abstraction.
    /// </summary>
    /// <remarks>
    /// Normally we shouldn't have to use this, but the problem is that Hangfire has a IJobCancellationToken.
    /// This token contiains an System.Threading.CancellationToken, but that token is only invoked on shutdown and not on job deletion.
    /// To detect the job deletion, we need to rely on the ThrowIfCancellationRequested method on IJobCancellationToken.
    /// This abstraction allows us not to depend on Hangfire directly.
    /// </remarks>
    public interface ICancellationToken
    {
        void ThrowIfCancellationRequested();
    }
}
