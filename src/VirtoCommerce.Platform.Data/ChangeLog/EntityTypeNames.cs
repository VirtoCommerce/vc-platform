using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    internal static class EntityTypeNames
    {
        private static readonly ConcurrentDictionary<Type, string[]> _namesByType = new();

        public static string[] Get(Type entityType)
        {
            return _namesByType.GetOrAdd(entityType, BuildTypeNames);
        }

        private static string[] BuildTypeNames(Type entityType)
        {
            var typeNames = new List<string>();

            while (entityType != null && entityType != typeof(Entity))
            {
                typeNames.Add(entityType.FullName);
                entityType = entityType.BaseType;
            }

            return typeNames.ToArray();
        }
    }
}
