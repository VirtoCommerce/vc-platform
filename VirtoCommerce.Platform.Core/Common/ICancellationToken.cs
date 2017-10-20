namespace VirtoCommerce.Platform.Core.Common
{
    public interface ICancellationToken
    {
        void ThrowIfCancellationRequested();
    }
}
