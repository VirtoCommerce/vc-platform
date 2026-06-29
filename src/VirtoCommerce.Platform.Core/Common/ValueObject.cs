using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Core.Common
{
    public abstract class ValueObject : IValueObject, ICacheKey, ICloneable
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>> TypeProperties = new ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>>();

        // Compiled property getters cached per PropertyInfo, so the reflective component path (ToString,
        // GetCacheKey, and the equality path of subtypes that override GetEqualityComponents) avoids
        // reflective PropertyInfo.GetValue (and the argument array it allocates) on every call.
        // Keyed by PropertyInfo (not Type) so it stays correct even if a subtype overrides GetProperties().
        private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> PropertyGetters = new ConcurrentDictionary<PropertyInfo, Func<object, object>>();

        // Per-type plan of compiled, mostly allocation-free accessors used by the fast Equals/GetHashCode
        // path. Only built for subtypes that do NOT override GetEqualityComponents (their components are
        // the public properties); subtypes that override it keep the legacy component-based path.
        private static readonly ConcurrentDictionary<Type, EqualityPlan> EqualityPlans = new ConcurrentDictionary<Type, EqualityPlan>();

        private delegate void HashContributor(object instance, ref HashCode hash);

        private delegate bool EqualityContributor(object left, object right);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (ReferenceEquals(null, obj))
                return false;
            if (GetType() != obj.GetType())
                return false;

            var plan = GetEqualityPlan(GetType());
            if (plan != null)
            {
                foreach (var accessor in plan.Accessors)
                {
                    if (!accessor.AreEqual(this, obj))
                    {
                        return false;
                    }
                }

                return true;
            }

            var other = obj as ValueObject;
            return other != null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            var plan = GetEqualityPlan(GetType());
            if (plan != null)
            {
                var hash = new HashCode();
                foreach (var accessor in plan.Accessors)
                {
                    accessor.ContributeHash(this, ref hash);
                }

                return hash.ToHashCode();
            }

            unchecked
            {
                return GetEqualityComponents().Aggregate(17, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
            }
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{{{string.Join(", ", GetProperties().Select(f => $"{f.Name}: {f.GetValue(this)}"))}}}";
        }

        public virtual string GetCacheKey()
        {
            var keyValues = GetEqualityComponents()
                .Select(x => x is string ? $"'{x}'" : x)
                .Select(x => x is ICacheKey cacheKey ? cacheKey.GetCacheKey() : x?.ToString());

            return string.Join("|", keyValues);
        }

        protected virtual IEnumerable<object> GetEqualityComponents()
        {
            foreach (var property in GetProperties())
            {
                var value = GetPropertyAccessor(property)(this);
                if (value == null)
                {
                    yield return null;
                }
                else
                {
                    var valueType = value.GetType();
                    if (valueType.IsAssignableFromGenericList())
                    {
                        yield return '[';
                        foreach (var child in ((IEnumerable)value))
                        {
                            yield return child;
                        }
                        yield return ']';
                    }
                    else
                    {
                        yield return value;
                    }
                }
            }
        }

        protected virtual IEnumerable<PropertyInfo> GetProperties()
        {
            return TypeProperties.GetOrAdd(
                GetType(),
                t => t
                    .GetTypeInfo()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .OrderBy(p => p.Name)
                    .ToList());
        }

        private static Func<object, object> GetPropertyAccessor(PropertyInfo property)
        {
            return PropertyGetters.GetOrAdd(property, BuildPropertyAccessor);
        }

        private static Func<object, object> BuildPropertyAccessor(PropertyInfo property)
        {
            // Compiles: instance => (object)((TDeclaringType)instance).Property
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var typedInstance = Expression.Convert(instanceParameter, property.DeclaringType);
            var propertyAccess = Expression.Property(typedInstance, property);
            var boxedResult = Expression.Convert(propertyAccess, typeof(object));

            return Expression.Lambda<Func<object, object>>(boxedResult, instanceParameter).Compile();
        }

        // Returns the fast-path plan for a type, or null when the type overrides GetEqualityComponents
        // (its components are not the public properties) and must use the legacy component-based path.
        private static EqualityPlan GetEqualityPlan(Type type)
        {
            return EqualityPlans.GetOrAdd(type, BuildEqualityPlan);
        }

        private static EqualityPlan BuildEqualityPlan(Type type)
        {
            // The fast path enumerates the public properties directly, so it is only valid when the type
            // uses the default component model — i.e. overrides NEITHER GetEqualityComponents (custom
            // components) NOR GetProperties (custom property set/order). Either override means equality
            // must flow through the virtual component path, so fall back to the legacy implementation.
            if (IsOverridden(type, nameof(GetEqualityComponents)) || IsOverridden(type, nameof(GetProperties)))
            {
                return null;
            }

            var accessors = type
                .GetTypeInfo()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(p => p.Name)
                .Select(ComponentAccessor.Create)
                .ToArray();

            return new EqualityPlan(accessors);
        }

        private static bool IsOverridden(Type type, string methodName)
        {
            var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

            return method?.DeclaringType != typeof(ValueObject);
        }

        // Replicates the per-property contribution of the legacy reflective GetEqualityComponents for a
        // reference-typed (or list) value, without boxing reference values: scalar/null compared directly,
        // and only list-bearing values pay the marked component-stream expansion.
        private static void ContributeReferenceHash(ref HashCode hash, object value)
        {
            if (value != null && value.GetType().IsAssignableFromGenericList())
            {
                hash.Add('[');
                foreach (var child in (IEnumerable)value)
                {
                    hash.Add(child);
                }

                hash.Add(']');
            }
            else
            {
                hash.Add(value);
            }
        }

        private static bool ReferenceComponentsEqual(object left, object right)
        {
            // The list predicate is per-value here (and in ContributeReferenceHash): both must mirror the
            // legacy "expand a list value to '[' + children + ']'" contract. Keep the two in lock-step —
            // if one side is a list and the other is a scalar/null, the expanded streams differ and the
            // values are correctly unequal.
            var leftIsList = left != null && left.GetType().IsAssignableFromGenericList();
            var rightIsList = right != null && right.GetType().IsAssignableFromGenericList();

            if (!leftIsList && !rightIsList)
            {
                return Equals(left, right);
            }

            // Mirror the legacy SequenceEqual over the '['/']'-marked component stream exactly, so a
            // value that is a list on one side and a scalar/null on the other stays unequal.
            return ExpandComponent(left).SequenceEqual(ExpandComponent(right));
        }

        private static IEnumerable<object> ExpandComponent(object value)
        {
            if (value == null)
            {
                yield return null;
            }
            else if (value.GetType().IsAssignableFromGenericList())
            {
                yield return '[';
                foreach (var child in (IEnumerable)value)
                {
                    yield return child;
                }

                yield return ']';
            }
            else
            {
                yield return value;
            }
        }

        #region ICloneable members
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        #endregion

        private sealed class EqualityPlan
        {
            public EqualityPlan(ComponentAccessor[] accessors)
            {
                Accessors = accessors;
            }

            public ComponentAccessor[] Accessors { get; }
        }

        // Compiled accessor for a single property. Value-typed properties use fully typed delegates
        // (no boxing); reference-typed properties get the value as object (free for reference types) and
        // route through the runtime list/scalar logic that mirrors the legacy component path.
        private sealed class ComponentAccessor
        {
            private readonly Func<object, object> _referenceGetter;
            private readonly HashContributor _valueHasher;
            private readonly EqualityContributor _valueEqualizer;

            private ComponentAccessor(Func<object, object> referenceGetter, HashContributor valueHasher, EqualityContributor valueEqualizer)
            {
                _referenceGetter = referenceGetter;
                _valueHasher = valueHasher;
                _valueEqualizer = valueEqualizer;
            }

            public static ComponentAccessor Create(PropertyInfo property)
            {
                // Value types are never list-valued at runtime (the exotic IList-implementing struct is
                // routed through the reference path), so they take the fully typed, allocation-free path.
                if (property.PropertyType.IsValueType && !property.PropertyType.IsAssignableFromGenericList())
                {
                    return new ComponentAccessor(null, BuildValueHasher(property), BuildValueEqualizer(property));
                }

                return new ComponentAccessor(GetPropertyAccessor(property), null, null);
            }

            public void ContributeHash(object instance, ref HashCode hash)
            {
                if (_valueHasher != null)
                {
                    _valueHasher(instance, ref hash);
                }
                else
                {
                    ContributeReferenceHash(ref hash, _referenceGetter(instance));
                }
            }

            public bool AreEqual(object left, object right)
            {
                return _valueEqualizer != null
                    ? _valueEqualizer(left, right)
                    : ReferenceComponentsEqual(_referenceGetter(left), _referenceGetter(right));
            }

            private static HashContributor BuildValueHasher(PropertyInfo property)
            {
                // Compiles: (instance, ref hash) => hash.Add<TProperty>(((TDeclaringType)instance).Property)
                var instanceParameter = Expression.Parameter(typeof(object), "instance");
                var hashParameter = Expression.Parameter(typeof(HashCode).MakeByRefType(), "hash");
                var typedInstance = Expression.Convert(instanceParameter, property.DeclaringType);
                var propertyAccess = Expression.Property(typedInstance, property);

                var addMethod = typeof(HashCode)
                    .GetMethods()
                    .Single(m => m.Name == nameof(HashCode.Add) && m.IsGenericMethodDefinition && m.GetParameters().Length == 1)
                    .MakeGenericMethod(property.PropertyType);

                var call = Expression.Call(hashParameter, addMethod, propertyAccess);

                return Expression.Lambda<HashContributor>(call, instanceParameter, hashParameter).Compile();
            }

            private static EqualityContributor BuildValueEqualizer(PropertyInfo property)
            {
                // Compiles: (left, right) => EqualityComparer<TProperty>.Default.Equals(
                //               ((TDeclaringType)left).Property, ((TDeclaringType)right).Property)
                var leftParameter = Expression.Parameter(typeof(object), "left");
                var rightParameter = Expression.Parameter(typeof(object), "right");
                var leftProperty = Expression.Property(Expression.Convert(leftParameter, property.DeclaringType), property);
                var rightProperty = Expression.Property(Expression.Convert(rightParameter, property.DeclaringType), property);

                var comparerType = typeof(EqualityComparer<>).MakeGenericType(property.PropertyType);
                var defaultComparer = Expression.Property(null, comparerType.GetProperty(nameof(EqualityComparer<object>.Default)));
                var equalsMethod = comparerType.GetMethod(nameof(EqualityComparer<object>.Equals), [property.PropertyType, property.PropertyType]);

                var call = Expression.Call(defaultComparer, equalsMethod, leftProperty, rightProperty);

                return Expression.Lambda<EqualityContributor>(call, leftParameter, rightParameter).Compile();
            }
        }
    }
}
