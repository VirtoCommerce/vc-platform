using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    /// <summary>
    /// Keeps notifications in Redis, shared by all platform instances and kept across restarts, by the same <see cref="PushNotificationStorageRules"/>
    /// as <see cref="PushNotificationInMemoryStorage"/>. Each user (creator) has a sorted set of
    /// notification ids scored by creation time and a hash of the notifications. A save keeps the newest
    /// <see cref="PushNotificationOptions.MaxNotificationsPerUser"/> created within <see cref="PushNotificationOptions.RetentionPeriod"/>,
    /// and both keys expire when the user gets no notification for the retention period.
    /// </summary>
    public class RedisPushNotificationStorage : IPushNotificationStorage
    {
        public const string DefaultChannelName = "VirtoCommerceChannel";

        // KEYS: index (sorted set), data (hash). ARGV: id, score, value, min score to keep ('' keeps all), max count (0 keeps all), ttl seconds (0 = none).
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

        private readonly IConnectionMultiplexer _redis;
        private readonly PushNotificationOptions _options;
        private readonly ILogger<RedisPushNotificationStorage> _logger;

        public RedisPushNotificationStorage(
            IConnectionMultiplexer redis,
            IOptions<PushNotificationOptions> options,
            IConfiguration configuration,
            ILogger<RedisPushNotificationStorage> logger)
        {
            _redis = redis;
            _options = options?.Value ?? new PushNotificationOptions();
            _logger = logger;

            // The instances of one environment share the channel name; another environment on the same Redis sets its own.
            var channelName = configuration?["PushNotifications:RedisBackplane:ChannelName"].EmptyToNull() ?? DefaultChannelName;
            KeyPrefix = $"{channelName}:PushNotifications";
        }

        public string KeyPrefix { get; }

        public virtual void SavePushNotification(PushNotification notification)
        {
            SavePushNotificationAsync(notification).GetAwaiter().GetResult();
        }

        public virtual async Task SavePushNotificationAsync(PushNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            var db = _redis.GetDatabase();
            var creator = notification.Creator;
            var toSave = notification;

            if (!await db.HashExistsAsync(GetDataKey(creator), notification.Id))
            {
                var last = await GetLastAsync(db, creator);
                if (PushNotificationStorageRules.TryCountRepeat(last, notification))
                {
                    toSave = last;
                }
            }

            var expiredBefore = GetExpiredBefore();
            var ttlSeconds = _options.RetentionPeriod > TimeSpan.Zero ? (long)Math.Ceiling(_options.RetentionPeriod.TotalSeconds) : 0;

            await db.ScriptEvaluateAsync(SaveScript,
                [GetIndexKey(creator), GetDataKey(creator)],
                [
                    toSave.Id,
                    ToScore(toSave.Created),
                    PushNotificationJsonSerializer.Serialize(toSave),
                    expiredBefore == null ? string.Empty : ToScore(expiredBefore.Value).ToString(CultureInfo.InvariantCulture),
                    PushNotificationStorageRules.GetMaxPerUser(_options),
                    ttlSeconds,
                ]);
        }

        public virtual PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria)
        {
            var db = _redis.GetDatabase();
            var expiredBefore = GetExpiredBefore();
            var ids = db.SortedSetRangeByScore(GetIndexKey(userId), expiredBefore == null ? double.NegativeInfinity : ToScore(expiredBefore.Value));

            var notifications = new List<PushNotification>();
            if (ids.Length > 0)
            {
                notifications.AddRange(db.HashGet(GetDataKey(userId), ids).Select(Deserialize).Where(x => x != null));
            }

            return PushNotificationSearch.Search(notifications, criteria);
        }

        protected virtual RedisKey GetIndexKey(string creator) => GetKey(creator, "index");

        protected virtual RedisKey GetDataKey(string creator) => GetKey(creator, "data");

        // The braces are a Redis Cluster hash tag: both keys of a user land in one slot, as the save script requires.
        private RedisKey GetKey(string creator, string suffix) => $"{KeyPrefix}:{{{creator}}}:{suffix}";

        private async Task<PushNotification> GetLastAsync(IDatabase db, string creator)
        {
            var lastIds = await db.SortedSetRangeByRankAsync(GetIndexKey(creator), 0, 0, Order.Descending);
            if (lastIds.Length == 0)
            {
                return null;
            }

            return Deserialize(await db.HashGetAsync(GetDataKey(creator), lastIds[0]));
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

        private DateTime? GetExpiredBefore() => PushNotificationStorageRules.GetExpiredBefore(_options);

        private static double ToScore(DateTime value)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc)).ToUnixTimeMilliseconds();
        }
    }
}
