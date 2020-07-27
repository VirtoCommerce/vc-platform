namespace VirtoCommerce.Platform.Redis
{
    internal class RedisCachingMessage
    {
        public string Id { get; set; }

        public object[] CacheKeys { get; set; }

        public bool IsPrefix { get; set; }
        public bool IsToken { get; set; }
    }
}
