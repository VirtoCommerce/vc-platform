using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Redis
{
    public class RedisPlatformMemoryCache : PlatformMemoryCache
    {
        private static string _instanceId { get; } = $"{Environment.MachineName}_{Guid.NewGuid():N}";
        private bool _isSubscribed;
        private readonly ISubscriber _bus;
        private readonly RedisCachingOptions _redisCachingOptions;
        private readonly IConnectionMultiplexer _connection;
        private readonly ILogger _log;
        private readonly object _lock = new object();
        private bool _disposed;

        public RedisPlatformMemoryCache(IMemoryCache memoryCache
            , IConnectionMultiplexer connection
            , ISubscriber bus
            , IOptions<CachingOptions> cachingOptions
            , IOptions<RedisCachingOptions> redisCachingOptions
            , ILogger<RedisPlatformMemoryCache> log
            ) : base(memoryCache, cachingOptions, log)
        {
            _connection = connection;
            _log = log;
            _bus = bus;

            _redisCachingOptions = redisCachingOptions.Value;

            CancellableCacheRegion.OnTokenCancelled = CacheCancellableTokensRegistry_OnTokenCancelled;
        }

        private void CacheCancellableTokensRegistry_OnTokenCancelled(TokenCancelledEventArgs e)
        {
            var message = new RedisCachingMessage { InstanceId = _instanceId, IsToken = true, CacheKeys = new[] { e.TokenKey } };
            Publish(message);
            _log.LogTrace("Published token cancellation message {Message}", message.ToString());
        }

        protected virtual void OnConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            _log.LogError("Redis disconnected from instance {InstanceId}. Endpoint is {EndPoint}, failure type is {FailureType}", _instanceId, e.EndPoint, e.FailureType);
        }

        protected virtual void OnConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            _log.LogInformation("Redis backplane connection restored for instance {InstanceId}. Endpoint is {EndPoint}", _instanceId, e.EndPoint);

            // We should fully clear cache because we don't know
            // what's changed in another instances since Redis became unavailable
            GlobalCacheRegion.ExpireRegion();
        }


        protected virtual void OnMessage(RedisChannel channel, RedisValue redisValue)
        {
            var message = JsonConvert.DeserializeObject<RedisCachingMessage>(redisValue);

            if (!string.IsNullOrEmpty(message.InstanceId) && !message.InstanceId.EqualsInvariant(_instanceId))
            {
                _log.LogTrace("Received message {Message}", message.ToString());

                foreach (var key in message.CacheKeys?.OfType<string>() ?? Array.Empty<string>())
                {
                    if (message.IsToken)
                    {
                        _log.LogTrace("Trying to cancel token with key: {Key}", key);
                        CancellableCacheRegion.CancelForKey(key, propagate: false);
                    }
                    else
                    {
                        _log.LogTrace("Trying to remove cache entry with key: {Key} from in-memory cache", key);
                        base.Remove(key);
                    }
                }
            }
        }

        public override bool TryGetValue(object key, out object value)
        {
            //We can't do subscription in the ctor due to the fact that it can be called multiple times despite the fact that it registered as a singleton.
            //So we have delayed the connection and subscription to the Redis server until the first cache call.
            EnsureRedisServerConnection();
            return base.TryGetValue(key, out value);
        }

        protected override void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            var message = new RedisCachingMessage { InstanceId = _instanceId, CacheKeys = new[] { key } };
            Publish(message);
            _log.LogTrace("Published message {Message} to the Redis backplane", message);

            base.EvictionCallback(key, value, reason, state);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _bus.Unsubscribe(GetRedisChannel(), null, CommandFlags.FireAndForget);

                    _log.LogInformation("Successfully unsubscribed to Redis backplane channel {ChannelName} with instance id:{InstanceId}", _redisCachingOptions.ChannelName, _instanceId);


                    _connection.ConnectionFailed -= OnConnectionFailed;
                    _connection.ConnectionRestored -= OnConnectionRestored;
                }
                _disposed = true;
            }

            base.Dispose(disposing);
        }

        protected virtual RedisChannel GetRedisChannel()
        {
            return RedisChannel.Literal(_redisCachingOptions.ChannelName);
        }

        private void Publish(RedisCachingMessage message)
        {
            EnsureRedisServerConnection();

            _bus.Publish(GetRedisChannel(), JsonConvert.SerializeObject(message), CommandFlags.FireAndForget);
        }

        private void EnsureRedisServerConnection()
        {
            if (!_isSubscribed)
            {
                lock (_lock)
                {
                    if (!_isSubscribed)
                    {
                        _connection.ConnectionFailed += OnConnectionFailed;
                        _connection.ConnectionRestored += OnConnectionRestored;

                        _bus.Subscribe(GetRedisChannel(), OnMessage, CommandFlags.FireAndForget);

                        _log.LogInformation("Successfully subscribed to Redis backplane channel {ChannelName} with instance id:{InstanceId}", _redisCachingOptions.ChannelName, _instanceId);
                        _isSubscribed = true;
                    }
                }
            }

        }
    }
}
