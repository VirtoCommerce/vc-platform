namespace VirtoCommerce.Platform.Redis
{
    public class RedisCachingMessage
    {
        public string Id { get; set; }

        public object[] CacheKeys { get; set; }

        public bool IsPrefix { get; set; }
    }
}
