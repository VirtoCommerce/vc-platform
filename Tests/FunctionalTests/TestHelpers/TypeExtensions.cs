using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTests.TestHelpers
{
    internal static class TypeExtensions
    {
        public static T CreateInstance<T>(this Type type)
        {
            Contract.Requires(type != null);
            Contract.Requires(typeof(T).IsAssignableFrom(type));


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
