using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace VirtoCommerce.Foundation.Frameworks
{
    /// <summary>
    /// Implements operations for and represents commerce class information.
    /// </summary>
    public class ClassHelper
    {
        private Type _Type;
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return _Type; }
        }

        private ConstructorInfo _DefaultConstructor;
        /// <summary>
        /// Gets the default constructor.
        /// </summary>
        /// <value>The default constructor.</value>
        public ConstructorInfo DefaultConstructor
        {
            get
            {
                return _DefaultConstructor;
            }
        }

        public ClassHelper(string type)
        {
            if (String.IsNullOrEmpty(type))
            {
                throw new TypeLoadException(String.Format("Type can't be null."));
            }

            _Type = Type.GetType(type);

            if (_Type == null)
            {
                throw new TypeLoadException(String.Format("Failed to load {0}.", type));
            }

            _DefaultConstructor = Type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }


        public ClassHelper(Type type)
        {
            _Type = type;

            if (_Type == null)
            {
                throw new TypeLoadException(String.Format("Failed to load {0}.", type));
            }

            _DefaultConstructor = Type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <returns></returns>
        public object CreateInstance()
        {
            return DefaultConstructor.Invoke(Type.EmptyTypes);
        }
    }
}
