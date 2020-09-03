using System;

namespace VirtoCommerce.Platform.Redis
{
    public class RedisCachingOptions
    {
        public string ChannelName { get; set; }

        [Obsolete("Use Redis connection string parameters for retry policy configration")]
        public int BusRetryCount { get; set; } = 3;
    }
}
