using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    /// <summary>
    /// Rows saved by the same SaveChanges are written at the same instant, so announcing a type name once per row
    /// instead of once per unit of work invalidates the same cache key thousands of times and propagates every one of
    /// them to the scale-out backplane. The duplicates are dropped here, before the cache region propagates anything,
    /// leaving the propagation semantics untouched.
    /// </summary>
    public class LastChangesNotifier : ILastChangesNotifier
    {
        private readonly ConditionalWeakTable<DbContext, HashSet<string>> _announcedTypeNames = new();

        private readonly ILastChangesService _lastChangesService;

        public LastChangesNotifier(ILastChangesService lastChangesService)
        {
            _lastChangesService = lastChangesService;
        }

        public void OnEntitySaving(DbContext context, IEntity entity)
        {
            var typeNames = EntityTypeNames.Get(entity.GetType());

            if (context is null)
            {
                Announce(typeNames);
                return;
            }

            var announced = _announcedTypeNames.GetValue(context, CreateAnnouncedTypeNames);

            foreach (var entityTypeName in typeNames)
            {
                if (announced.Add(entityTypeName))
                {
                    _lastChangesService.Reset(entityTypeName);
                }
            }
        }

        private void Announce(string[] typeNames)
        {
            foreach (var entityTypeName in typeNames)
            {
                _lastChangesService.Reset(entityTypeName);
            }
        }

        /// <summary>
        /// The set lives for one SaveChanges: a later save has new information to announce. DbContext raises these
        /// events after every Inserting/Updating trigger of the batch has run, whatever its base class, and is not
        /// thread safe, so the set is only ever touched by the thread running SaveChanges.
        /// </summary>
        private static HashSet<string> CreateAnnouncedTypeNames(DbContext context)
        {
            var announced = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            context.SavedChanges += (_, _) => announced.Clear();
            context.SaveChangesFailed += (_, _) => announced.Clear();

            return announced;
        }
    }
}
