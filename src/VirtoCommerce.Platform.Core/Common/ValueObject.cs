using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Core.Common;

public abstract class ValueObject : IValueObject, ICacheKey, ICloneable
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>> _typeProperties = new();

    // Compiled property getters cached per PropertyInfo, so the reflective component path (ToString,
    // GetCacheKey, and the equality path of subtypes that override GetEqualityComponents) avoids
    // reflective PropertyInfo.GetValue (and the argument array it allocates) on every call.
    // Keyed by PropertyInfo (not Type) so it stays correct even if a subtype overrides GetProperties().
    private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> _propertyGetters = new();

    // Per-type set of compiled, mostly allocation-free component accessors used by the fast
    // Equals/GetHashCode path. Cached as null for subtypes that override GetEqualityComponents (custom
    // components) or GetProperties (custom property set/order) — those keep the legacy component path.
    private static readonly ConcurrentDictionary<Type, ComponentAccessor[]> _componentAccessors = new();

    private delegate void ValueHasher(object instance, ref HashCode hash);

    private delegate bool ValueEqualityCheck(object left, object right);

    [SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions",
        Justification = "This is the equality hot path. A LINQ 'accessors.All(a => a.AreEqual(this, obj))' allocates a closure capturing 'this'/'obj' on every Equals call — the exact per-call allocation this compiled fast path exists to avoid.")]
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var accessors = GetComponentAccessors(GetType());
        if (accessors is not null)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var accessor in accessors)
            {
                if (!accessor.AreEqual(this, obj))
                {
                    return false;
                }
            }

            return true;
        }

        var other = obj as ValueObject;

        return other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        var accessors = GetComponentAccessors(GetType());
        if (accessors is not null)
        {
            var hash = new HashCode();
            foreach (var accessor in accessors)
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
        var properties = GetProperties().Select(x => $"{x.Name}: {GetPropertyGetter(x)(this)}");

        return $"{{{string.Join(", ", properties)}}}";
    }

    public virtual string GetCacheKey()
    {
        var keyValues = GetEqualityComponents().Select(obj => obj switch
        {
            string text => $"'{text}'",
            ICacheKey cacheKey => cacheKey.GetCacheKey(),
            _ => obj?.ToString(),
        });

        return string.Join("|", keyValues);
    }

    protected virtual IEnumerable<object> GetEqualityComponents()
    {
        // Each property contributes the same marked component stream as the fast path: a list value
        // expands to '[' + children + ']', everything else passes through as-is (see ExpandComponent).
        foreach (var property in GetProperties())
        {
            foreach (var component in ExpandComponent(GetPropertyGetter(property)(this)))
            {
                yield return component;
            }
        }
    }

    protected virtual IEnumerable<PropertyInfo> GetProperties()
    {
        return _typeProperties.GetOrAdd(GetType(), type => GetProperties(type).ToList());
    }

    private static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        return type
            .GetTypeInfo()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .OrderBy(p => p.Name);
    }

    // PropertyInfo.DeclaringType is typed as nullable, but for properties enumerated from a type it is
    // always the declaring class. The compiled accessors cast the instance to this type, so a null here
    // would otherwise surface as a NullReferenceException deep inside Expression.Convert.
    private static Type GetDeclaringType(PropertyInfo property)
    {
        return property.DeclaringType
               ?? throw new InvalidOperationException($"Property '{property.Name}' has no declaring type.");
    }

    private static Func<object, object> GetPropertyGetter(PropertyInfo property)
    {
        return _propertyGetters.GetOrAdd(property, BuildPropertyGetter);
    }

    private static Func<object, object> BuildPropertyGetter(PropertyInfo property)
    {
        // Compiles: instance => (object)((TDeclaringType)instance).Property
        var instanceParameter = Expression.Parameter(typeof(object), "instance");
        var typedInstance = Expression.Convert(instanceParameter, GetDeclaringType(property));
        var propertyAccess = Expression.Property(typedInstance, property);
        var boxedResult = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<object, object>>(boxedResult, instanceParameter).Compile();
    }

    // Returns the fast-path component accessors for a type, or null when the type overrides
    // GetEqualityComponents or GetProperties and must use the legacy component-based path.
    private static ComponentAccessor[] GetComponentAccessors(Type type)
    {
        return _componentAccessors.GetOrAdd(type, BuildComponentAccessors);
    }

    [SuppressMessage("Major Code Smell", "S1168:Empty arrays and collections should be returned instead of null",
        Justification = "null is the sentinel for 'this type uses the legacy component path'; an empty array would mean 'fast path with zero components', which is a different behaviour.")]
    private static ComponentAccessor[] BuildComponentAccessors(Type type)
    {
        // The fast path enumerates the public properties directly, so it is only valid when the type
        // uses the default component model — i.e. overrides NEITHER GetEqualityComponents (custom
        // components) NOR GetProperties (custom property set/order). Either override means equality
        // must flow through the virtual component path, so fall back to the legacy implementation.
        if (IsOverridden(type, nameof(GetEqualityComponents)) || IsOverridden(type, nameof(GetProperties)))
        {
            return null;
        }

        return GetProperties(type).Select(ComponentAccessor.Create).ToArray();
    }

    [SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
        Justification = "Reflection only detects whether a subtype overrides the protected virtual GetEqualityComponents/GetProperties (the fast-path gate); it neither invokes them nor widens their accessibility.")]
    private static bool IsOverridden(Type type, string methodName)
    {
        var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

        return method?.DeclaringType != typeof(ValueObject);
    }

    // Hashes one reference-typed (or list) component, mirroring the marked stream that ExpandComponent
    // yields but WITHOUT its enumerator allocation — this runs on the GetHashCode hot path, so the
    // expansion is inlined here on purpose. Keep the '['/']' list contract in lock-step with
    // ExpandComponent and ReferenceComponentsEqual: a list expands to '[' + children + ']', and any
    // scalar/null is added directly.
    private static void HashReferenceComponent(object value, ref HashCode hash)
    {
        if (IsListComponent(value))
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
        // The list predicate mirrors HashReferenceComponent and ExpandComponent: all three must agree on
        // the "expand a list value to '[' + children + ']'" contract. Keep them in lock-step — if one
        // side is a list and the other is a scalar/null, the expanded streams differ and the values are
        // correctly unequal.
        if (!IsListComponent(left) && !IsListComponent(right))
        {
            return Equals(left, right);
        }

        // Mirror the legacy SequenceEqual over the '['/']'-marked component stream exactly, so a
        // value that is a list on one side and a scalar/null on the other stays unequal.
        return ExpandComponent(left).SequenceEqual(ExpandComponent(right));
    }

    // True when a component value is a generic list (IList<>) and must expand to the '['/']'-marked
    // stream. Centralized so HashReferenceComponent, ReferenceComponentsEqual and ExpandComponent
    // cannot drift on what counts as a list component.
    private static bool IsListComponent(object value)
    {
        return value is not null && value.GetType().IsAssignableFromGenericList();
    }

    private static IEnumerable<object> ExpandComponent(object value)
    {
        if (IsListComponent(value))
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

    public virtual object Clone()
    {
        return MemberwiseClone();
    }

    // Compiled accessor for a single property. Value-typed properties use fully typed delegates
    // (no boxing); reference-typed properties get the value as object (free for reference types) and
    // route through the runtime list/scalar logic that mirrors the legacy component path.
    private sealed class ComponentAccessor
    {
        private readonly Func<object, object> _referenceGetter;
        private readonly ValueHasher _valueHasher;
        private readonly ValueEqualityCheck _valueEqualityCheck;

        private ComponentAccessor(Func<object, object> referenceGetter, ValueHasher valueHasher = null, ValueEqualityCheck valueEqualityCheck = null)
        {
            _referenceGetter = referenceGetter;
            _valueHasher = valueHasher;
            _valueEqualityCheck = valueEqualityCheck;
        }

        private ComponentAccessor(ValueHasher valueHasher, ValueEqualityCheck valueEqualityCheck)
            : this(null, valueHasher, valueEqualityCheck)
        {
        }

        public static ComponentAccessor Create(PropertyInfo property)
        {
            // Value types are never list-valued at runtime (the exotic IList-implementing struct is
            // routed through the reference path), so they take the fully typed, allocation-free path.
            return property.PropertyType.IsValueType && !property.PropertyType.IsAssignableFromGenericList()
                ? new ComponentAccessor(BuildValueHasher(property), BuildValueEqualityCheck(property))
                : new ComponentAccessor(GetPropertyGetter(property));
        }

        public void ContributeHash(object instance, ref HashCode hash)
        {
            if (_valueHasher is not null)
            {
                _valueHasher(instance, ref hash);
            }
            else
            {
                HashReferenceComponent(_referenceGetter(instance), ref hash);
            }
        }

        public bool AreEqual(object left, object right)
        {
            return _valueEqualityCheck?.Invoke(left, right) ?? ReferenceComponentsEqual(_referenceGetter(left), _referenceGetter(right));
        }

        private static ValueHasher BuildValueHasher(PropertyInfo property)
        {
            // Compiles: (instance, ref hash) => hash.Add<TProperty>(((TDeclaringType)instance).Property)
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var hashParameter = Expression.Parameter(typeof(HashCode).MakeByRefType(), "hash");
            var typedInstance = Expression.Convert(instanceParameter, GetDeclaringType(property));
            var propertyAccess = Expression.Property(typedInstance, property);

            var addMethod = typeof(HashCode).GetMethods()
                .Single(m => m.Name == nameof(HashCode.Add) && m.IsGenericMethodDefinition && m.GetParameters().Length == 1)
                .MakeGenericMethod(property.PropertyType);

            var call = Expression.Call(hashParameter, addMethod, propertyAccess);

            return Expression.Lambda<ValueHasher>(call, instanceParameter, hashParameter).Compile();
        }

        private static ValueEqualityCheck BuildValueEqualityCheck(PropertyInfo property)
        {
            // Compiles: (left, right) => EqualityComparer<TProperty>.Default.Equals(
            //               ((TDeclaringType)left).Property, ((TDeclaringType)right).Property)
            var leftParameter = Expression.Parameter(typeof(object), "left");
            var rightParameter = Expression.Parameter(typeof(object), "right");
            var leftProperty = Expression.Property(Expression.Convert(leftParameter, GetDeclaringType(property)), property);
            var rightProperty = Expression.Property(Expression.Convert(rightParameter, GetDeclaringType(property)), property);

            var comparerType = typeof(EqualityComparer<>).MakeGenericType(property.PropertyType);
            var defaultProperty = comparerType.GetProperty(nameof(EqualityComparer<>.Default))
                ?? throw new InvalidOperationException($"'{comparerType}' has no Default property.");
            var equalsMethod = comparerType.GetMethod(nameof(EqualityComparer<>.Equals), [property.PropertyType, property.PropertyType])
                ?? throw new InvalidOperationException($"'{comparerType}' has no Equals(TProperty, TProperty) method.");

            var defaultComparer = Expression.Property(null, defaultProperty);
            var call = Expression.Call(defaultComparer, equalsMethod, leftProperty, rightProperty);

            return Expression.Lambda<ValueEqualityCheck>(call, leftParameter, rightParameter).Compile();
        }
    }
}
