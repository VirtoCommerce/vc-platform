using System;

namespace VirtoCommerce.Platform.Tests.Helpers
{
    internal static class TypeExtensions
    {
        public static T CreateInstance<T>(this Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new ArgumentException(type + " must have parameterless constructor");
            }

            if (type.IsAbstract)
            {
                throw new ArgumentException(type + " is abstract");
            }

            if (type.IsGenericType)
            {
                throw new ArgumentException(type + " is generic");
            }

            return (T)Activator.CreateInstance(type);
        }
    }
}
