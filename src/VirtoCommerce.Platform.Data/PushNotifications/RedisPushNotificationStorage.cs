using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    /// <summary>
    /// Keeps notifications in Redis, shared by all platform instances and kept across restarts, by the same
    /// <see cref="PushNotificationStorageRules"/> as <see cref="PushNotificationInMemoryStorage"/>. Each user (creator) has a sorted set of
    /// notification ids scored by creation time and a hash of the notifications. A save keeps the newest
    /// <see cref="PushNotificationOptions.MaxNotificationsPerUser"/> created within <see cref="PushNotificationOptions.RetentionPeriod"/>,
    /// and both keys expire when the user gets no notification for the retention period. Every operation is one script, so it sees
    /// and changes both keys atomically.
    /// </summary>
    public class RedisPushNotificationStorage : IPushNotificationStorage
    {
        // KEYS: index, data. ARGV: id, score, value, min score to keep ('' keeps all), max count (0 keeps all), ttl seconds (0 = none).
        private const string SaveScript = """
            redis.call('ZADD', KEYS[1], ARGV[2], ARGV[1])
            redis.call('HSET', KEYS[2], ARGV[1], ARGV[3])
            local removed = {}
            if ARGV[4] ~= '' then
              for _, id in ipairs(redis.call('ZRANGEBYSCORE', KEYS[1], '-inf', '(' .. ARGV[4])) do table.insert(removed, id) end
              redis.call('ZREMRANGEBYSCORE', KEYS[1], '-inf', '(' .. ARGV[4])
            end
            local max = tonumber(ARGV[5])
            if max > 0 then
              local count = redis.call('ZCARD', KEYS[1])
              if count > max then
                for _, id in ipairs(redis.call('ZRANGE', KEYS[1], 0, count - max - 1)) do table.insert(removed, id) end
                redis.call('ZREMRANGEBYRANK', KEYS[1], 0, count - max - 1)
              end
            end
            if #removed > 0 then redis.call('HDEL', KEYS[2], unpack(removed)) end
            local ttl = tonumber(ARGV[6])
            if ttl > 0 then
              redis.call('EXPIRE', KEYS[1], ttl)
              redis.call('EXPIRE', KEYS[2], ttl)
            end
            return #removed
            """;

        // KEYS: index, data. ARGV: min score to return ('-inf' returns all). Returns the stored notifications, oldest first.
        private const string SearchScript = """
            local ids = redis.call('ZRANGEBYSCORE', KEYS[1], ARGV[1], '+inf')
            if #ids == 0 then return {} end
            return redis.call('HMGET', KEYS[2], unpack(ids))
            """;

        // KEYS: data. ARGV: triples of id, value read, value marked as read. Replaces a value only if it is still the one read, so a
        // notification saved meanwhile (a job's newer progress) is not overwritten. Returns the ids it replaced.
        private const string MarkAsReadScript = """
            local updated = {}
            for i = 1, #ARGV, 3 do
              if redis.call('HGET', KEYS[1], ARGV[i]) == ARGV[i + 1] then
                redis.call('HSET', KEYS[1], ARGV[i], ARGV[i + 2])
                table.insert(updated, ARGV[i])
              end
            end
            return updated
            """;

        private readonly IConnectionMultiplexer _redis;
        private readonly PushNotificationOptions _options;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<RedisPushNotificationStorage> _logger;

        public RedisPushNotificationStorage(
            IConnectionMultiplexer redis,
            IOptions<PushNotificationOptions> options,
            ILogger<RedisPushNotificationStorage> logger,
            TimeProvider timeProvider = null)
        {
            _redis = redis;
            _options = options?.Value ?? new PushNotificationOptions();
            _logger = logger;
            _timeProvider = timeProvider ?? TimeProvider.System;

            KeyPrefix = $"{PushNotificationRedisOptions.GetChannelName(_options.RedisBackplane)}:PushNotifications";
        }

        /// <summary>
        /// The prefix of every key: <c>{PushNotifications:RedisBackplane:ChannelName}:PushNotifications</c>.
        /// </summary>
        public string KeyPrefix { get; }

        public virtual Task SavePushNotificationAsync(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            return SaveAsync(notification);
        }

        private async Task SaveAsync(PushNotification notification)
        {
            var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
            var ttlSeconds = _options.RetentionPeriod > TimeSpan.Zero ? (long)Math.Ceiling(_options.RetentionPeriod.TotalSeconds) : 0;

            await _redis.GetDatabase().ScriptEvaluateAsync(SaveScript, [GetIndexKey(notification.Creator), GetDataKey(notification.Creator)],
            [
                notification.Id,
                // Scored by the save time, not Created: a job keeps its Created across progress saves, and must stay in the history
                // while it keeps reporting. Display order is by Created and does not depend on the score.
                ToScore(_timeProvider.GetUtcNow().UtcDateTime),
                PushNotificationJsonSerializer.Serialize(notification),
                expiredBefore == null ? string.Empty : ToScore(expiredBefore.Value).ToString(CultureInfo.InvariantCulture),
                PushNotificationStorageRules.GetMaxPerUser(_options),
                ttlSeconds,
            ]);
        }

        public virtual async Task<PushNotificationSearchResult> SearchPushNotificationsAsync(string userId, PushNotificationSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            var stored = await ReadAsync(userId, cancellationToken);

            return PushNotificationSearch.Search(stored.Select(x => x.Notification), criteria);
        }

        public virtual async Task<IList<PushNotification>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
        {
            var stored = await ReadAsync(userId, cancellationToken);

            var values = new List<RedisValue>();
            var unread = new Dictionary<string, PushNotification>();
            foreach (var (value, notification) in stored.Where(x => x.Notification.IsNew))
            {
                notification.IsNew = false;
                values.AddRange([notification.Id, value, PushNotificationJsonSerializer.Serialize(notification)]);
                unread[notification.Id] = notification;
            }

            if (unread.Count == 0)
            {
                return [];
            }

            var updatedIds = (string[])await _redis.GetDatabase().ScriptEvaluateAsync(MarkAsReadScript, [GetDataKey(userId)], [.. values]);

            return updatedIds.Select(id => unread[id]).ToList();
        }

        protected virtual RedisKey GetIndexKey(string creator) => GetKey(creator, "index");

        protected virtual RedisKey GetDataKey(string creator) => GetKey(creator, "data");

        // The braces are a Redis Cluster hash tag: both keys of a user land in one slot, as the scripts require. An empty tag ("{}")
        // would hash the whole key and split the two keys across slots, so a notification without a creator gets a placeholder that
        // the default user name rules do not allow.
        private RedisKey GetKey(string creator, string suffix) => $"{KeyPrefix}:{{{(string.IsNullOrEmpty(creator) ? NoCreatorTag : creator)}}}:{suffix}";

        private const string NoCreatorTag = "(none)";

        // The user's notifications within the retention period, oldest first, with the stored value each was read from.
        private async Task<List<(RedisValue Value, PushNotification Notification)>> ReadAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
            var minScore = expiredBefore == null ? "-inf" : ToScore(expiredBefore.Value).ToString(CultureInfo.InvariantCulture);

            var values = (RedisValue[])await _redis.GetDatabase().ScriptEvaluateAsync(SearchScript, [GetIndexKey(userId), GetDataKey(userId)], [minScore]);

            var result = new List<(RedisValue, PushNotification)>(values.Length);
            foreach (var value in values)
            {
                var notification = Deserialize(value);
                if (notification != null)
                {
                    result.Add((value, notification));
                }
            }

            return result;
        }

        private PushNotification Deserialize(RedisValue value)
        {
            if (value.IsNullOrEmpty)
            {
                return null;
            }

            try
            {
                return PushNotificationJsonSerializer.Deserialize(value);
            }
            catch (Exception ex)
            {
                // For example, the notification type belongs to a module that is no longer installed.
                _logger.LogWarning(ex, "Could not read a push notification stored in Redis");
                return null;
            }
        }

        private static double ToScore(DateTime value)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc)).ToUnixTimeMilliseconds();
        }
    }
}
