namespace VirtoCommerce.Platform.Redis
{
    public class RedisCachingOptions
    {
        public string ChannelName { get; set; }
        public int BusRetryCount { get; set; } = 3;
    }
}
