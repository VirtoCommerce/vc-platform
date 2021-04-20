namespace VirtoCommerce.Platform.Core.DistributedLock
{
    public class StaticDistributedLockResource : DistributedLockResourceBase
    {
        public StaticDistributedLockResource(string resourceId)
        {
            ResourceId = resourceId;
        }
    }
}
