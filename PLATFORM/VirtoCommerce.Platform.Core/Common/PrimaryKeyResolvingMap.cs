using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Helper class used for resolving model object primary keys when it presisted in persistent infrastructure
    /// Used in model to db model converters
    /// </summary>
    public class PrimaryKeyResolvingMap 
    {
        private Dictionary<Entity, Entity> _resolvingMap = new Dictionary<Entity, Entity>();
   
        public void AddPair(Entity transientEntity, Entity persistentEntity)
        {
            _resolvingMap[transientEntity] = persistentEntity;
        }

        public void ResolvePrimaryKeys()
        {
            foreach(var pair in _resolvingMap)
            {
                if(pair.Key.IsTransient() && !pair.Value.IsTransient())
                {
                    pair.Key.Id = pair.Value.Id;
                }
            }
        }
    }
}
