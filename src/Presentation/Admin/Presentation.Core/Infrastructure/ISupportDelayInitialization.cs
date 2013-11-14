
namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface ISupportDelayInitialization
    {
        void InitializeForOpen();
        bool IsInitializing { get; set; }
    }
}
