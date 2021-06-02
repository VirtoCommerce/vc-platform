using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Caching.GenericCrud
{
    public class GenericCrudCachingRegion<T> : CancellableCacheRegion<GenericCrudCachingRegion<T>> where T : Entity
    {
        public static IChangeToken CreateChangeToken(T[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            return CreateChangeToken(entities.Select(x => x.Id).ToArray());
        }

        public static IChangeToken CreateChangeToken(string[] entityIds)
        {
            if (entityIds == null)
            {
                throw new ArgumentNullException(nameof(entityIds));
            }

            var changeTokens = new List<IChangeToken> { CreateChangeToken() };
            foreach (var entityId in entityIds)
            {
                changeTokens.Add(CreateChangeTokenForKey(entityId));
            }
            return new CompositeChangeToken(changeTokens);
        }
    }
}
