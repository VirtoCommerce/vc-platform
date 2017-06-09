using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CacheManager.Core;
using CacheManager.Core.Internal;
using CacheManager.Core.Logging;
using CacheManager.Redis;
using Common.Logging;

namespace VirtoCommerce.Platform.Web.Cache
{
    /// <summary>
    /// This overlapping allow to Redis cache server play role of cache invalidation server (between redistributed instances with memory cache) and avoid to store real data in redistributed cache.
    ///  https://github.com/MichaCo/CacheManager/issues/111
    /// Sample configuration 
    /// <cacheManager.Redis>
    ///    <connections>
    ///        <connection id = "redisConnectionString" database="0" connectionString="redis-host:6380,password=secret,ssl=True,abortConnect=False,allowAdmin=true" />
    ///    </connections>
    ///</cacheManager.Redis>
    ///<cacheManager>
    ///    <managers>
    ///        <cache name = "platformCache" enableStatistics="true" backplaneName="redisConnectionString" backplaneType="VirtoCommerce.Platform.Web.Cache.RedisCacheBackplane2, VirtoCommerce.Platform.Web">
    ///            <handle name = "memCacheHandle" ref="memCacheHandle"  expirationMode="Sliding" timeout="10m" />
    ///            <handle name = "redisConnectionString" ref="redisHandle" isBackplaneSource="true" />
    ///        </cache>          
    ///    </managers>
    ///    <cacheHandles>
    ///        <handleDef id = "redisHandle" type="VirtoCommerce.Platform.Web.Cache.RedisCacheHandle2`1,  VirtoCommerce.Platform.Web" />
    ///        <handleDef id = "memCacheHandle" type="CacheManager.SystemRuntimeCaching.MemoryCacheHandle`1, CacheManager.SystemRuntimeCaching" />
    ///    </cacheHandles>
    ///</cacheManager>    
    ///<system.runtime.caching>
    ///    <memoryCache>
    ///        <namedCaches>
    ///            <add name = "memCacheHandle" physicalMemoryLimitPercentage="80" pollingInterval="00:00:30" />
    ///        </namedCaches>
    ///    </memoryCache>
    ///</system.runtime.caching>
    /// </summary>
    public class RedisCacheHandle2<TCacheValue> : RedisCacheHandle<TCacheValue>
    {
        public RedisCacheHandle2(CacheManagerConfiguration managerConfiguration, CacheHandleConfiguration configuration, ILoggerFactory loggerFactory, ICacheSerializer serializer)
            : base(managerConfiguration, configuration, loggerFactory, serializer)
        {
        }

        protected override CacheItem<TCacheValue> GetCacheItemInternal(string key, string region)
        {
            return null;
        }
        public override UpdateItemResult<TCacheValue> Update(string key, string region, Func<TCacheValue, TCacheValue> updateValue, int maxRetries)
        {
            return UpdateItemResult.ForItemDidNotExist<TCacheValue>();
        }
        protected override void PutInternalPrepared(CacheItem<TCacheValue> item)
        {     
            //Nothing todo   
        }
        protected override bool AddInternalPrepared(CacheItem<TCacheValue> item)
        {          
            return true;
        }
    }

    public class CacheManagerLoggerFactory : ILoggerFactory, ILogger
    {
        private ILog _log;
        public CacheManagerLoggerFactory(ILog log)
        {
            _log = log;
        }

        public IDisposable BeginScope(object state)
        {
            return null;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this;
        }

        public ILogger CreateLogger<T>(T instance)
        {
            return this;
        }

        public bool IsEnabled(CacheManager.Core.Logging.LogLevel logLevel)
        {
            return true;
        }

        public void Log(CacheManager.Core.Logging.LogLevel logLevel, int eventId, object message, Exception exception)
        {
            if (logLevel == CacheManager.Core.Logging.LogLevel.Warning)
            {
                _log.Warn(message, exception);
            }
            else if (logLevel == CacheManager.Core.Logging.LogLevel.Critical || logLevel == CacheManager.Core.Logging.LogLevel.Error)
            {
                _log.Error(message, exception);
            }
            else if (logLevel == CacheManager.Core.Logging.LogLevel.Trace)
            {
                _log.Trace(message, exception);
            }
            else if (logLevel == CacheManager.Core.Logging.LogLevel.Debug)
            {
                _log.Debug(message, exception);
            }
            else if (logLevel == CacheManager.Core.Logging.LogLevel.Trace)
            {
                _log.Trace(message, exception);
            }
            else
            {
                _log.Info(message, exception);
            }
        }
    }   
}