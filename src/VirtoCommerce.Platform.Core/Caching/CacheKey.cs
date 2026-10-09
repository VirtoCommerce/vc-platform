using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Core.Caching
{
    public static class CacheKey
    {
        // Only type-owned prefixes are registered, never individual keys or request data.
        private static readonly ConcurrentDictionary<string, CacheName> _cacheNames = new(StringComparer.OrdinalIgnoreCase);
        private static readonly ConcurrentDictionary<Type, Owner> _owners = new();

        public static string With(params string[] keys)
        {
            return string.Join("-", keys);
        }

        public static string With(params ReadOnlySpan<string> keys)
        {
            return string.Join("-", keys);
        }

        public static string With(Type ownerType, params string[] keys)
        {
            return $"{GetOwnerPrefix(ownerType)}:{string.Join("-", keys)}";
        }

        public static string With(Type ownerType, params ReadOnlySpan<string> keys)
        {
            return $"{GetOwnerPrefix(ownerType)}:{string.Join("-", keys)}";
        }

        /// <summary>
        /// Associates an owner's cache keys with a logical model type for telemetry.
        /// Does not change cache keys, lookup behavior, or invalidation.
        /// </summary>
        public static void RegisterCacheName(Type ownerType, Type modelType)
        {
            GetOwner(ownerType).CacheName.RegisterModel(modelType);
        }

        /// <summary>
        /// Returns the registered logical group for a type-owned key, or null for an unclassified key.
        /// Arbitrary string prefixes and the variable part of a key are never used as metric names.
        /// </summary>
        public static string GetCacheName(object key)
        {
            if (key is string stringKey)
            {
                var separator = stringKey.IndexOf(':');
                if (separator > 0 && _cacheNames.GetAlternateLookup<ReadOnlySpan<char>>().TryGetValue(stringKey.AsSpan(0, separator), out var name))
                {
                    return name.Name;
                }
            }

            return null;
        }

        private static string GetOwnerPrefix(Type ownerType)
        {
            return GetOwner(ownerType).Prefix;
        }

        private static Owner GetOwner(Type ownerType)
        {
            return _owners.GetOrAdd(ownerType, static type =>
            {
                var prefix = type.GetCacheKey();
                return new Owner(prefix, _cacheNames.GetOrAdd(prefix, static name => new CacheName(name)));
            });
        }

        private sealed class Owner(string prefix, CacheName cacheName)
        {
            // Telemetry matching is case-insensitive; cache key construction preserves each Type's spelling.
            public string Prefix { get; } = prefix;
            public CacheName CacheName { get; } = cacheName;
        }

        private sealed class CacheName(string prefix)
        {
            public string Prefix { get; } = prefix;
            public string Name => _name;
            private readonly object _registrationLock = new();
            private volatile string _name = prefix;
            private volatile Type _modelType;
            private volatile bool _conflicted;

            public void RegisterModel(Type modelType)
            {
                ArgumentNullException.ThrowIfNull(modelType);
                if (_conflicted || _modelType == modelType)
                {
                    return;
                }

                // Only first registration and conflicts write. Transient service construction is read-only.
                lock (_registrationLock)
                {
                    if (_conflicted || _modelType == modelType)
                    {
                        return;
                    }

                    if (_modelType is null)
                    {
                        _name = modelType.GetCacheKey();
                        _modelType = modelType;
                    }
                    else
                    {
                        // Existing keys contain short names, so different owners cannot be recovered from
                        // the key. Keep their shared owner prefix permanently after an ambiguous mapping.
                        _name = Prefix;
                        _conflicted = true;
                    }
                }
            }
        }

        public static object Normalize(object key)
        {
            return key is string stringKey
                ? Normalize(stringKey)
                : key;
        }

        [SuppressMessage("Major Code Smell", "S3267:Loops should be simplified using the \"Where\" LINQ method",
            Justification = "Perf-critical cache-key normalization: a LINQ Any/Where over EnumerateRunes() would box the StringRuneEnumerator and allocate a delegate on every call — the explicit alloc-free scan is the whole point of this hot path.")]
        public static string Normalize(string key)
        {
            foreach (var rune in key.EnumerateRunes())
            {
                if (Rune.ToLowerInvariant(rune) != rune)
                {
                    return key.ToLowerInvariant();
                }
            }

            return key;
        }
    }
}
