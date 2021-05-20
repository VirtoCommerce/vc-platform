using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Core.Utils.ChangeDetector
{
    /// <summary>
    /// Object changes gathering utility.
    /// </summary>
    public static class ChangesDetector
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<ChangesDetectorPropertyInfo>> _changesDetectorPropertyInfosCache = new ConcurrentDictionary<Type, IEnumerable<ChangesDetectorPropertyInfo>>();

        /// <summary>
        /// Gather changes info for objects by properties, defined in native type of objects.
        /// Comparable objects should have equal type.
        /// Properties to compare should be annotated with <see cref="DetectChangesAttribute Attribute"/>.
        /// </summary>
        /// <param name="newObj">An object</param>
        /// <param name="oldObj">An object compare to</param>
        /// <returns></returns>
        public static ListDictionary<string, string> Gather(object newObj, object oldObj)
        {
            var objType = newObj.GetType();

            if (!objType.Equals(oldObj.GetType()))
            {
                throw new PlatformException("Can't compare objects of different types.");

            }
            return Gather(newObj, oldObj, objType);
        }

        /// <summary>
        /// Gather changes info for objects by properties, defined in type objType.
        /// Properties to compare should be annotated with <see cref="DetectChangesAttribute Attribute"/>.
        /// </summary>        
        /// <param name="newObj">An object</param>
        /// <param name="oldObj">An object compare to</param>
        /// <param name="objType">Type which properties should be used to compare</param>
        /// <returns></returns>
        public static ListDictionary<string, string> Gather(object newObj, object oldObj, Type objType, bool inherit = true)
        {
            var result = new ListDictionary<string, string>();

            // Should collect all the getters marked by UseInChangesDetectorAttribute and store into the cache
            var infos = _changesDetectorPropertyInfosCache.GetOrAdd(objType, _ =>
                _.FindPropertiesWithAttribute(typeof(DetectChangesAttribute)).Select(
                x => new ChangesDetectorPropertyInfo()
                {
                    PropertyName = x.Name,
                    Inherited = !x.DeclaringType.Equals(_),
                    ChangeKey = x.GetCustomAttribute<DetectChangesAttribute>().ChangeKey,
                    Getter = x.GetGetMethod()
                })
            );

            foreach (var info in infos)
            {
                var newValue = info.Getter.Invoke(newObj, new object[0]);
                var oldValue = info.Getter.Invoke(oldObj, new object[0]);

                if (!object.Equals(newValue, oldValue) && (inherit || !inherit && !info.Inherited))
                {
                    result.Add(info.ChangeKey, $"Changes: {info.PropertyName}: {oldValue} -> {newValue}");
                }
            }
            return result;
        }
    }
}
