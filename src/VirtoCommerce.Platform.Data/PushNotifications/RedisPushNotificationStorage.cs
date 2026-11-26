using System;
using System.Collections.Generic;
using System.Globalization;
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
        // KEYS: index, data. ARGV: id. Returns the user's last notification, or nil when the id is already stored or the user has none.
        private const string GetLastUnlessStoredScript = """
            if redis.call('HEXISTS', KEYS[2], ARGV[1]) == 1 then return false end
            local last = redis.call('ZREVRANGE', KEYS[1], 0, 0)
            if #last == 0 then return false end
            return redis.call('HGET', KEYS[2], last[1])
            """;

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

            KeyPrefix = $"{_options.RedisBackplane?.ChannelName ?? new PushNotificationRedisOptions().ChannelName}:PushNotifications";
        }

        /// <summary>
        /// The prefix of every key: <c>{PushNotifications:RedisBackplane:ChannelName}:PushNotifications</c>.
        /// </summary>
        public string KeyPrefix { get; }

        public virtual async Task SavePushNotificationAsync(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            var db = _redis.GetDatabase();
            RedisKey[] keys = [GetIndexKey(notification.Creator), GetDataKey(notification.Creator)];

            var toSave = notification;
            var last = Deserialize((RedisValue)await db.ScriptEvaluateAsync(GetLastUnlessStoredScript, keys, [notification.Id]));
            if (PushNotificationStorageRules.TryCountRepeat(last, notification, _timeProvider))
            {
                toSave = last;
            }

            var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
            var ttlSeconds = _options.RetentionPeriod > TimeSpan.Zero ? (long)Math.Ceiling(_options.RetentionPeriod.TotalSeconds) : 0;

            await db.ScriptEvaluateAsync(SaveScript, keys,
            [
                toSave.Id,
                ToScore(toSave.Created),
                PushNotificationJsonSerializer.Serialize(toSave),
                expiredBefore == null ? string.Empty : ToScore(expiredBefore.Value).ToString(CultureInfo.InvariantCulture),
                PushNotificationStorageRules.GetMaxPerUser(_options),
                ttlSeconds,
            ]);
        }

        public virtual async Task<PushNotificationSearchResult> SearchPushNotificationsAsync(string userId, PushNotificationSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var expiredBefore = PushNotificationStorageRules.GetExpiredBefore(_options, _timeProvider);
            var minScore = expiredBefore == null ? "-inf" : ToScore(expiredBefore.Value).ToString(CultureInfo.InvariantCulture);

            var values = (RedisValue[])await _redis.GetDatabase().ScriptEvaluateAsync(SearchScript, [GetIndexKey(userId), GetDataKey(userId)], [minScore]);

            var notifications = new List<PushNotification>(values.Length);
            foreach (var value in values)
            {
                var notification = Deserialize(value);
                if (notification != null)
                {
                    notifications.Add(notification);
                }
            }

            return PushNotificationSearch.Search(notifications, criteria);
        }

        protected virtual RedisKey GetIndexKey(string creator) => GetKey(creator, "index");

        protected virtual RedisKey GetDataKey(string creator) => GetKey(creator, "data");

        // The braces are a Redis Cluster hash tag: both keys of a user land in one slot, as the scripts require.
        private RedisKey GetKey(string creator, string suffix) => $"{KeyPrefix}:{{{creator}}}:{suffix}";

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
