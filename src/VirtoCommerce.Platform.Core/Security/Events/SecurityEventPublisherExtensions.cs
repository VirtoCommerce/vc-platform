using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public static class SecurityEventsPublisherExtensions
    {
        static Dictionary<string, Func<List<GenericChangedEntry<ApplicationUser>>, IEvent>> factoriesMap = new Dictionary<string, Func<List<GenericChangedEntry<ApplicationUser>>, IEvent>>()
        {
            { PlatformConstants.Security.Changes.UserUpdated, f => new UserChangedEvent(f) },
            { PlatformConstants.Security.Changes.UserPasswordChanged, f => new UserPasswordChangedEvent(f.FirstOrDefault()?.NewEntry) }
        };

        public static Task PublishSecurityEventsAsync(this IEventPublisher eventPublisher, List<GenericChangedEntry<ApplicationUser>> changedEntries)
        {
            var events = MakeEvents(changedEntries);
            var tasks = events.Select(x => eventPublisher.Publish(x)); 
            return Task.WhenAll(tasks);
        }

        private static IEnumerable<IEvent> MakeEvents(List<GenericChangedEntry<ApplicationUser>> changedEntries)
        {
            var changes = changedEntries.SelectMany(e => e.NewEntry.DetectUserChanges(e.OldEntry));
            foreach (var change in changes)
            {
                if (factoriesMap.TryGetValue(change.Key, out var factory))
                {
                    yield return factory.Invoke(changedEntries);
                }
            }
        }
    }
}
