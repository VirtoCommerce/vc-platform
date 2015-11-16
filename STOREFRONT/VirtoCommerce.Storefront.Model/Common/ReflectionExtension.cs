using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public static class ReflectionExtension
    {
        public static IEnumerable<PropertyInfo> GetTypePropsRecursively(this Type baseType, Func<PropertyInfo, bool> predicate)
        {
            return IteratePropsInner(baseType, predicate);
        }

        private static IEnumerable<PropertyInfo> IteratePropsInner(Type baseType, Func<PropertyInfo, bool> predicate)
        {
            var props = baseType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in props.Where(x=> predicate(x)))
            {
                var type = ListTypesOrSelf(property.PropertyType);

                foreach (var info in IteratePropsInner(type, predicate))
                    yield return info;

                yield return property;
            }
        }

        public static Type ListTypesOrSelf(Type type)
        {
            if (!type.IsGenericType)
                return type;
            return type.GetGenericArguments()[0];
        }

    }
}
