namespace VirtoCommerce.Platform.Core.DistributedLock
{
    /// <summary>
    /// Simple static lock resource with specified string key
    /// </summary>
    public class StaticDistributedLockResource : DistributedLockResourceBase
    {
        /// <summary>
        /// Construct static lock resource with specified string key
        /// </summary>
        /// <param name="resourceId"></param>
        public StaticDistributedLockResource(string resourceId)
        {
            ResourceId = resourceId;
        }
    }
}
