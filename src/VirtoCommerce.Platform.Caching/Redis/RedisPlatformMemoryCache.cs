using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
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
        private readonly ISubscriber _bus;
        private readonly IMemoryCache _memoryCache;
        private readonly CachingOptions _cachingOptions;
        private readonly RedisCachingOptions _redisCachingOptions;
        private readonly ILogger _log;
        private readonly TelemetryClient _telemetryClient;

        private static readonly string _cacheId = Guid.NewGuid().ToString("N");
        

        public RedisPlatformMemoryCache(IMemoryCache memoryCache, IOptions<CachingOptions> options
            , ConnectionMultiplexer connection
            , ISubscriber bus
            , IOptions<CachingOptions> cachingOptions
            , IOptions<RedisCachingOptions> redisCachingOptions
            , ILogger<RedisPlatformMemoryCache> log
            , TelemetryClient telemetryClient
            ) : base(memoryCache, options, log)
       {
            _memoryCache = memoryCache;
            _log = log;
            _telemetryClient = telemetryClient;
            _bus = bus;

            _cachingOptions = cachingOptions.Value;
            _redisCachingOptions = redisCachingOptions.Value;

            connection.ConnectionFailed += OnConnectionFailed;
            connection.ConnectionRestored += OnConnectionRestored;

            _bus.Subscribe(_redisCachingOptions.ChannelName, OnMessage, CommandFlags.FireAndForget);

            _log.LogInformation($"{nameof(RedisPlatformMemoryCache)}: subscribe to channel {_redisCachingOptions.ChannelName } current instance:{ _cacheId }");
            _telemetryClient.TrackEvent("RedisSubscribed", new Dictionary<string, string>
            {
                {"channelName", _redisCachingOptions.ChannelName},
                {"cacheId", _cacheId}
            });
        }

        protected virtual void OnConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            _log.LogError($"Redis disconnected from instance { _cacheId }. Endpoint is {e.EndPoint}, failure type is {e.FailureType}");
            _telemetryClient.TrackException(e.Exception);
            _telemetryClient.TrackEvent("RedisDisconnected", new Dictionary<string, string>
            {
                {"channelName", _redisCachingOptions.ChannelName},
                {"cacheId", _cacheId},
                {"endpoint", e.EndPoint.ToString()},
                {"failureType", e.FailureType.ToString()}
            });

            CacheEnabled = false;
            GlobalCacheRegion.ExpireRegion();
        }

        protected virtual void OnConnectionRestored(object? sender, ConnectionFailedEventArgs e)
        {
            _log.LogInformation($"Redis connection restored for instance { _cacheId }");
            _telemetryClient.TrackEvent("RedisConnectionRestored", new Dictionary<string, string>
            {
                {"channelName", _redisCachingOptions.ChannelName},
                {"cacheId", _cacheId}
            });

            CacheEnabled = _cachingOptions.CacheEnabled;
            GlobalCacheRegion.ExpireRegion();
        }
        

        protected virtual void OnMessage(RedisChannel channel, RedisValue redisValue)
        {
            var message = JsonConvert.DeserializeObject<RedisCachingMessage>(redisValue);

            if (!string.IsNullOrEmpty(message.Id) && !message.Id.EqualsInvariant(_cacheId))
            {
                foreach (var item in message.CacheKeys)
                {
                    base.Remove(item);

                    _log.LogInformation($"{nameof(RedisPlatformMemoryCache)}: channel[{_redisCachingOptions.ChannelName }] remove local cache that cache key is {item} from instance:{ _cacheId }");
                }
            }
        }

        protected override void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            _log.LogInformation($"{nameof(RedisPlatformMemoryCache)}: channel[{_redisCachingOptions.ChannelName }] sending a message with key:{key} from instance:{ _cacheId } to all subscribers");

            base.EvictionCallback(key, value, reason, state);
        }
    }
}
