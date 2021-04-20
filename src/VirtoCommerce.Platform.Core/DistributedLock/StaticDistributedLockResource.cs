using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.Platform.Core.DistributedLock
{
    public class StaticDistributedLockResource: DistributedLockResourceBase
    {
        public StaticDistributedLockResource(string resourceId)
        {
            ResourceId = resourceId;
        }
    }
}
