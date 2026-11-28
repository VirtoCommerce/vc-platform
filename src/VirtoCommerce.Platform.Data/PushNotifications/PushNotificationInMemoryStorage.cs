using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    /// <summary>
    /// Keeps notifications in process memory, by the same <see cref="PushNotificationStorageRules"/> as <see cref="RedisPushNotificationStorage"/>.
    /// Used when no Redis is configured; each instance keeps its own notifications, and they are lost on restart.
    /// </summary>
    public class PushNotificationInMemoryStorage : IPushNotificationStorage
    {
        private readonly Lock _lock = new();

        // In save order: a save of a known id moves the notification to the end, so the end holds the most recently saved.
        private readonly List<StoredNotification> _storage = [];
        private readonly PushNotificationOptions _options;
        private readonly TimeProvider _timeProvider;

        public PushNotificationInMemoryStorage(IOptions<PushNotificationOptions> options, TimeProvider timeProvider = null)
        {
            _options = options?.Value ?? new PushNotificationOptions();
            _timeProvider = timeProvider ?? TimeProvider.System;
        }

        public virtual Task SavePushNotificationAsync(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            lock (_lock)
            {
                _storage.RemoveAll(x => x.Notification.Id == notification.Id);
                _storage.Add(new StoredNotification(notification, _timeProvider.GetUtcNow().UtcDateTime));

                RemoveExpired();
                RemoveOverLimit(notification.Creator);
            }

            return Task.CompletedTask;
        }

        public virtual Task<PushNotificationSearchResult> SearchPushNotificationsAsync(string userId, PushNotificationSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            List<PushNotification> userNotifications;
            lock (_lock)
            {
                userNotifications = GetStored(userId).ToList();
            }

            return Task.FromResult(PushNotificationSearch.Search(userNotifications, criteria));
        }

        public virtual Task<IList<PushNotification>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
        {
            IList<PushNotification> unread;
            lock (_lock)
            {
                unread = GetStored(userId).Where(x => x.IsNew).ToList();

                foreach (var notification in unread)
                {
                    notification.IsNew = false;
                }
            }

            return Task.FromResult(unread);
        }

        /// <summary>
        /// Removes the notifications last saved more than <see cref="PushNotificationOptions.RetentionPeriod"/> ago. Called under the storage lock.
        /// </summary>
        protected virtual void RemoveExpired()
        {
            var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
            if (expiredBefore != null)
            {
                _storage.RemoveAll(x => x.SavedAt < expiredBefore);
            }
        }

        /// <summary>
        /// Keeps only the <see cref="PushNotificationOptions.MaxNotificationsPerUser"/> most recently saved notifications of the creator.
        /// Called under the storage lock.
        /// </summary>
        protected virtual void RemoveOverLimit(string creator)
        {
            var maxPerUser = PushNotificationStorageRules.GetMaxPerUser(_options);
            if (maxPerUser == 0)
            {
                return;
            }

            var creatorNotifications = _storage.Where(x => x.Notification.Creator == creator).ToList();
            var overLimit = creatorNotifications.Take(creatorNotifications.Count - maxPerUser).ToHashSet();

            if (overLimit.Count > 0)
            {
                _storage.RemoveAll(overLimit.Contains);
            }
        }

        // The creator's notifications saved within the retention period. Called under the storage lock.
        private IEnumerable<PushNotification> GetStored(string creator)
        {
            var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);

            return _storage
                .Where(x => x.Notification.Creator == creator && (expiredBefore == null || x.SavedAt >= expiredBefore))
                .Select(x => x.Notification);
        }

        private sealed record StoredNotification(PushNotification Notification, DateTime SavedAt);
    }
}
