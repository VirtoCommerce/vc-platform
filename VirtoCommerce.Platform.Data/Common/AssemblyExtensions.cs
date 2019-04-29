using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VirtoCommerce.Platform.Data.Common
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        /// <summary>
        /// Gets all assembly types with custom attribute specified.
        /// </summary>
        /// <param name="assembly">Assembly to get types</param>
        /// <param name="customAttributeType">Custom attribute type to check</param>
        /// <param name="inherited"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithAttribute(this Assembly assembly, Type customAttributeType, bool inherited)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (customAttributeType == null)
            {
                throw new ArgumentNullException(nameof(customAttributeType));
            }

            try
            {
                return assembly.GetTypes().Where(x => x.GetCustomAttributes(customAttributeType, inherited).Length > 0);
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(x => x.GetCustomAttributes(customAttributeType, inherited).Length > 0);
            }
        }
    }
}
