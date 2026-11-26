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
        private readonly List<PushNotification> _storage = [];
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
                var index = _storage.FindIndex(x => x.Id == notification.Id);
                if (index >= 0)
                {
                    _storage[index] = notification;
                }
                else
                {
                    _storage.Add(notification);
                }

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
                var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
                userNotifications = _storage.Where(x => x.Creator == userId && (expiredBefore == null || x.Created >= expiredBefore)).ToList();
            }

            return Task.FromResult(PushNotificationSearch.Search(userNotifications, criteria));
        }

        public virtual Task<IList<PushNotification>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
        {
            IList<PushNotification> unread;
            lock (_lock)
            {
                var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
                unread = _storage.Where(x => x.Creator == userId && x.IsNew && (expiredBefore == null || x.Created >= expiredBefore)).ToList();

                foreach (var notification in unread)
                {
                    notification.IsNew = false;
                }
            }

            return Task.FromResult(unread);
        }

        /// <summary>
        /// Removes the notifications older than <see cref="PushNotificationOptions.RetentionPeriod"/>. Called under the storage lock.
        /// </summary>
        protected virtual void RemoveExpired()
        {
            var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
            if (expiredBefore != null)
            {
                _storage.RemoveAll(x => x.Created < expiredBefore);
            }
        }

        /// <summary>
        /// Keeps only the newest <see cref="PushNotificationOptions.MaxNotificationsPerUser"/> notifications of the creator. Called under the storage lock.
        /// </summary>
        protected virtual void RemoveOverLimit(string creator)
        {
            var maxPerUser = PushNotificationStorageRules.GetMaxPerUser(_options);
            if (maxPerUser == 0)
            {
                return;
            }

            var overLimit = _storage
                .Where(x => x.Creator == creator)
                .OrderByDescending(x => x.Created)
                .Skip(maxPerUser)
                .ToHashSet();

            if (overLimit.Count > 0)
            {
                _storage.RemoveAll(overLimit.Contains);
            }
        }
    }
}
