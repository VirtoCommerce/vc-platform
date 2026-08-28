using System;
using System.Collections.Generic;
using System.Linq;
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

            Announce(typeNames.Where(announced.Add));
        }

        private bool IsIgnored(string[] typeNames)
        {
            return _ignoredEntityTypes.Count > 0 && typeNames.Any(_ignoredEntityTypes.Contains);
        }

        private void Announce(IEnumerable<string> typeNames)
        {
            foreach (var entityTypeName in typeNames)
            {
                _lastChangesService.Reset(entityTypeName);
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
