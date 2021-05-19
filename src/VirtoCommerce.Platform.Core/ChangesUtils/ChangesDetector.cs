using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ChangesUtils
{
    public static class ChangesDetector
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<ChangesDetectorPropertyInfo>> _changesDetectorPropertyInfosCache = new ConcurrentDictionary<Type, IEnumerable<ChangesDetectorPropertyInfo>>();

        public static ListDictionary<string, string> Gather(object newObj, object oldObj)
        {
            var result = new ListDictionary<string, string>();

            var objType = newObj.GetType();
            if (objType.Equals(oldObj.GetType()))
            {
                // Should collect all the getters marked by WithChangesDetectorAttribute and store into the cache
                var infos = _changesDetectorPropertyInfosCache.GetOrAdd(objType, _ =>
                    _.FindPropertiesWithAttribute(typeof(UseInChangesDetectorAttribute)).Select(
                    x => new ChangesDetectorPropertyInfo()
                    {
                        PropertyName = x.Name,
                        ChangeKey = x.GetCustomAttribute<UseInChangesDetectorAttribute>().ChangeKey,
                        Getter = x.GetGetMethod()
                    })
                );

                foreach (var info in infos)
                {
                    var newValue = info.Getter.Invoke(newObj, new object[0]);
                    var oldValue = info.Getter.Invoke(oldObj, new object[0]);

                    if (!newValue.Equals(oldValue))
                    {
                        result.Add(info.ChangeKey, $"Changes: {info.PropertyName}: {oldValue} -> {newValue}");
                    }
                }
            }

            return result;
        }
    }
}
