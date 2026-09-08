using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    public class LastChangesNotifier : ILastChangesNotifier
    {
        private readonly ConditionalWeakTable<DbContext, HashSet<string>> _announcedTypeNames = new();

        private readonly ILastChangesService _lastChangesService;
        private readonly HashSet<string> _ignoredEntityTypes;

        public LastChangesNotifier(ILastChangesService lastChangesService, IOptions<LastChangesOptions> options)
        {
            _lastChangesService = lastChangesService;
            _ignoredEntityTypes = new HashSet<string>(options.Value.IgnoredEntityTypes ?? [], StringComparer.OrdinalIgnoreCase);
        }

        public void OnEntitySaving(DbContext context, IEntity entity)
        {
            var typeNames = EntityTypeNames.Get(entity.GetType());

            if (IsIgnored(typeNames))
            {
                return;
            }

            if (context is null)
            {
                Announce(typeNames);
                return;
            }

            var announced = _announcedTypeNames.GetValue(context, CreateAnnouncedTypeNames);

            Announce(typeNames, announced);
        }

        private bool IsIgnored(string[] typeNames)
        {
            if (_ignoredEntityTypes.Count == 0)
            {
                return false;
            }

            foreach (var typeName in typeNames)
            {
                if (_ignoredEntityTypes.Contains(typeName))
                {
                    return true;
                }
            }

            return false;
        }

        private void Announce(string[] typeNames, HashSet<string> announced = null)
        {
            foreach (var entityTypeName in typeNames)
            {
                if (announced is null || announced.Add(entityTypeName))
                {
                    _lastChangesService.Reset(entityTypeName);
                }
            }
        }

        private static HashSet<string> CreateAnnouncedTypeNames(DbContext context)
        {
            var announced = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            context.SavedChanges += (_, _) => announced.Clear();
            context.SaveChangesFailed += (_, _) => announced.Clear();

            return announced;
        }
    }
}
