using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    /// <summary>
    /// Keeps notifications in process memory, by the same <see cref="PushNotificationStorageRules"/> as <see cref="RedisPushNotificationStorage"/>.
    /// Used by a single platform instance, or when no Redis is configured; the notifications are lost on restart.
    /// </summary>
    public class PushNotificationInMemoryStorage : IPushNotificationStorage
    {
        private readonly object _lockObject = new object();

        private readonly List<PushNotification> _storage = new List<PushNotification>();

        private readonly PushNotificationOptions _options;

        public PushNotificationInMemoryStorage()
            : this(Options.Create(new PushNotificationOptions()))
        {
        }

        public PushNotificationInMemoryStorage(IOptions<PushNotificationOptions> options)
        {
            _options = options?.Value ?? new PushNotificationOptions();
        }

        public virtual void SavePushNotification(PushNotification notification)
        {
            SavePushNotificationAsync(notification).GetAwaiter().GetResult();
        }

        public virtual Task SavePushNotificationAsync(PushNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            lock (_lockObject)
            {
                var alreadyExistNotify = _storage.FirstOrDefault(x => x.Id == notification.Id);
                if (alreadyExistNotify != null)
                {
                    _storage.Remove(alreadyExistNotify);
                    _storage.Add(notification);
                }
                else
                {
                    var lastEvent = _storage.Where(x => x.Creator == notification.Creator).OrderByDescending(x => x.Created).FirstOrDefault();
                    if (!PushNotificationStorageRules.TryCountRepeat(lastEvent, notification))
                    {
                        _storage.Add(notification);
                    }
                }

                RemoveExpired();
                RemoveOverLimit(notification.Creator);
            }

            return Task.CompletedTask;
        }

        public virtual PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria)
        {
            List<PushNotification> userNotifications;
            lock (_lockObject)
            {
                var expiredBefore = GetExpiredBefore();
                userNotifications = _storage.Where(x => x.Creator == userId && (expiredBefore == null || x.Created >= expiredBefore)).ToList();
            }

            return PushNotificationSearch.Search(userNotifications, criteria);
        }

        /// <summary>
        /// Removes the notifications older than <see cref="PushNotificationOptions.RetentionPeriod"/>. Called under the storage lock.
        /// </summary>
        protected virtual void RemoveExpired()
        {
            var expiredBefore = GetExpiredBefore();
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

        private DateTime? GetExpiredBefore() => PushNotificationStorageRules.GetExpiredBefore(_options);
    }
}
