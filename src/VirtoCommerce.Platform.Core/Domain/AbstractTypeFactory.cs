using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Represents factory that supports type overriding and provides special factory capabilities.
    /// </summary>
    /// <typeparam name="BaseType"></typeparam>
    public static class AbstractTypeFactory<BaseType>
    {
        private static readonly List<TypeInfo<BaseType>> _typeInfos = new List<TypeInfo<BaseType>>();
        private static readonly ConcurrentDictionary<string, TypeInfo<BaseType>> _typeNameIndex =
            new ConcurrentDictionary<string, TypeInfo<BaseType>>(StringComparer.OrdinalIgnoreCase);

        // Cached delegate for creating BaseType when no overrides are registered.
        // Lazily compiled on first New() call via Interlocked.CompareExchange.
        private static Func<BaseType> _defaultFactory;

        /// <summary>
        /// Gets all registered type mapping information within the current factory instance.
        /// </summary>
        public static IEnumerable<TypeInfo<BaseType>> AllTypeInfos
        {
            get
            {
                return _typeInfos;
            }
        }

#pragma warning disable S2743
        /// <summary>
        /// Gets a value indicating whether there are any type overrides registered in the factory.
        /// </summary>
        public static bool HasOverrides
#pragma warning restore S2743 // Static fields should not be used in generic types
        {
            get
            {
                return _typeInfos.Count > 0;
            }
        }

        /// <summary>
        /// Creates an instance of the most derived registered type, or the base type if no overrides exist.
        /// This is the fastest creation path: when no overrides are registered, it bypasses type lookup entirely
        /// and uses a cached compiled delegate (~5 ns vs ~180 ns for TryCreateInstance).
        /// </summary>
        /// <returns>An instance of the most derived type, or BaseType if no overrides are registered.</returns>
        /// <exception cref="OperationCanceledException">Thrown when BaseType is abstract and no overrides are registered.</exception>
        public static BaseType New()
        {
            if (_typeInfos.Count == 0)
            {
                // No overrides — create base type directly via cached delegate (no lookup needed)
                return GetOrCompileDefaultFactory()();
            }

            return TryCreateInstance();
        }

        /// <summary>
        /// Registers a new type in the factory and returns a TypeInfo instance for further configuration.
        /// </summary>
        /// <typeparam name="T">The type to be registered.</typeparam>
        /// <returns>TypeInfo instance for the registered type.</returns>
        public static TypeInfo<BaseType> RegisterType<T>() where T : BaseType
        {
            return RegisterType(typeof(T));
        }

        /// <summary>
        /// Registers a new type in the factory and returns a TypeInfo instance for further configuration.
        /// </summary>
        /// <param name="type">The type to be registered.</param>
        /// <returns>TypeInfo instance for the registered type.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TypeInfo<BaseType> RegisterType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = _typeInfos.FirstOrDefault(x => x.AllSubclasses.Contains(type));

            if (result == null)
            {
                result = new TypeInfo<BaseType>(type);
                _typeInfos.Add(result);
                RebuildIndex();
            }

            // Invalidate default factory — type resolution may have changed
            Volatile.Write(ref _defaultFactory, null);

            return result;
        }


        /// <summary>
        /// Overrides an already registered type with a new one and returns a TypeInfo instance for further configuration.
        /// </summary>
        /// <typeparam name="OldType">The currently registered type.</typeparam>
        /// <typeparam name="NewType">The currently registered type.</typeparam>
        /// <returns>TypeInfo instance for the overridden type.</returns>
        public static TypeInfo<BaseType> OverrideType<OldType, NewType>() where NewType : BaseType
        {
            return OverrideType(typeof(OldType), typeof(NewType));
        }



        /// <summary>
        /// Overrides an already registered type with a new one and returns a TypeInfo instance for further configuration.
        /// </summary>
        /// <param name="oldType">The currently registered type.</param>
        /// <param name="newType">The type to override oldType with.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static TypeInfo<BaseType> OverrideType(Type oldType, Type newType)
        {
            if (!typeof(BaseType).IsAssignableFrom(newType))
            {
                throw new ArgumentException($"Only a type assignable to {typeof(BaseType)} can be used to override {oldType}", nameof(newType));
            }

            var existTypeInfo = _typeInfos.FirstOrDefault(x => x.Type == oldType);
            var newTypeInfo = new TypeInfo<BaseType>(newType);
            if (existTypeInfo != null)
            {
                _typeInfos.Remove(existTypeInfo);
            }

            _typeInfos.Add(newTypeInfo);

            // Rebuild entire index — clears cached inheritance lookups that may now be stale
            RebuildIndex();

            // Invalidate default factory — type resolution may have changed
            Volatile.Write(ref _defaultFactory, null);

            return newTypeInfo;
        }

        /// <summary>
        /// Creates an instance of the base type using the type name.
        /// </summary>
        /// <returns>An instance of the base type.</returns>
        public static BaseType TryCreateInstance()
        {
            return TryCreateInstance(typeof(BaseType).Name);
        }

        /// <summary>
        /// Creates an instance of the base type using the type name and a default object.
        /// </summary>
        /// <param name="defaultObj"> The default object to use if the instance cannot be created.</param>
        /// <returns>An instance of the base type.</returns>
        public static BaseType TryCreateInstance(BaseType defaultObj)
        {
            return TryCreateInstance(typeof(BaseType).Name, defaultObj);
        }

        /// <summary>
        /// Creates an instance of the base type using the type name, a default object, and additional constructor arguments.
        /// </summary>
        /// <param name="defaultObj">The default object to use if the instance cannot be created.</param>
        /// <param name="args">Additional constructor arguments.</param>
        /// <returns>An instance of the base type.</returns>
        public static BaseType TryCreateInstance(BaseType defaultObj, params object[] args)
        {
            return TryCreateInstance(typeof(BaseType).Name, defaultObj, args);
        }

        /// <summary>
        /// Creates an instance of the specified generic type using the type name.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <returns>An instance of the specified generic type.</returns>
        public static T TryCreateInstance<T>() where T : BaseType
        {
            return (T)TryCreateInstance(typeof(T).Name);
        }

        /// <summary>
        /// Creates an instance of the specified generic type using the type name and a default object.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="defaultObj">The default object to use if the instance cannot be created.</param>
        /// <returns>An instance of the specified generic type.</returns>
        public static T TryCreateInstance<T>(T defaultObj) where T : BaseType
        {
            return (T)TryCreateInstance(typeof(T).Name, defaultObj);
        }

        /// <summary>
        /// Creates an instance of the specified generic type using the type name, a default object, and additional constructor arguments.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="defaultObj">The default object to use if the instance cannot be created.</param>
        /// <param name="args"> Additional constructor arguments.</param>
        /// <returns>An instance of the specified generic type.</returns>
        public static T TryCreateInstance<T>(T defaultObj, params object[] args) where T : BaseType
        {
            return (T)TryCreateInstance(typeof(T).Name, defaultObj, args);
        }

        /// <summary>
        /// Creates an instance of the base type using the specified type name.
        /// Optimized fast path that avoids params array allocation.
        /// </summary>
        /// <param name="typeName">The name of the type to create.</param>
        /// <returns>An instance of the base type.</returns>
        /// <exception cref="OperationCanceledException"></exception>
        public static BaseType TryCreateInstance(string typeName)
        {
            BaseType result;
            var typeInfo = FindTypeInfoByName(typeName);
            if (typeInfo != null)
            {
                var factory = typeInfo.GetOrCompileFactory();
                if (factory != null)
                {
                    result = factory();
                }
                else
                {
                    result = (BaseType)Activator.CreateInstance(typeInfo.Type);
                }
                typeInfo.SetupAction?.Invoke(result);
            }
            else
            {
                var baseType = typeof(BaseType);
                if (baseType.IsAbstract)
                {
                    throw new OperationCanceledException($"A type with {typeName} name is not registered in the AbstractFactory, you cannot create an instance of an abstract class {baseType.Name} because it does not have a complete implementation");
                }
                result = GetOrCompileDefaultFactory()();
            }

            return result;
        }

        /// <summary>
        /// Creates an instance of the base type using the specified type name and a default object.
        /// </summary>
        /// <param name="typeName">The name of the type to create.</param>
        /// <param name="defaultObj">The default object to use if the instance cannot be created.</param>
        /// <returns>An instance of the base type.</returns>
        public static BaseType TryCreateInstance(string typeName, BaseType defaultObj)
        {
            var typeInfo = FindTypeInfoByName(typeName);
            if (typeInfo != null)
            {
                return TryCreateInstance(typeName);
            }
            return defaultObj;
        }

        /// <summary>
        /// Creates an instance of the base type using the specified type name, a default object, and additional constructor arguments.
        /// </summary>
        /// <param name="typeName">The name of the type to create.</param>
        /// <param name="defaultObj">The default object to use if the instance cannot be created.</param>
        /// <param name="args">Additional constructor arguments.</param>
        /// <returns>An instance of the base type.</returns>
        public static BaseType TryCreateInstance(string typeName, BaseType defaultObj, params object[] args)
        {
            var typeInfo = FindTypeInfoByName(typeName);
            if (typeInfo != null)
            {
                if (args == null || args.Length == 0)
                {
                    return TryCreateInstance(typeName);
                }
                return TryCreateInstance(typeName, args);
            }
            return defaultObj;
        }

        /// <summary>
        /// Creates an instance of the base type using the specified type name and additional constructor arguments.
        /// </summary>
        /// <param name="typeName">The name of the type to create.</param>
        /// <param name="args">Additional constructor arguments.</param>
        /// <returns>An instance of the base type.</returns>
        /// <exception cref="OperationCanceledException"></exception>
        public static BaseType TryCreateInstance(string typeName, params object[] args)
        {
            // Fast path: no args → use parameterless overload (avoids Activator)
            if (args == null || args.Length == 0)
            {
                return TryCreateInstance(typeName);
            }

            BaseType result;
            var typeInfo = FindTypeInfoByName(typeName);
            if (typeInfo != null)
            {
                // With args — must use Activator (can't cache parameterized delegates)
                result = (BaseType)Activator.CreateInstance(typeInfo.Type, args);
                typeInfo.SetupAction?.Invoke(result);
            }
            else
            {
                var baseType = typeof(BaseType);
                if (baseType.IsAbstract)
                {
                    throw new OperationCanceledException($"A type with {typeName} name is not registered in the AbstractFactory, you cannot create an instance of an abstract class {baseType.Name} because it does not have a complete implementation");
                }
                result = (BaseType)Activator.CreateInstance(typeof(BaseType), args);
            }

            return result;
        }

        /// <summary>
        /// Finds the type information for the specified type name.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>The TypeInfo instance for the specified type name.</returns>
        public static TypeInfo<BaseType> FindTypeInfoByName(string typeName)
        {
            // O(1) direct match via concurrent dictionary
            if (_typeNameIndex.TryGetValue(typeName, out var result))
            {
                return result;
            }

            // Fallback: inheritance chain scan (e.g., lookup "ShoppingCart" when "LeoShoppingCart" is registered)
            result = _typeInfos.FirstOrDefault(x => x.IsAssignableTo(typeName));

            // Cache the result so subsequent lookups for the same name are O(1)
            if (result != null)
            {
                _typeNameIndex.TryAdd(typeName, result);
            }

            return result;
        }

        private static Func<BaseType> GetOrCompileDefaultFactory()
        {
            var factory = Volatile.Read(ref _defaultFactory);
            if (factory == null)
            {
                var baseType = typeof(BaseType);
                if (baseType.IsAbstract)
                {
                    throw new OperationCanceledException($"Cannot create an instance of abstract class {baseType.Name} because no concrete type is registered in the AbstractTypeFactory and it does not have a complete implementation");
                }
                factory = CompileFactory(baseType);
                Interlocked.CompareExchange(ref _defaultFactory, factory, null);
                factory = Volatile.Read(ref _defaultFactory);
            }
            return factory;
        }

        private static Func<BaseType> CompileFactory(Type type)
        {
            return Expression.Lambda<Func<BaseType>>(Expression.New(type)).Compile();
        }

        /// <summary>
        /// Rebuilds the type name index from scratch. Called on RegisterType/OverrideType
        /// to clear any cached inheritance lookups that may now be stale.
        /// </summary>
        private static void RebuildIndex()
        {
            _typeNameIndex.Clear();
            foreach (var typeInfo in _typeInfos)
            {
                _typeNameIndex[typeInfo.TypeName] = typeInfo;
            }
        }
    }

    /// <summary>
    /// Helper class that contains type mapping information.
    /// </summary>
    public class TypeInfo<BaseType>
    {
        // Backing field for Factory — supports Interlocked.CompareExchange for thread-safe lazy caching
        private Func<BaseType> _factory;

        public TypeInfo(Type type)
        {
            Services = new List<object>();
            Type = type;
            TypeName = type.Name;
        }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        public string TypeName { get; private set; }
        /// <summary>
        /// Gets the factory function used to create an instance of the type.
        /// When no factory is explicitly set via <see cref="WithFactory"/>, a compiled delegate
        /// is auto-cached on first use for types with a parameterless constructor.
        /// </summary>
        public Func<BaseType> Factory => Volatile.Read(ref _factory);
        /// <summary>
        /// Gets or sets the setup action to be performed on the created instance.
        /// </summary>
        public Action<BaseType> SetupAction { get; private set; }
        /// <summary>
        /// Gets or sets the type associated with the type mapping information.
        /// </summary>
        public Type Type { get; private set; }
        /// <summary>
        /// Gets or sets the mapped type that the associated type should be mapped to.
        /// </summary>
        public Type MappedType { get; set; }
        /// <summary>
        /// Gets or sets the mapped type that the associated type should be mapped to.
        /// </summary>
        public ICollection<object> Services { get; set; }

        /// <summary>
        /// Gets the service of the specified type from the collection of services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The service of the specified type, or default(T) if the service is not found.</returns>
        public T GetService<T>()
        {
            return Services.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Adds the specified service to the collection of services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <returns>The setup action.</returns>
        public TypeInfo<BaseType> WithService<T>(T service)
        {
            if (!Services.Contains(service))
            {
                Services.Add(service);
            }
            return this;
        }

        /// <summary>
        /// Maps the associated type to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The setup action.</returns>
        public TypeInfo<BaseType> MapToType<T>()
        {
            MappedType = typeof(T);
            return this;
        }

        /// <summary>
        /// Sets the factory function used to create an instance of the type.
        /// </summary>
        /// <param name="factory">The factory function.</param>
        /// <returns>The TypeInfo instance for chaining.</returns>
        public TypeInfo<BaseType> WithFactory(Func<BaseType> factory)
        {
            Volatile.Write(ref _factory, factory);
            return this;
        }

        /// <summary>
        /// Maps the associated type to the specified type.
        /// </summary>
        /// <param name="setupAction">The setup action.</param>
        /// <returns>The setup action.</returns>
        public TypeInfo<BaseType> WithSetupAction(Action<BaseType> setupAction)
        {
            SetupAction = setupAction;
            return this;
        }

        /// <summary>
        /// Sets the name of the type.
        /// </summary>
        /// <param name="name">Sets the name of the type.</param>
        /// <returns>Sets the name of the type.</returns>
        public TypeInfo<BaseType> WithTypeName(string name)
        {
            TypeName = name;
            return this;
        }

        /// <summary>
        /// Checks if the associated type is assignable to the specified type name.
        /// </summary>
        /// <param name="typeName">The name of the type to check.</param>
        /// <returns>true if the associated type is assignable to the specified type name; otherwise, false.</returns>
        public bool IsAssignableTo(string typeName)
        {
            return Type.GetTypeInheritanceChainTo(typeof(BaseType)).Concat([typeof(BaseType)]).Any(t => typeName.EqualsIgnoreCase(t.Name));
        }

        /// <summary>
        /// Gets all subclasses of the associated type.
        /// </summary>
        public IEnumerable<Type> AllSubclasses
        {
            get
            {
                return Type.GetTypeInheritanceChainTo(typeof(BaseType)).ToArray();
            }
        }

        /// <summary>
        /// Returns the cached factory delegate, auto-compiling one if the type has a parameterless constructor.
        /// Thread-safe via Interlocked.CompareExchange — benign race on first call.
        /// </summary>
        internal Func<BaseType> GetOrCompileFactory()
        {
            var factory = Volatile.Read(ref _factory);
            if (factory == null)
            {
                var ctor = Type.GetConstructor(Type.EmptyTypes);
                if (ctor != null)
                {
                    var compiled = Expression.Lambda<Func<BaseType>>(Expression.New(ctor)).Compile();
                    Interlocked.CompareExchange(ref _factory, compiled, null);
                    factory = Volatile.Read(ref _factory);
                }
            }
            return factory;
        }
    }
}
