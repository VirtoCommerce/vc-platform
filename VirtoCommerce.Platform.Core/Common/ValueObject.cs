using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VirtoCommerce.Platform.Core.Common
{
    public abstract class ValueObject : IValueObject, ICacheKey
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>> TypeProperties = new ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>>();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (ReferenceEquals(null, obj))
                return false;
            if (GetType() != obj.GetType())
                return false;
            var other = obj as ValueObject;
            return other != null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
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
            return string.Join("|", GetEqualityComponents().Select(x => x ?? "null").Select(x => x is ICacheKey cacheKey ? cacheKey.GetCacheKey() : x.ToString()));
        }

        protected virtual IEnumerable<object> GetEqualityComponents()
        {
            foreach (var property in GetProperties())
            {
                var value = property.GetValue(this);
                if (value == null)
                {
                    yield return "null";
                }
                else
                {
                    var valueType = value.GetType();
                    if (valueType.IsAssignableFromGenericList())
                    {
                        foreach (var child in ((IEnumerable)value))
                        {
                            yield return child ?? "null";
                        }
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
    }

    /// <summary>
    /// Previous generic ValueObject` type, leave it for backward compatibility
    /// TODO: Make Obsolete later
    /// </summary>
    /// <typeparam name="TValueObject"></typeparam>
    [Obsolete("Use non generic type instead")]
    public class ValueObject<TValueObject> : ValueObject, IEquatable<TValueObject>
    {
        public bool Equals(TValueObject other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !Equals(left, right);
        }

    }
}
