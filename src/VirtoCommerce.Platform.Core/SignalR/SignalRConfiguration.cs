namespace VirtoCommerce.Platform.Core.SignalR
{
    public static class SignalRConfiguration
    {
        public const string SectionName = "SignalR";
        public const string ScalabilityProvider = nameof(ScalabilityProvider);
        public const string AzureSignalRService = nameof(AzureSignalRService);
        public const string RedisBackplane = nameof(RedisBackplane);
    }
}
